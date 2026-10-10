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
            label1 = new Label();
            label2 = new Label();
            numLongBreak = new NumericUpDown();
            numSessions = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)numWorkDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numShortBreak).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLongBreak).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSessions).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(78, 129);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "Старт";
            btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            btnStop.Location = new Point(330, 129);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 1;
            btnStop.Text = "Стоп";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Font = new Font("Segoe UI", 28F);
            lblTimer.Location = new Point(330, 9);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(137, 62);
            lblTimer.TabIndex = 3;
            lblTimer.Text = "00:00";
            // 
            // numWorkDuration
            // 
            numWorkDuration.Location = new Point(91, 242);
            numWorkDuration.Name = "numWorkDuration";
            numWorkDuration.Size = new Size(150, 27);
            numWorkDuration.TabIndex = 4;
            // 
            // numShortBreak
            // 
            numShortBreak.Location = new Point(495, 242);
            numShortBreak.Name = "numShortBreak";
            numShortBreak.Size = new Size(150, 27);
            numShortBreak.TabIndex = 5;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(536, 129);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(200, 29);
            btnSaveSettings.TabIndex = 6;
            btnSaveSettings.Text = "Зберегти налаштування";
            btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(282, 71);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(232, 20);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Статус: Очікування підключення";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(102, 284);
            label1.Name = "label1";
            label1.Size = new Size(87, 20);
            label1.TabIndex = 8;
            label1.Text = "Робота (хв)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(495, 284);
            label2.Name = "label2";
            label2.Size = new Size(138, 20);
            label2.TabIndex = 9;
            label2.Text = "Коротка пауза (хв)";
            // 
            // numLongBreak
            // 
            numLongBreak.Location = new Point(95, 329);
            numLongBreak.Name = "numLongBreak";
            numLongBreak.Size = new Size(150, 27);
            numLongBreak.TabIndex = 10;
            // 
            // numSessions
            // 
            numSessions.Location = new Point(495, 329);
            numSessions.Name = "numSessions";
            numSessions.Size = new Size(150, 27);
            numSessions.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(96, 376);
            label3.Name = "label3";
            label3.Size = new Size(122, 20);
            label3.TabIndex = 12;
            label3.Text = "Довга пауза (хв)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(492, 371);
            label4.Name = "label4";
            label4.Size = new Size(159, 20);
            label4.TabIndex = 13;
            label4.Text = "Сесій до довгої паузи";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(numSessions);
            Controls.Add(numLongBreak);
            Controls.Add(label2);
            Controls.Add(label1);
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
            ((System.ComponentModel.ISupportInitialize)numLongBreak).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSessions).EndInit();
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
        private Label label1;
        private Label label2;
        private NumericUpDown numLongBreak;
        private NumericUpDown numSessions;
        private Label label3;
        private Label label4;
    }
}
