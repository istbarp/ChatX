namespace Server
{
    partial class ServerGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtIO = new System.Windows.Forms.TextBox();
            this.txtServiceIP = new System.Windows.Forms.TextBox();
            this.lblhost = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(442, 459);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 31);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(316, 459);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 31);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop Server";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtIO
            // 
            this.txtIO.Location = new System.Drawing.Point(12, 12);
            this.txtIO.Multiline = true;
            this.txtIO.Name = "txtIO";
            this.txtIO.ReadOnly = true;
            this.txtIO.Size = new System.Drawing.Size(550, 369);
            this.txtIO.TabIndex = 2;
            // 
            // txtServiceIP
            // 
            this.txtServiceIP.Location = new System.Drawing.Point(12, 387);
            this.txtServiceIP.Name = "txtServiceIP";
            this.txtServiceIP.Size = new System.Drawing.Size(240, 22);
            this.txtServiceIP.TabIndex = 3;
            // 
            // lblhost
            // 
            this.lblhost.AutoSize = true;
            this.lblhost.Location = new System.Drawing.Point(258, 390);
            this.lblhost.Name = "lblhost";
            this.lblhost.Size = new System.Drawing.Size(78, 17);
            this.lblhost.TabIndex = 4;
            this.lblhost.Text = "Host Name";
            // 
            // ServerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 502);
            this.Controls.Add(this.lblhost);
            this.Controls.Add(this.txtServiceIP);
            this.Controls.Add(this.txtIO);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "ServerGUI";
            this.Text = "ServerGUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtIO;
        private System.Windows.Forms.TextBox txtServiceIP;
        private System.Windows.Forms.Label lblhost;
    }
}