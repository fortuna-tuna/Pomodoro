using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class PomodoroClient
    {
        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        public event Action<string>? OnMessageReceived;

        public async Task ConnectAsync(string ip, int port)
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(ip, port);
            _stream = _tcpClient.GetStream();
        }

        public async Task SendMessageAsync<T>(int messageType, T payloadObject)
        {
            if (_stream == null) return;

            string payloadJson = JsonSerializer.Serialize(payloadObject);

            var message = new
            {
                Type = messageType,
                Payload = payloadJson
            };

            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            byte[] lengthPrefix = BitConverter.GetBytes(payloadBytes.Length);

            await _stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
            await _stream.WriteAsync(payloadBytes, 0, payloadBytes.Length);
        }

        public async Task ListenServerAsync(CancellationToken ct)
        {
            if (_stream == null) return;

            byte[] lengthBuffer = new byte[4];

            while (!ct.IsCancellationRequested)
            {
                int bytesRead = await ReadExactAsync(_stream, lengthBuffer, 0, 4, ct);
                if (bytesRead < 4) break;

                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                byte[] payloadBuffer = new byte[messageLength];
                await ReadExactAsync(_stream, payloadBuffer, 0, messageLength, ct);

                string json = Encoding.UTF8.GetString(payloadBuffer);
                OnMessageReceived?.Invoke(json);
            }
        }

        private async Task<int> ReadExactAsync(NetworkStream stream, byte[] buffer, int offset, int count, CancellationToken ct)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = await stream.ReadAsync(buffer, offset + totalRead, count - totalRead, ct);
                if (read == 0) break;
                totalRead += read;
            }
            return totalRead;
        }
    }
}