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
            ((System.ComponentModel.ISupportInitialize)numWorkDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numShortBreak).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(46, 45);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "Старт";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(230, 45);
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
            lblTimer.Location = new Point(46, 159);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(44, 20);
            lblTimer.TabIndex = 3;
            lblTimer.Text = "25:00";
            // 
            // numWorkDuration
            // 
            numWorkDuration.Location = new Point(54, 205);
            numWorkDuration.Name = "numWorkDuration";
            numWorkDuration.Size = new Size(150, 27);
            numWorkDuration.TabIndex = 4;
            // 
            // numShortBreak
            // 
            numShortBreak.Location = new Point(287, 205);
            numShortBreak.Name = "numShortBreak";
            numShortBreak.Size = new Size(150, 27);
            numShortBreak.TabIndex = 5;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(387, 47);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(200, 29);
            btnSaveSettings.TabIndex = 6;
            btnSaveSettings.Text = "Зберегти налаштування";
            btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(46, 108);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(232, 20);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Статус: Очікування підключення";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}
