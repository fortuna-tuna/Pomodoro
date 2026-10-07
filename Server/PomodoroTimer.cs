using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;
using Pomodoro.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class PomodoroTimer
    {
        public PomodoroSettingsDTO PomodoroSettings { get; private set; }

        public int RemainingSeconds { get; private set; }
        public TimerStates CurrentState { get; set; } = TimerStates.Stopped;
        public bool IsRunning { get; private set; }

        public event Action<int>? OnTick;
        public event Action<TimerStates>? OnTimerState;
        public event Action<TimerStates>? OnTimerCompleted;

        private CancellationTokenSource? _cancellationTokenSource;

        public PomodoroTimer(PomodoroSettingsDTO pomodoro)
        {
            PomodoroSettings = pomodoro;
        }

        public async Task Start(TimerStates state, PomodoroSettingsDTO newSettings = null)
        {
            if (newSettings != null)
                UpdateSettings(newSettings);

            Stop();

            CurrentState = state;
            OnTimerState?.Invoke(CurrentState);

            int minutes = state switch
            {
                TimerStates.Working => PomodoroSettings.PomodoroDuration,
                TimerStates.ShortBreak => PomodoroSettings.ShortBreak,
                TimerStates.LongBreak => PomodoroSettings.LongBreak,
                _ => 0
            };

            if (minutes > 0)
            {
                await RunTimerAsync(minutes);
            }
        }

        private async Task RunTimerAsync(int minutes)
        {
            RemainingSeconds = minutes * 60;
            IsRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested && RemainingSeconds > 0)
                {
                    await Task.Delay(1000, _cancellationTokenSource.Token);
                    RemainingSeconds--;
                    OnTick?.Invoke(RemainingSeconds);
                }

                if (RemainingSeconds == 0 && !_cancellationTokenSource.IsCancellationRequested)
                {
                    IsRunning = false;
                    OnTimerCompleted?.Invoke(CurrentState);
                }
                    

            }
            catch (Exception ex)
            {
                //...
            }
            finally
            {
                IsRunning = false;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            IsRunning = false;
        }

        public void UpdateSettings(PomodoroSettingsDTO newSettings)
        {
            PomodoroSettings = newSettings;
        }
    }
}