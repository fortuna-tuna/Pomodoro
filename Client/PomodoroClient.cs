using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client
{
    public class PomodoroClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private bool _isConnected;

        public event Action<string> OnMessageReceived;

        public async Task ConnectAsync(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(ipAddress, port);
            _stream = _tcpClient.GetStream();
            _isConnected = true;

            _ = ListenAsync();
        }

        private async Task ListenAsync()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (_isConnected)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                OnMessageReceived?.Invoke($"Помилка з'єднання: {ex.Message}");
            }
        }

        public async Task SendSettingsAsync(int workDuration, int shortBreak)
        {
            if (_stream == null) return;

            var request = new
            {
                Command = "UpdateSettings",
                WorkDuration = workDuration,
                ShortBreakDuration = shortBreak
            };

            string json = JsonSerializer.Serialize(request);
            byte[] data = Encoding.UTF8.GetBytes(json);

            await _stream.WriteAsync(data, 0, data.Length);
        }

        public void Disconnect()
        {
            _isConnected = false;
            _stream?.Close();
            _tcpClient?.Close();
        }
    }
}