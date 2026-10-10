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
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class Server
    {
        TcpListener _listener;
        CancellationTokenSource _cancellationTokenSource;
        PomodoroTimer _timer;
        int _countCompletePomodoros = 0;
        PomodoroSettingsDTO _defaultSettings = new()
        {
            PomodoroDuration = 25,
            ShortBreak = 5,
            LongBreak = 15,
            CountPomodoroBeforeLongBreak = 4
        };

        public Server()
        {
            _timer = new PomodoroTimer(_defaultSettings);

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
                    _ = Task.Run(() => ClientHandlerAsync(client, _cancellationTokenSource.Token));
                } catch (Exception ex)
                {
                    Console.WriteLine($"Connect error. {ex}");
                }
            }
        }

        private async Task ClientHandlerAsync(TcpClient client, CancellationToken token)
        {
            Action<TimerStates> stateHandler = async (state) => await SendStateAsync(client, state);
            Action<TimerStates> timerCompleteHandler = async (completeState) => await HandleTimerCompleteAsync(client, completeState);
            Action<int> tickHandler = async (remainingSeconds) => await SendTickAsync(client, remainingSeconds);

            using (client)
            {
                _timer.OnTimerState += stateHandler;
                _timer.OnTimerCompleted += timerCompleteHandler;
                _timer.OnTick += tickHandler;

                var stream = client.GetStream();
                byte[] lengthBuffer = new byte[4];
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        var buf = await NetworkHelper.ReadDataAsync(stream, token);
                        if (buf == null) break;

                        await HandleIncomingMessage(client, NetworkSerializer.DeserializeMessage(buf));
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine($"Client error. {ex}");
                }
                finally
                {
                    _timer.OnTimerState -= stateHandler;
                    _timer.OnTimerCompleted -= timerCompleteHandler;
                    _timer.OnTick -= tickHandler;
                }

            }
        }

        private async Task HandleIncomingMessage(TcpClient client, NetworkMessage message)
        {
            using PomodoroDbContext dbContext = new();

            switch (message.Type)
            {
                case MessageType.StartPomodoro:
                    var pomodoroSettings = NetworkSerializer.DeserializePayload<PomodoroSettingsDTO>(message.Payload);
                    _timer.UpdateSettings(pomodoroSettings);
                    _ = _timer.Start(TimerStates.Working);
                    break;
                case MessageType.StopPomodoro:
                    await dbContext.Statistics.AddAsync(new Statistic { TotalCompletePomodoros = _countCompletePomodoros, Day = DateTime.Now.Date});
                    await dbContext.SaveChangesAsync();

                    _countCompletePomodoros = 0;
                    _timer.Stop();
                    break;
                case MessageType.SaveSettings:
                    var userSettings = NetworkSerializer.DeserializePayload<SaveSettingsDTO>(message.Payload);
                    await SaveSettingsToDatabase(userSettings);
                    await SendNotificationAsync(client, "Save settings success");
                    break;
                case MessageType.GetSettings:
                    var userRequest = NetworkSerializer.DeserializePayload<GetSettingsDTO>(message.Payload);
                    var (settings, isFirstTime) = await GetOrInitializeSettings(client, userRequest.Login);

                    await NetworkHelper.WriteExactAsync(client.GetStream(), MessageType.GetSettings, settings);

                    if (!isFirstTime)
                    {
                        await SendNotificationAsync(client, "Settings download success!");
                    } else
                    {
                        await SendNotificationAsync(client, "Default settings were applied.");
                    }
                    break;
            }
        }

        private async Task SendStateAsync(TcpClient client, TimerStates state)
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

            await NetworkHelper.WriteExactAsync(client.GetStream(), MessageType.TimerChangeState, dto);
        }

        private async Task SendTickAsync(TcpClient client, int remainingSeconds)
        {
            var dto = new TimerTickDTO() { RemainingSeconds = remainingSeconds };
            await NetworkHelper.WriteExactAsync(client.GetStream(), MessageType.TimerTick, dto);
        }

        private async Task HandleTimerCompleteAsync(TcpClient client, TimerStates completeState)
        {
            if (completeState == TimerStates.Working)
            {
                _countCompletePomodoros++;

                if(_countCompletePomodoros % _timer.PomodoroSettings.CountPomodoroBeforeLongBreak == 0)
                {
                    await SendNotificationAsync(client, "Long break has been started!");
                    await _timer.Start(TimerStates.LongBreak);
                } else
                {
                    await SendNotificationAsync(client, "Short break has been started!");
                    await _timer.Start(TimerStates.ShortBreak);
                }
            }
            else
            {
                await SendNotificationAsync(client, "Time to work!");
                await _timer.Start(TimerStates.Working);
            }
        }

        private async Task SendNotificationAsync(TcpClient client, string message)
        {
            try
            {
                var notificationDto = new NotificationDTO
                {
                    NotificationText = message
                };

                await NetworkHelper.WriteExactAsync(client.GetStream(), MessageType.ShowNotification, notificationDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification error. {ex}");
            }
        }
        private async Task SaveSettingsToDatabase(SaveSettingsDTO userSettings)
        {
            using PomodoroDbContext dbContext = new();
            var res = await dbContext.PomodoroSettings.Where(ps => ps.UserProfileId == userSettings.UserId).FirstOrDefaultAsync();
            if (res != null)
            {
                res.PomodoroDuration = userSettings.PomodoroSettings.PomodoroDuration;
                res.ShortBreak = userSettings.PomodoroSettings.ShortBreak;
                res.LongBreak = userSettings.PomodoroSettings.LongBreak;
                res.CountPomodoroBeforeLongBreak = userSettings.PomodoroSettings.CountPomodoroBeforeLongBreak;

            } else
            {
                await dbContext.PomodoroSettings.AddAsync(new PomodoroSettings
                {
                    UserProfileId = userSettings.UserId,
                    PomodoroDuration = userSettings.PomodoroSettings.PomodoroDuration,
                    ShortBreak = userSettings.PomodoroSettings.ShortBreak,
                    LongBreak = userSettings.PomodoroSettings.LongBreak,
                    CountPomodoroBeforeLongBreak = userSettings.PomodoroSettings.CountPomodoroBeforeLongBreak
                });
            }
            
            await dbContext.SaveChangesAsync();
        }

        private async Task<(GetSettingsDTO Settings, bool IsFirstTime)> GetOrInitializeSettings(TcpClient client, string login)
        {
            using PomodoroDbContext dbContext = new();
            var user = await dbContext.UserProfiles.FirstOrDefaultAsync(up => up.Login == login);

            if (user == null)
            {
                await SendNotificationAsync(client, "Need authorize");

                var settings = new GetSettingsDTO
                {
                    Login = "Unknown",
                    Settings = _defaultSettings
                };

                return (settings, true);
            }

            var userSettings = await dbContext.PomodoroSettings.FirstOrDefaultAsync(ps => ps.UserProfileId == user.Id);

            if (userSettings == null)
            {
                var defSettings = new PomodoroSettings
                {
                    UserProfileId = user.Id,
                    PomodoroDuration = _defaultSettings.PomodoroDuration,
                    ShortBreak = _defaultSettings.ShortBreak,
                    LongBreak = _defaultSettings.LongBreak,
                    CountPomodoroBeforeLongBreak = _defaultSettings.CountPomodoroBeforeLongBreak,
                };

                await dbContext.PomodoroSettings.AddAsync(defSettings);
                await dbContext.SaveChangesAsync();

                var settings = new GetSettingsDTO
                {
                    Login = user.Login,
                    Settings = _defaultSettings
                };

                return (settings, true);
            }

            var settingsDTO = new GetSettingsDTO
            {
                Login = user.Login,
                Settings = new PomodoroSettingsDTO
                {
                    PomodoroDuration = userSettings.PomodoroDuration,
                    ShortBreak = userSettings.ShortBreak,
                    LongBreak = userSettings.LongBreak,
                    CountPomodoroBeforeLongBreak = userSettings.CountPomodoroBeforeLongBreak
                }
            };

            return (settingsDTO, false);
        }
    }
}
