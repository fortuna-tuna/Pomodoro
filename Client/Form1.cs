using System;
using System.Text.Json; 
using System.Windows.Forms;
using Pomodoro.Common;

namespace Client
{
    public partial class Form1 : Form
    {
        private PomodoroClient _client;

        public Form1()
        {
            InitializeComponent();

            
            _client = new PomodoroClient("127.0.0.1", 5000);

           
            lblTimer.Text = "0:00";
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                
                var settingsDto = new PomodoroSettingsDto
                {
                    WorkDuration = (int)numWorkDuration.Value,
                    ShortBreakDuration = (int)numShortBreak.Value,
                    PomodoroCount = (int)numPomodoroCount.Value
                };

                
                string payloadJson = JsonSerializer.Serialize(settingsDto);

               
                var message = new NetworkMessage
                {
                    Type = MessageType.StartPomodoro,
                    Payload = payloadJson
                };

                
                await _client.SendNetworkMessageAsync(message);

                lblStatus.Text = "Статус: Надіслано запит на старт";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при запуску: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var message = new NetworkMessage
                {
                    Type = MessageType.StopPomodoro,
                    Payload = string.Empty
                };

                await _client.SendNetworkMessageAsync(message);

                lblStatus.Text = "Статус: Таймер зупинено";
                lblTimer.Text = "0:00"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зупинці: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var settingsDto = new PomodoroSettingsDto
                {
                    WorkDuration = (int)numWorkDuration.Value,
                    ShortBreakDuration = (int)numShortBreak.Value,
                    PomodoroCount = (int)numPomodoroCount.Value
                };

                string payloadJson = JsonSerializer.Serialize(settingsDto);

                var message = new NetworkMessage
                {
                    Type = MessageType.SaveSettings, 
                    Payload = payloadJson
                };

                await _client.SendNetworkMessageAsync(message);

                lblStatus.Text = "Статус: Налаштування збережено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

    }


}