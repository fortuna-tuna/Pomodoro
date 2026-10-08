using Pomodoro.Common.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Pomodoro.Common;

namespace Client
{
    public class PomodoroClient
    {
        private readonly string _serverIp;
        private readonly int _serverPort;
        
        private TcpClient _tcpClient = new TcpClient();

        public PomodoroClient(string serverIp, int serverPort)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public async Task SendNetworkMessageAsync(NetworkMessage message)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_serverIp, _serverPort);
            }

            
            string jsonMessage = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(jsonMessage);

            NetworkStream stream = _tcpClient.GetStream();

            
            await stream.WriteAsync(data, 0, data.Length);
            await stream.FlushAsync();
        }
    }
}