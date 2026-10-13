using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using Pomodoro.Common.Models;

namespace Client
{
    public partial class Form1 : Form
    {
        private PomodoroClient _pomodoroClient;

        public Form1()
        {
            InitializeComponent();

            _pomodoroClient = new PomodoroClient();

            SetSettingsAccess(false);

           
            _pomodoroClient.OnTimerStateChanged += (state) => {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateStatus($"Стан: {state}")));
                }
                else
                {
                    UpdateStatus($"Стан: {state}");
                }
            };

            
            _pomodoroClient.OnAuthorized += (isSuccess) => {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => HandleAuthResult(isSuccess)));
                }
                else
                {
                    HandleAuthResult(isSuccess);
                }
            };

            
            _pomodoroClient.OnSettingsReceived += (settings) => {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ApplySettingsToUI(settings)));
                }
                else
                {
                    ApplySettingsToUI(settings);
                }
            };

            _ = ConnectToServerAsync();
        }

        private async Task ConnectToServerAsync()
        {
            try
            {
                await _pomodoroClient.ConnectAsync("127.0.0.1", 1234);
                UpdateStatus("Статус: Підключено до сервера. Будь ласка, увійдіть.");
            }
            catch (Exception)
            {
                UpdateStatus("Статус: Очікування підключення...");
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

        private void SetSettingsAccess(bool allowed)
        {
            numWorkDuration.Enabled = allowed;
            numShortBreak.Enabled = allowed;
            numLongBreak.Enabled = allowed;
            numSessions.Enabled = allowed;
            btnSaveSettings.Enabled = allowed;
        }

        private void HandleAuthResult(bool isSuccess)
        {
            if (isSuccess)
            {
                SetSettingsAccess(true);
                MessageBox.Show("Успішний вхід!", "Авторизація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetSettingsAccess(false);
            }
        }

        private void ApplySettingsToUI(PomodoroSettingsDTO settings)
        {
            numWorkDuration.Value = Math.Clamp(settings.PomodoroDuration, numWorkDuration.Minimum, numWorkDuration.Maximum);
            numShortBreak.Value = Math.Clamp(settings.ShortBreak, numShortBreak.Minimum, numShortBreak.Maximum);
            numLongBreak.Value = Math.Clamp(settings.LongBreak, numLongBreak.Minimum, numLongBreak.Maximum);
            numSessions.Value = Math.Clamp(settings.CountPomodoroBeforeLongBreak, numSessions.Minimum, numSessions.Maximum);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string login = txtLogin.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введіть логін та пароль!");
                    return;
                }

                await _pomodoroClient.AuthorizeAsync(login, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                int workTime = (int)numWorkDuration.Value;
                int shortBreak = (int)numShortBreak.Value;
                int longBreak = (int)numLongBreak.Value;
                int sessions = (int)numSessions.Value;

                
                await _pomodoroClient.SaveSettingsAsync(workTime, shortBreak, longBreak, sessions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка старту: {ex.Message}");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _pomodoroClient.Disconnect();
                UpdateStatus("Статус: Зупинено");
                SetSettingsAccess(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зупинки: {ex.Message}");
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

                if (_pomodoroClient != null)
                {
                    await _pomodoroClient.SaveSettingsAsync(workTime, shortBreak, longBreak, sessions);
                    MessageBox.Show("Налаштування збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Немає підключення до сервера");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }
    }
}