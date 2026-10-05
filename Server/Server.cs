using Pomodoro.Common.Helpers;
using Pomodoro.Common.Models;
using Pomodoro.Common.Enum;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Pomodoro.Common.Enum;

namespace Server
{
    internal class Server
    {
        TcpListener _listener;
        TcpClient _client;
        CancellationTokenSource _cancellationTokenSource;

        public async Task StartServer(string host, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(host), port);
            _listener.Start();
            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync(_cancellationTokenSource.Token);
                    _client = client;
                    _ = Task.Run(() => ClientHandlerAsync(client, _cancellationTokenSource.Token));
                } catch (Exception ex)
                {
                    // ...
                }
            }
        }

        public async Task ClientHandlerAsync(TcpClient client, CancellationToken token)
        {
            using (client)
            {
                var stream = client.GetStream();
                byte[] lengthBuffer = new byte[4];
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        int bytesRead = await NetworkHelper.ReadExactAsync(stream, lengthBuffer, 0, 4, token);
                        if (bytesRead < 4) break;

                        var messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                        var buf = new byte[messageLength];
                        await NetworkHelper.ReadExactAsync(stream, buf, 0, messageLength, token);

                        HandleIncomingMessage(NetworkSerializer.DeserializeMessage(buf));
                    } catch
                    {
                        // ...
                    }
                    
                }
            }
        }

        private void HandleIncomingMessage(NetworkMessage message)
        {
            switch (message.Type)
            {
                case MessageType.StartPomodoro:
                    var pomodoroSettings = NetworkSerializer.DeserializePayload<PomodoroSettingsDTO>(message.Payload);

                    // ...

                    break;
                case MessageType.StopPomodoro:

                    // ...

                    break;
                case MessageType.PausePomodoro:

                    // ...

                    break;

            }
        }
    }
}
