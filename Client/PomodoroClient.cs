using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;
using Pomodoro.Common.Helpers;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class PomodoroClient
    {
        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        private bool _isConnected;
        private CancellationTokenSource? _cts;

        public event Action<string>? OnMessageReceived;
        public event Action<int>? OnTick;
        public event Action<string>? OnTimerStateChanged;
        public event Action<string>? OnNotificationReceived;

        public async Task ConnectAsync(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(ipAddress, port);
            _stream = _tcpClient.GetStream();
            _isConnected = true;
            _cts = new CancellationTokenSource();

            _ = ListenAsync(_cts.Token);
        }

        private async Task ListenAsync(CancellationToken token)
        {
            try
            {
                while (_isConnected && _stream != null && !token.IsCancellationRequested)
                {
                    var buf = await NetworkHelper.ReadDataAsync(_stream, token);
                    if (buf == null) break;

                    var networkMessage = NetworkSerializer.DeserializeMessage(buf);
                    if (networkMessage != null)
                    {
                        await HandleIncomingMessage(networkMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    OnMessageReceived?.Invoke($"Помилка з'єднання: {ex.Message}");
                }
            }
        }

        private Task HandleIncomingMessage(NetworkMessage message)
        {
            switch (message.Type)
            {
                case MessageType.ShowNotification:
                    OnNotificationReceived?.Invoke(message.Payload);
                    MessageBox.Show(message.Payload, "Сповіщення сервера", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case MessageType.ErrorResponce:
                    OnMessageReceived?.Invoke($"Помилка від сервера: {message.Payload}");
                    break;

                case MessageType.TimerChangeState:
                    OnTimerStateChanged?.Invoke(message.Payload);
                    break;

                case MessageType.TimerTick:
                    if (int.TryParse(message.Payload, out int remainingSeconds))
                    {
                        OnTick?.Invoke(remainingSeconds);
                    }
                    break;

                default:
                    OnMessageReceived?.Invoke($"Отримано невідомий тип повідомлення: {message.Payload}");
                    break;
            }

            return Task.CompletedTask;
        }

        public async Task SendSettingsAsync(int pomodoroDuration, int shortBreak, int longBreak, int sessions)
        {
            var settingsDto = new PomodoroSettingsDTO
            {
                PomodoroDuration = pomodoroDuration,
                ShortBreak = shortBreak,
                LongBreak = longBreak
            };

            if (_stream == null || !_isConnected)
                throw new InvalidOperationException("Клієнт не підключений до сервера.");

            await NetworkHelper.WriteExactAsync(_stream, MessageType.StartPomodoro, settingsDto);
        }

        public void Disconnect()
        {
            _isConnected = false;
            _cts?.Cancel();
            _stream?.Close();
            _tcpClient?.Close();
        }
    }
}