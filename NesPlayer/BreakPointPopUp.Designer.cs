namespace NesPlayer
{
    partial class BreakPointPopUp
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
            this.CPUMemoryGroupBox = new System.Windows.Forms.GroupBox();
            this.ToLabel = new System.Windows.Forms.Label();
            this.HighMemoryTextBox = new System.Windows.Forms.TextBox();
            this.WriteRadioButton = new System.Windows.Forms.RadioButton();
            this.ReadRadioButton = new System.Windows.Forms.RadioButton();
            this.LowMemoryTextBox = new System.Windows.Forms.TextBox();
            this.OpCodeLabel = new System.Windows.Forms.Label();
            this.OpCodeTextBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.OpCodeRadioButton = new System.Windows.Forms.RadioButton();
            this.CPUMemoryRadioButton = new System.Windows.Forms.RadioButton();
            this.CloseButton = new System.Windows.Forms.Button();
            this.EnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.CPUMemoryGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // CPUMemoryGroupBox
            // 
            this.CPUMemoryGroupBox.Controls.Add(this.ToLabel);
            this.CPUMemoryGroupBox.Controls.Add(this.HighMemoryTextBox);
            this.CPUMemoryGroupBox.Controls.Add(this.WriteRadioButton);
            this.CPUMemoryGroupBox.Controls.Add(this.ReadRadioButton);
            this.CPUMemoryGroupBox.Controls.Add(this.LowMemoryTextBox);
            this.CPUMemoryGroupBox.Enabled = false;
            this.CPUMemoryGroupBox.Location = new System.Drawing.Point(37, 58);
            this.CPUMemoryGroupBox.Name = "CPUMemoryGroupBox";
            this.CPUMemoryGroupBox.Size = new System.Drawing.Size(150, 69);
            this.CPUMemoryGroupBox.TabIndex = 59;
            this.CPUMemoryGroupBox.TabStop = false;
            this.CPUMemoryGroupBox.Text = "CPU Memory";
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(58, 39);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(20, 13);
            this.ToLabel.TabIndex = 45;
            this.ToLabel.Text = "To";
            // 
            // HighMemoryTextBox
            // 
            this.HighMemoryTextBox.Location = new System.Drawing.Point(87, 36);
            this.HighMemoryTextBox.Name = "HighMemoryTextBox";
            this.HighMemoryTextBox.Size = new System.Drawing.Size(45, 20);
            this.HighMemoryTextBox.TabIndex = 49;
            // 
            // WriteRadioButton
            // 
            this.WriteRadioButton.AutoSize = true;
            this.WriteRadioButton.Location = new System.Drawing.Point(67, 13);
            this.WriteRadioButton.Name = "WriteRadioButton";
            this.WriteRadioButton.Size = new System.Drawing.Size(50, 17);
            this.WriteRadioButton.TabIndex = 47;
            this.WriteRadioButton.Text = "Write";
            this.WriteRadioButton.UseVisualStyleBackColor = true;
            // 
            // ReadRadioButton
            // 
            this.ReadRadioButton.AutoSize = true;
            this.ReadRadioButton.Checked = true;
            this.ReadRadioButton.Location = new System.Drawing.Point(10, 13);
            this.ReadRadioButton.Name = "ReadRadioButton";
            this.ReadRadioButton.Size = new System.Drawing.Size(51, 17);
            this.ReadRadioButton.TabIndex = 46;
            this.ReadRadioButton.TabStop = true;
            this.ReadRadioButton.Text = "Read";
            this.ReadRadioButton.UseVisualStyleBackColor = true;
            // 
            // LowMemoryTextBox
            // 
            this.LowMemoryTextBox.Location = new System.Drawing.Point(10, 36);
            this.LowMemoryTextBox.Name = "LowMemoryTextBox";
            this.LowMemoryTextBox.Size = new System.Drawing.Size(45, 20);
            this.LowMemoryTextBox.TabIndex = 37;
            // 
            // OpCodeLabel
            // 
            this.OpCodeLabel.AutoSize = true;
            this.OpCodeLabel.Location = new System.Drawing.Point(95, 34);
            this.OpCodeLabel.Name = "OpCodeLabel";
            this.OpCodeLabel.Size = new System.Drawing.Size(49, 13);
            this.OpCodeLabel.TabIndex = 58;
            this.OpCodeLabel.Text = "Op Code";
            // 
            // OpCodeTextBox
            // 
            this.OpCodeTextBox.Location = new System.Drawing.Point(37, 31);
            this.OpCodeTextBox.Name = "OpCodeTextBox";
            this.OpCodeTextBox.Size = new System.Drawing.Size(55, 20);
            this.OpCodeTextBox.TabIndex = 53;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(37, 133);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(69, 23);
            this.SaveButton.TabIndex = 61;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OpCodeRadioButton
            // 
            this.OpCodeRadioButton.AutoSize = true;
            this.OpCodeRadioButton.Checked = true;
            this.OpCodeRadioButton.Location = new System.Drawing.Point(16, 34);
            this.OpCodeRadioButton.Name = "OpCodeRadioButton";
            this.OpCodeRadioButton.Size = new System.Drawing.Size(14, 13);
            this.OpCodeRadioButton.TabIndex = 63;
            this.OpCodeRadioButton.TabStop = true;
            this.OpCodeRadioButton.UseVisualStyleBackColor = true;
            this.OpCodeRadioButton.CheckedChanged += new System.EventHandler(this.OpCodeRadioButton_CheckedChanged);
            // 
            // CPUMemoryRadioButton
            // 
            this.CPUMemoryRadioButton.AutoSize = true;
            this.CPUMemoryRadioButton.Location = new System.Drawing.Point(16, 58);
            this.CPUMemoryRadioButton.Name = "CPUMemoryRadioButton";
            this.CPUMemoryRadioButton.Size = new System.Drawing.Size(14, 13);
            this.CPUMemoryRadioButton.TabIndex = 64;
            this.CPUMemoryRadioButton.UseVisualStyleBackColor = true;
            this.CPUMemoryRadioButton.CheckedChanged += new System.EventHandler(this.CPUMemoryRadioButton_CheckedChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(119, 133);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(68, 23);
            this.CloseButton.TabIndex = 65;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EnabledCheckBox
            // 
            this.EnabledCheckBox.AutoSize = true;
            this.EnabledCheckBox.Checked = true;
            this.EnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnabledCheckBox.Location = new System.Drawing.Point(16, 10);
            this.EnabledCheckBox.Name = "EnabledCheckBox";
            this.EnabledCheckBox.Size = new System.Drawing.Size(65, 17);
            this.EnabledCheckBox.TabIndex = 66;
            this.EnabledCheckBox.Text = "Enabled";
            this.EnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // BreakPointPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 170);
            this.Controls.Add(this.EnabledCheckBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.CPUMemoryRadioButton);
            this.Controls.Add(this.OpCodeRadioButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CPUMemoryGroupBox);
            this.Controls.Add(this.OpCodeLabel);
            this.Controls.Add(this.OpCodeTextBox);
            this.Name = "BreakPointPopUp";
            this.Text = "BreakPointPopUp";
            this.Load += new System.EventHandler(this.BreakPointPopUp_Load);
            this.CPUMemoryGroupBox.ResumeLayout(false);
            this.CPUMemoryGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox CPUMemoryGroupBox;
        private System.Windows.Forms.Label ToLabel;
        private System.Windows.Forms.TextBox HighMemoryTextBox;
        private System.Windows.Forms.RadioButton WriteRadioButton;
        private System.Windows.Forms.RadioButton ReadRadioButton;
        private System.Windows.Forms.TextBox LowMemoryTextBox;
        private System.Windows.Forms.Label OpCodeLabel;
        private System.Windows.Forms.TextBox OpCodeTextBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.RadioButton OpCodeRadioButton;
        private System.Windows.Forms.RadioButton CPUMemoryRadioButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.CheckBox EnabledCheckBox;
    }
}