using System;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Client
{
    public partial class Form1 : Form
    {
        private PomodoroClient _pomodoroClient;

        public Form1()
        {
            InitializeComponent();

            _pomodoroClient = new PomodoroClient();

            _pomodoroClient.OnMessageReceived += (message) => {
                UpdateStatus(message);
            };

            _pomodoroClient.OnTick += (remainingSeconds) => {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => FormatAndDisplayTime(remainingSeconds)));
                }
                else
                {
                    FormatAndDisplayTime(remainingSeconds);
                }
            };

            _pomodoroClient.OnTimerStateChanged += (state) => {
                UpdateStatus($"Стан таймера: {state}");
            };

            _ = ConnectToServerAsync();
        }

        private async Task ConnectToServerAsync()
        {
            try
            {
                await _pomodoroClient.ConnectAsync("127.0.0.1", 1234);
                UpdateStatus("Статус: Підключено до сервера");
            }
            catch (Exception)
            {
                UpdateStatus("Статус: Очікування підключення");
            }
        }

        private void UpdateStatus(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => lblStatus.Text = message));
            }
            else
            {
                lblStatus.Text = message;
            }
        }

        private void FormatAndDisplayTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            lblStatus.Text = $"Час: {minutes:D2}:{seconds:D2}";
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                int workTime = (int)numWorkDuration.Value;
                int shortBreak = (int)numShortBreak.Value;
                int longBreak = (int)numLongBreak.Value;
                int sessions = (int)numSessions.Value;

                await _pomodoroClient.SendSettingsAsync(workTime, shortBreak, longBreak, sessions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка старту: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _pomodoroClient.Disconnect();
                UpdateStatus("Статус: Зупинено");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зупинки: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                int workTime = (int)numWorkDuration.Value;
                int shortBreak = (int)numShortBreak.Value;
                int longBreak = (int)numLongBreak.Value;
                int sessions = (int)numSessions.Value;

                await _pomodoroClient.SendSettingsAsync(workTime, shortBreak, longBreak, sessions);
                MessageBox.Show("Налаштування успішно надіслано на сервер!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void btnStart_Click_1(object sender, EventArgs e) => btnStart_Click(sender, e);
        private void btnStop_Click_1(object sender, EventArgs e) => btnStop_Click(sender, e);
        private void btnSaveSettings_Click_1(object sender, EventArgs e) => btnSaveSettings_Click(sender, e);
    }
}