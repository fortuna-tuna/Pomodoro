using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;
using Pomodoro.Common.Helpers;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
            if (_isConnected && _tcpClient != null && _tcpClient.Connected)
                return;

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
                    OnMessageReceived?.Invoke(ex.Message);
                }
            }
        }

        private Task HandleIncomingMessage(NetworkMessage message)
        {
            switch (message.Type)
            {
                case MessageType.ShowNotification:
                    OnNotificationReceived?.Invoke(message.Payload ?? string.Empty);
                    break;

                case MessageType.ErrorResponce:
                    OnMessageReceived?.Invoke(message.Payload ?? string.Empty);
                    break;

                case MessageType.TimerChangeState:
                    OnTimerStateChanged?.Invoke(message.Payload ?? string.Empty);
                    break;

                case MessageType.TimerTick:
                    if (int.TryParse(message.Payload, out int remainingSeconds))
                    {
                        OnTick?.Invoke(remainingSeconds);
                    }
                    break;

                default:
                    OnMessageReceived?.Invoke(message.Payload ?? string.Empty);
                    break;
            }

            return Task.CompletedTask;
        }

        private async Task EnsureConnectedAsync()
        {
            if (!_isConnected || _tcpClient == null || !_tcpClient.Connected)
            {
                await ConnectAsync("127.0.0.1", 1234);
            }

            if (_stream == null)
                throw new InvalidOperationException();
        }

        public async Task StartPomodoroAsync(int pomodoroDuration, int shortBreak, int longBreak, int sessions)
        {
            await EnsureConnectedAsync();

            var settingsDto = new PomodoroSettingsDTO
            {
                PomodoroDuration = pomodoroDuration,
                ShortBreak = shortBreak,
                LongBreak = longBreak
            };

            await NetworkHelper.WriteExactAsync(_stream!, MessageType.StartPomodoro, settingsDto);
        }

        public async Task SaveSettingsAsync(int pomodoroDuration, int shortBreak, int longBreak, int sessions)
        {
            await EnsureConnectedAsync();

            var settingsDto = new SaveSettingsDTO
            {
                // ТУТ должey быть логин!
                // ...

                PomodoroSettings = new PomodoroSettingsDTO
                {
                    PomodoroDuration = pomodoroDuration,
                    ShortBreak = shortBreak,
                    LongBreak = longBreak,
                    CountPomodoroBeforeLongBreak = sessions
                }
            };
            
            await NetworkHelper.WriteExactAsync(_stream!, MessageType.SaveSettings, settingsDto);
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