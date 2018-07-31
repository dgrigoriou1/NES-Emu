namespace NesPlayer
{
    partial class NES
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NES));
            this.NESMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fULLSCREENToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.OutputPictureBox = new System.Windows.Forms.PictureBox();
            this.RunNESBackGroundWorker = new System.ComponentModel.BackgroundWorker();
            this.FramePerSecondTimer = new System.Windows.Forms.Timer(this.components);
            this.NESMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NESMenuStrip
            // 
            this.NESMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.videoToolStripMenuItem,
            this.inputToolStripMenuItem,
            this.soundToolStripMenuItem,
            this.DebugToolStripMenuItem});
            this.NESMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.NESMenuStrip.Name = "NESMenuStrip";
            this.NESMenuStrip.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.NESMenuStrip.Size = new System.Drawing.Size(256, 24);
            this.NESMenuStrip.TabIndex = 0;
            this.NESMenuStrip.Text = "NESMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openToolStripMenuItem1,
            this.resetToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 4, 0);
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Game";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openToolStripMenuItem.Text = "&Open & Play";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.openToolStripMenuItem1.Text = "Open";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Enabled = false;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click_1);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click_1);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // videoToolStripMenuItem
            // 
            this.videoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.xToolStripMenuItem1,
            this.fULLSCREENToolStripMenuItem});
            this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
            this.videoToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.videoToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.videoToolStripMenuItem.Text = "Video";
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Checked = true;
            this.xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.xToolStripMenuItem.Text = "1X";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.StandardSizeToolStripMenuItem_Click);
            // 
            // xToolStripMenuItem1
            // 
            this.xToolStripMenuItem1.Name = "xToolStripMenuItem1";
            this.xToolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
            this.xToolStripMenuItem1.Text = "2X";
            this.xToolStripMenuItem1.Click += new System.EventHandler(this.DoubleSizeToolStripMenuItem1_Click);
            // 
            // fULLSCREENToolStripMenuItem
            // 
            this.fULLSCREENToolStripMenuItem.Name = "fULLSCREENToolStripMenuItem";
            this.fULLSCREENToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.fULLSCREENToolStripMenuItem.Text = "FULL SCREEN";
            this.fULLSCREENToolStripMenuItem.Click += new System.EventHandler(this.FullScreenToolStripMenuItem_Click);
            // 
            // inputToolStripMenuItem
            // 
            this.inputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem});
            this.inputToolStripMenuItem.Name = "inputToolStripMenuItem";
            this.inputToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.inputToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.inputToolStripMenuItem.Text = "Input";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.InputConfigurationToolStripMenuItem_Click);
            // 
            // soundToolStripMenuItem
            // 
            this.soundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableToolStripMenuItem,
            this.channelsToolStripMenuItem});
            this.soundToolStripMenuItem.Name = "soundToolStripMenuItem";
            this.soundToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.soundToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.soundToolStripMenuItem.Text = "Sound";
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Checked = true;
            this.enableToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.EnableSoundToolStripMenuItem_Click);
            // 
            // channelsToolStripMenuItem
            // 
            this.channelsToolStripMenuItem.Name = "channelsToolStripMenuItem";
            this.channelsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.channelsToolStripMenuItem.Text = "Channels";
            this.channelsToolStripMenuItem.Click += new System.EventHandler(this.SoundChannelsToolStripMenuItem_Click);
            // 
            // DebugToolStripMenuItem
            // 
            this.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem";
            this.DebugToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DebugToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.DebugToolStripMenuItem.Text = "Debugger";
            this.DebugToolStripMenuItem.Click += new System.EventHandler(this.DebuggerToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // OutputPictureBox
            // 
            this.OutputPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputPictureBox.Location = new System.Drawing.Point(0, 24);
            this.OutputPictureBox.Name = "OutputPictureBox";
            this.OutputPictureBox.Size = new System.Drawing.Size(256, 240);
            this.OutputPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OutputPictureBox.TabIndex = 1;
            this.OutputPictureBox.TabStop = false;
            // 
            // RunNESBackGroundWorker
            // 
            this.RunNESBackGroundWorker.WorkerReportsProgress = true;
            this.RunNESBackGroundWorker.WorkerSupportsCancellation = true;
            this.RunNESBackGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RunNESBackGroundWorker1_DoWork);
            this.RunNESBackGroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.RunNESBackGroundWorker_ProgressChanged);
            this.RunNESBackGroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RunNESBackGroundWorker1_RunWorkerCompleted);
            // 
            // FramePerSecondTimer
            // 
            this.FramePerSecondTimer.Enabled = true;
            this.FramePerSecondTimer.Interval = 1000;
            this.FramePerSecondTimer.Tick += new System.EventHandler(this.FramesPerSecondTimer_Tick);
            // 
            // NES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 264);
            this.Controls.Add(this.OutputPictureBox);
            this.Controls.Add(this.NESMenuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.NESMenuStrip;
            this.Name = "NES";
            this.Text = "NES";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NES_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NES_KeyUp);
            this.NESMenuStrip.ResumeLayout(false);
            this.NESMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip NESMenuStrip;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PictureBox OutputPictureBox;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fULLSCREENToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelsToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker RunNESBackGroundWorker;
        public System.Windows.Forms.Timer FramePerSecondTimer;
    }
}

