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

            
            SetSettingsAccess(false);

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
                UpdateStatus($"Стан: {state}");
            };

            _pomodoroClient.OnNotificationReceived += (notification) => {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => MessageBox.Show(notification, "Сповіщення сервера", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                }
                else
                {
                    MessageBox.Show(notification, "Сповіщення сервера", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void FormatAndDisplayTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            lblStatus.Text = $"Час: {minutes:D2}:{seconds:D2}";
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
            }
            else
            {
                SetSettingsAccess(false); 
            }
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

                await _pomodoroClient.StartPomodoroAsync(workTime, shortBreak, longBreak, sessions);
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

                if (_pomodoroClient.Login != null)
                    await _pomodoroClient.SaveSettingsAsync(_pomodoroClient.Login, workTime, shortBreak, longBreak, sessions);
                else
                    throw new Exception("Невірний логін");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }
    }
}