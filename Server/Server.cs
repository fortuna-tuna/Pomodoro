using Pomodoro.Common.Helpers;
using Pomodoro.Common.Models;
using Pomodoro.Common.Enum;
using Pomodoro.DAL;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Server
    {
        TcpListener _listener;
        TcpClient _client;
        CancellationTokenSource _cancellationTokenSource;
        PomodoroTimer _timer;
        int _countCompletePomodoros = 0;

        public Server()
        {
            PomodoroSettingsDTO defaultSettings = new()
            {
                PomodoroDuration = 25,
                ShortBreak = 5,
                LongBreak = 15,
                CountPomodoroBeforeLongBreak = 4
            };

            _timer = new PomodoroTimer(defaultSettings);

            _timer.OnTimerState += async (state) => await SendStateAsync(state);
            _timer.OnTick += async (remainingSeconds) => await SendTickAsync(remainingSeconds);
            _timer.OnTimerCompleted += async (completeState) => await HandleTimerCompleteAsync(completeState);
        }
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
                    _timer.UpdateSettings(pomodoroSettings);
                    _ = _timer.Start(TimerStates.Working);
                    break;
                case MessageType.StopPomodoro:
                    _countCompletePomodoros = 0;
                    _timer.Stop();
                    break;
            }
        }

        private async Task SendStateAsync(TimerStates state)
        {
            var dto = new TimeStateDTO
            {
                State = state,
                StateDuration = state switch
                {
                    TimerStates.Working => _timer.PomodoroSettings.PomodoroDuration,
                    TimerStates.ShortBreak => _timer.PomodoroSettings.ShortBreak,
                    TimerStates.LongBreak => _timer.PomodoroSettings.LongBreak,
                    _ => 0
                }
            };
            var message = NetworkSerializer.SerializeMessage(MessageType.TimerChangeState, dto);

            var stream = _client.GetStream();
            await stream.WriteAsync(message);
        }

        private async Task SendTickAsync(int remainingSeconds)
        {
            var dto = new TimerTickDTO() { RemainingSeconds = remainingSeconds };
            var message = NetworkSerializer.SerializeMessage(MessageType.TimerTick, dto);

            var stream = _client.GetStream();
            await stream.WriteAsync(message);

        }

        private async Task HandleTimerCompleteAsync(TimerStates completeState)
        {
            if (completeState == TimerStates.Working)
            {
                _countCompletePomodoros++;

                // Database
                var dbContext = new PomodoroDbContext();

                if(_countCompletePomodoros % _timer.PomodoroSettings.CountPomodoroBeforeLongBreak == 0)
                {
                    await _timer.Start(TimerStates.LongBreak);
                } else
                {
                    await _timer.Start(TimerStates.ShortBreak);
                }
            }
            else
            {
                await _timer.Start(TimerStates.Working);
            }
        }
    }
}
