namespace NesPlayer
{
    partial class SoundConfig
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
            this.ChannelLabel = new System.Windows.Forms.Label();
            this.Pulse1Label = new System.Windows.Forms.Label();
            this.Pulse2Label = new System.Windows.Forms.Label();
            this.TriangleLabel = new System.Windows.Forms.Label();
            this.NoiseLabel = new System.Windows.Forms.Label();
            this.DMCLabel = new System.Windows.Forms.Label();
            this.Pulse1CheckBox = new System.Windows.Forms.CheckBox();
            this.Pulse2CheckBox = new System.Windows.Forms.CheckBox();
            this.TriangleCheckBox = new System.Windows.Forms.CheckBox();
            this.NoiseCheckBox = new System.Windows.Forms.CheckBox();
            this.DMCCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ChannelLabel
            // 
            this.ChannelLabel.AutoSize = true;
            this.ChannelLabel.Location = new System.Drawing.Point(42, 15);
            this.ChannelLabel.Name = "ChannelLabel";
            this.ChannelLabel.Size = new System.Drawing.Size(51, 13);
            this.ChannelLabel.TabIndex = 0;
            this.ChannelLabel.Text = "Channels";
            // 
            // Pulse1Label
            // 
            this.Pulse1Label.AutoSize = true;
            this.Pulse1Label.Location = new System.Drawing.Point(14, 42);
            this.Pulse1Label.Name = "Pulse1Label";
            this.Pulse1Label.Size = new System.Drawing.Size(42, 13);
            this.Pulse1Label.TabIndex = 1;
            this.Pulse1Label.Text = "Pulse 1";
            // 
            // Pulse2Label
            // 
            this.Pulse2Label.AutoSize = true;
            this.Pulse2Label.Location = new System.Drawing.Point(14, 65);
            this.Pulse2Label.Name = "Pulse2Label";
            this.Pulse2Label.Size = new System.Drawing.Size(42, 13);
            this.Pulse2Label.TabIndex = 2;
            this.Pulse2Label.Text = "Pulse 2";
            // 
            // TriangleLabel
            // 
            this.TriangleLabel.AutoSize = true;
            this.TriangleLabel.Location = new System.Drawing.Point(14, 88);
            this.TriangleLabel.Name = "TriangleLabel";
            this.TriangleLabel.Size = new System.Drawing.Size(45, 13);
            this.TriangleLabel.TabIndex = 3;
            this.TriangleLabel.Text = "Triangle";
            // 
            // NoiseLabel
            // 
            this.NoiseLabel.AutoSize = true;
            this.NoiseLabel.Location = new System.Drawing.Point(14, 112);
            this.NoiseLabel.Name = "NoiseLabel";
            this.NoiseLabel.Size = new System.Drawing.Size(34, 13);
            this.NoiseLabel.TabIndex = 4;
            this.NoiseLabel.Text = "Noise";
            // 
            // DMCLabel
            // 
            this.DMCLabel.AutoSize = true;
            this.DMCLabel.Location = new System.Drawing.Point(14, 138);
            this.DMCLabel.Name = "DMCLabel";
            this.DMCLabel.Size = new System.Drawing.Size(31, 13);
            this.DMCLabel.TabIndex = 5;
            this.DMCLabel.Text = "DMC";
            // 
            // Pulse1CheckBox
            // 
            this.Pulse1CheckBox.AutoSize = true;
            this.Pulse1CheckBox.Location = new System.Drawing.Point(75, 42);
            this.Pulse1CheckBox.Name = "Pulse1CheckBox";
            this.Pulse1CheckBox.Size = new System.Drawing.Size(59, 17);
            this.Pulse1CheckBox.TabIndex = 6;
            this.Pulse1CheckBox.Text = "Enable";
            this.Pulse1CheckBox.UseVisualStyleBackColor = true;
            this.Pulse1CheckBox.CheckedChanged += new System.EventHandler(this.Pulse1CheckBox_CheckedChanged);
            // 
            // Pulse2CheckBox
            // 
            this.Pulse2CheckBox.AutoSize = true;
            this.Pulse2CheckBox.Location = new System.Drawing.Point(75, 65);
            this.Pulse2CheckBox.Name = "Pulse2CheckBox";
            this.Pulse2CheckBox.Size = new System.Drawing.Size(59, 17);
            this.Pulse2CheckBox.TabIndex = 7;
            this.Pulse2CheckBox.Text = "Enable";
            this.Pulse2CheckBox.UseVisualStyleBackColor = true;
            this.Pulse2CheckBox.CheckedChanged += new System.EventHandler(this.Pulse2CheckBox_CheckedChanged);
            // 
            // TriangleCheckBox
            // 
            this.TriangleCheckBox.AutoSize = true;
            this.TriangleCheckBox.Location = new System.Drawing.Point(75, 88);
            this.TriangleCheckBox.Name = "TriangleCheckBox";
            this.TriangleCheckBox.Size = new System.Drawing.Size(59, 17);
            this.TriangleCheckBox.TabIndex = 8;
            this.TriangleCheckBox.Text = "Enable";
            this.TriangleCheckBox.UseVisualStyleBackColor = true;
            this.TriangleCheckBox.CheckedChanged += new System.EventHandler(this.TriangleCheckBox_CheckedChanged);
            // 
            // NoiseCheckBox
            // 
            this.NoiseCheckBox.AutoSize = true;
            this.NoiseCheckBox.Location = new System.Drawing.Point(75, 111);
            this.NoiseCheckBox.Name = "NoiseCheckBox";
            this.NoiseCheckBox.Size = new System.Drawing.Size(59, 17);
            this.NoiseCheckBox.TabIndex = 9;
            this.NoiseCheckBox.Text = "Enable";
            this.NoiseCheckBox.UseVisualStyleBackColor = true;
            this.NoiseCheckBox.CheckedChanged += new System.EventHandler(this.NoiseCheckBox_CheckedChanged);
            // 
            // DMCCheckBox
            // 
            this.DMCCheckBox.AutoSize = true;
            this.DMCCheckBox.Location = new System.Drawing.Point(75, 134);
            this.DMCCheckBox.Name = "DMCCheckBox";
            this.DMCCheckBox.Size = new System.Drawing.Size(59, 17);
            this.DMCCheckBox.TabIndex = 10;
            this.DMCCheckBox.Text = "Enable";
            this.DMCCheckBox.UseVisualStyleBackColor = true;
            this.DMCCheckBox.CheckedChanged += new System.EventHandler(this.DMCCheckBox_CheckedChanged);
            // 
            // SoundConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(147, 162);
            this.Controls.Add(this.DMCCheckBox);
            this.Controls.Add(this.NoiseCheckBox);
            this.Controls.Add(this.TriangleCheckBox);
            this.Controls.Add(this.Pulse2CheckBox);
            this.Controls.Add(this.Pulse1CheckBox);
            this.Controls.Add(this.DMCLabel);
            this.Controls.Add(this.NoiseLabel);
            this.Controls.Add(this.TriangleLabel);
            this.Controls.Add(this.Pulse2Label);
            this.Controls.Add(this.Pulse1Label);
            this.Controls.Add(this.ChannelLabel);
            this.Name = "SoundConfig";
            this.Text = "SoundConfig";
            this.Load += new System.EventHandler(this.SoundConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ChannelLabel;
        private System.Windows.Forms.Label Pulse1Label;
        private System.Windows.Forms.Label Pulse2Label;
        private System.Windows.Forms.Label TriangleLabel;
        private System.Windows.Forms.Label NoiseLabel;
        private System.Windows.Forms.Label DMCLabel;
        private System.Windows.Forms.CheckBox Pulse1CheckBox;
        private System.Windows.Forms.CheckBox Pulse2CheckBox;
        private System.Windows.Forms.CheckBox TriangleCheckBox;
        private System.Windows.Forms.CheckBox NoiseCheckBox;
        private System.Windows.Forms.CheckBox DMCCheckBox;
    }
}