using System;
using System.Windows.Forms;
using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;
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

            
            _ = ConnectToServerAsync();

           
            _pomodoroClient.OnMessageReceived += (message) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => HandleIncomingMessage(message)));
                }
                else
                {
                    HandleIncomingMessage(message);
                }
            };
        }

                var message = new NetworkMessage
                {
                    Type = MessageType.StartPomodoro,
                    Payload = payloadJson
                };

                await _client.SendNetworkMessageAsync(message);
                lblStatus.Text = "Статус: Надіслано запит на старт";

                await _pomodoroClient.ConnectAsync("127.0.0.1", 5000);
                if (lblStatus != null)
                    lblStatus.Text = "Статус: Підключено до сервера";
            }
            catch (Exception)
            {
                lblStatus.Text = "Статус: Очікування підключення";
            }
        }

        private void HandleIncomingMessage(string message)
        {
            
            lblStatus.Text = message;
        }

        
        private async void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                int workTime = (int)numWorkDuration.Value;
                int breakTime = (int)numShortBreak.Value;

                await _pomodoroClient.SendSettingsAsync(workTime, breakTime);
                MessageBox.Show("Налаштування успішно надіслано на сервер!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відправки: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                int workTime = (int)numWorkDuration.Value;
                int breakTime = (int)numShortBreak.Value;

               
                await _pomodoroClient.SendSettingsAsync(workTime, breakTime);
                lblStatus.Text = "Статус: Таймер запущено";
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
                lblStatus.Text = "Статус: Зупинено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зупинки: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            _pomodoroClient?.Disconnect();
        }

        private void btnSaveSettings_Click_1(object sender, EventArgs e)
        {

        }
    }
}