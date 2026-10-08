using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private PomodoroClient _pomodoroClient;

        public Form1()
        {
            InitializeComponent();
            _pomodoroClient = new PomodoroClient();
            _pomodoroClient.OnMessageReceived += HandleMessageReceived;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                
                await _pomodoroClient.ConnectAsync("127.0.0.1", 5000);
                if (lblStatus != null)
                    lblStatus.Text = "Статус: Підключено до сервера";
            }
            catch (Exception ex)
            {
                if (lblStatus != null)
                    lblStatus.Text = $"Статус: Помилка підключення ({ex.Message})";
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Таймер Pomodoro запущено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _pomodoroClient?.Disconnect();
            if (lblStatus != null)
                lblStatus.Text = "Статус: Зупинено / Відключено";
        }

        private async void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                int workTime = (int)numWorkDuration.Value;
                int breakTime = (int)numShortBreak.Value;

                await _pomodoroClient.SendSettingsAsync(workTime, breakTime);
                MessageBox.Show("Налаштування успішно відправлені на сервер!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відправки: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleMessageReceived(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(HandleMessageReceived), message);
                return;
            }

            if (lblStatus != null)
            {
                lblStatus.Text = message;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pomodoroClient?.Disconnect();
        }
    }
}