namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            lblTimer = new Label();
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
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(46, 113);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(138, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Статус: Очікування";
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTimer);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Name = "Form1";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label lblTimer;
    }
}
