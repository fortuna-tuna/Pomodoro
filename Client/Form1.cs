using System;
using System.Text.Json;
using System.Windows.Forms;
using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;



namespace Client
{
    public partial class Form1 : Form
    {
        private readonly PomodoroClient _client;

        public Form1()
        {
            InitializeComponent();

            
            _client = new PomodoroClient("127.0.0.1", 5000);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                var settingsDto = new PomodoroSettingsDTO
                {
                    PomodoroDuration = (int)numWorkDuration.Value,
                    ShortBreak = (int)numShortBreak.Value,
                    CountPomodoroBeforeLongBreak = (int)numPomodoroCount.Value
                };
                string payloadJson = JsonSerializer.Serialize(settingsDto);

                var message = new NetworkMessage
                {
                    Type = MessageType.StartPomodoro,
                    Payload = payloadJson
                };

                await _client.SendNetworkMessageAsync(message);
                lblStatus.Text = "Статус: Надіслано запит на старт";

                //await _pomodoroClient.ConnectAsync("127.0.0.1", 5000);
                if (lblStatus != null)
                    lblStatus.Text = "Статус: Підключено до сервера";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var settingsDto = new PomodoroSettingsDTO
                {
                    PomodoroDuration = (int)numWorkDuration.Value,
                    ShortBreak = (int)numShortBreak.Value,
                    CountPomodoroBeforeLongBreak = (int)numPomodoroCount.Value
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_pomodoroClient?.Disconnect();
        }

        private void btnSaveSettings_Click_1(object sender, EventArgs e)
        {

        }
    }
}