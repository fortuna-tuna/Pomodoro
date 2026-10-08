using System;
using System.Text.Json;
using System.Threading.Tasks;
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
            _pomodoroClient.OnMessageReceived += HandleIncomingMessage;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                
                await _pomodoroClient.ConnectAsync("127.0.0.1", 5000);

                
                _ = Task.Run(() => _pomodoroClient.ListenServerAsync(default));

                lblStatus.Text = "Статус: Підключено до сервера";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Статус: Помилка підключення";
                MessageBox.Show($"Не вдалося підключитись до сервера: {ex.Message}");
            }
        }

        private void HandleIncomingMessage(string jsonResponse)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = doc.RootElement;
                    int messageType = 0;

                    if (root.TryGetProperty("Type", out JsonElement typeElement) ||
                        root.TryGetProperty("type", out typeElement))
                    {
                        messageType = typeElement.GetInt32();
                    }

                    
                    this.Invoke((MethodInvoker)(() =>
                    {
                        
                        lblStatus.Text = $"Тип від сервера: {messageType}";

                        
                        if (root.TryGetProperty("Payload", out JsonElement payloadElement))
                        {
                            string payloadStr = payloadElement.ValueKind == JsonValueKind.String
                                ? payloadElement.GetString()
                                : payloadElement.GetRawText();

                            if (!string.IsNullOrEmpty(payloadStr))
                            {
                                lblTimer.Text = payloadStr;
                            }
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка обробки: {ex.Message}");
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                await _pomodoroClient.SendMessageAsync(1, new { Action = "Start" });
                lblStatus.Text = "Статус: Помодоро запущено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відправки: {ex.Message}");
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                await _pomodoroClient.SendMessageAsync(2, new { Action = "Stop" });
                lblStatus.Text = "Статус: Зупинено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відправки: {ex.Message}");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}