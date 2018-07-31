using System;
using System.Windows.Forms;

namespace NesPlayer
{
    public partial class BreakPointPopUp : Form
    {
        public BreakPointPopUp()
        {
            InitializeComponent();
        }
        public static bool IsEdit = false;
        public static string BreakPointToEdit;

        private void BreakPointPopUp_Load(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                string BreakPointType = BreakPointToEdit.Split(':')[0].Trim();
                string[] BreakPointData = BreakPointToEdit.Split(' ');
                if (BreakPointType == "Write")
                {
                    CPUMemoryRadioButton.Checked = true;
                    LowMemoryTextBox.Text = BreakPointData[1].Split('-')[0];
                    if(BreakPointData[1].Split('-')[0]!= BreakPointData[1].Split('-')[1])
                    {
                        HighMemoryTextBox.Text = BreakPointData[1].Split('-')[1];
                    }
                }
                else if (BreakPointType == "Read")
                {
                    CPUMemoryRadioButton.Checked = true;
                    LowMemoryTextBox.Text = BreakPointData[1].Split('-')[0];
                    if (BreakPointData[1].Split('-')[0] != BreakPointData[1].Split('-')[1])
                    {
                        HighMemoryTextBox.Text = BreakPointData[1].Split('-')[1];
                    }
                }
                else if (BreakPointType == "OpCode")
                {
                    OpCodeRadioButton.Checked = true;
                    OpCodeTextBox.Text = BreakPointData[1].Trim().ToUpper();
                }

                if (BreakPointData[BreakPointData.Length - 1].Trim() == "(+)")
                {
                    EnabledCheckBox.Checked = true;
                }
                else
                {
                    EnabledCheckBox.Checked = false;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ListBox DebuggerBreakPointListBox = (ListBox)DebuggerForm.Dbug.Controls.Find("BreakPointsListBox", true)[0];
            if (IsEdit)
            {
                DebuggerBreakPointListBox.Items.Remove(BreakPointToEdit);
            }
            string IsEnabledOutput = "(+)";
            if (!EnabledCheckBox.Checked)
            {
                IsEnabledOutput = "(-)";
            }
            if (OpCodeRadioButton.Checked)
            {
                if (OpCodeTextBox.Text.ToString().ToUpper().Trim().Length > 0)
                {                   
                    DebuggerBreakPointListBox.Items.Add("OpCode: "+ OpCodeTextBox.Text.ToString().ToUpper().Trim() + " "+IsEnabledOutput);
                }
            }
            else
            {
                string BreakPointAccessType = "Read: ";
                if (!ReadRadioButton.Checked)
                {
                    BreakPointAccessType = "Write: ";
                }                
                string MemoryLow = LowMemoryTextBox.Text.ToString().ToUpper().Trim().PadLeft(4,'0');                
                string MemoryHigh = HighMemoryTextBox.Text.ToString().ToUpper().Trim().PadLeft(4, '0');
                if (MemoryHigh.Length > 0)
                {
                    DebuggerBreakPointListBox.Items.Add(BreakPointAccessType + MemoryLow + "-" + MemoryHigh + " " + IsEnabledOutput);
                }
                else if (MemoryLow.Length > 0)
                {
                    DebuggerBreakPointListBox.Items.Add(BreakPointAccessType + MemoryLow + "-" + MemoryLow + " " + IsEnabledOutput);
                }
            }

            for (int i = 0; i < DebuggerBreakPointListBox.Items.Count; i++)
            {
                string BreakPointListBoxItem = DebuggerBreakPointListBox.Items[i].ToString().ToUpper().Trim();
                string BreakPointType = BreakPointListBoxItem.Split(':')[0].Trim();
                string[] ExistingBreakPoint = BreakPointListBoxItem.Split(' ');
                string[] NewBreakPoint = new string[5];
                NewBreakPoint[0] = BreakPointType;
                NewBreakPoint[1] = ExistingBreakPoint[ExistingBreakPoint.Length - 1].Trim();
                if (BreakPointType == "WRITE")
                {
                    NewBreakPoint[2] = ExistingBreakPoint[1].Split('-')[0];
                    if (ExistingBreakPoint[1].Split('-')[0] != ExistingBreakPoint[1].Split('-')[1])
                    {
                        NewBreakPoint[3] = ExistingBreakPoint[1].Split('-')[1];
                    }
                }
                else if (BreakPointType == "READ")
                {
                    NewBreakPoint[2] = ExistingBreakPoint[1].Split('-')[0];
                    if (ExistingBreakPoint[1].Split('-')[0] != ExistingBreakPoint[1].Split('-')[1])
                    {
                        NewBreakPoint[3] = ExistingBreakPoint[1].Split('-')[1];
                    }
                }
                else if (BreakPointType == "OPCODE")
                {
                    NewBreakPoint[4] = ExistingBreakPoint[1].Trim();
                }
                Debugger.BreakPointList.Add(NewBreakPoint);
            }
            this.Close();
        }
        

        private void OpCodeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (OpCodeRadioButton.Checked)
            {
                OpCodeTextBox.Enabled = true;
            }
            else
            {
                OpCodeTextBox.Enabled = false;
            }
        }

        private void CPUMemoryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CPUMemoryRadioButton.Checked)
            {
                CPUMemoryGroupBox.Enabled = true;
            }
            else
            {
                CPUMemoryGroupBox.Enabled = false;
            }
        }
    }
}
