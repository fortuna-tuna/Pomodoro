namespace Client
{
    partial class Form1
    {
        
        private System.ComponentModel.IContainer components = null;

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            btnStart = new Button();
            btnStop = new Button();
            lblTimer = new Label();
            numWorkDuration = new NumericUpDown();
            numShortBreak = new NumericUpDown();
            btnSaveSettings = new Button();
            lblStatus = new Label();
            numPomodoroCount = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)numWorkDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numShortBreak).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPomodoroCount).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(86, 140);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "Старт";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(358, 140);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 1;
            btnStop.Text = "Стоп";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Font = new Font("Segoe UI", 26F);
            lblTimer.Location = new Point(349, 9);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(131, 60);
            lblTimer.TabIndex = 3;
            lblTimer.Text = "00:00";
            // 
            // numWorkDuration
            // 
            numWorkDuration.Location = new Point(55, 242);
            numWorkDuration.Name = "numWorkDuration";
            numWorkDuration.Size = new Size(150, 27);
            numWorkDuration.TabIndex = 4;
            // 
            // numShortBreak
            // 
            numShortBreak.Location = new Point(302, 242);
            numShortBreak.Name = "numShortBreak";
            numShortBreak.Size = new Size(150, 27);
            numShortBreak.TabIndex = 5;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(525, 140);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(200, 29);
            btnSaveSettings.TabIndex = 6;
            btnSaveSettings.Text = "Зберегти налаштування";
            btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(287, 78);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(232, 20);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Статус: Очікування підключення";
            // 
            // numPomodoroCount
            // 
            numPomodoroCount.Location = new Point(554, 242);
            numPomodoroCount.Name = "numPomodoroCount";
            numPomodoroCount.Size = new Size(150, 27);
            numPomodoroCount.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(55, 198);
            label1.Name = "label1";
            label1.Size = new Size(87, 20);
            label1.TabIndex = 9;
            label1.Text = "Робота (хв)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(311, 198);
            label2.Name = "label2";
            label2.Size = new Size(138, 20);
            label2.TabIndex = 10;
            label2.Text = "Коротка пауза (хв)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(561, 197);
            label3.Name = "label3";
            label3.Size = new Size(152, 20);
            label3.TabIndex = 11;
            label3.Text = "Кількість Помодорів";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numPomodoroCount);
            Controls.Add(lblStatus);
            Controls.Add(btnSaveSettings);
            Controls.Add(numShortBreak);
            Controls.Add(numWorkDuration);
            Controls.Add(lblTimer);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Name = "Form1";
            Text = "Client";
            ((System.ComponentModel.ISupportInitialize)numWorkDuration).EndInit();
            ((System.ComponentModel.ISupportInitialize)numShortBreak).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPomodoroCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private Button btnStop;
        private Label lblTimer;
        private NumericUpDown numWorkDuration;
        private NumericUpDown numShortBreak;
        private Button btnSaveSettings;
        private Label lblStatus;
        private NumericUpDown numPomodoroCount;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
