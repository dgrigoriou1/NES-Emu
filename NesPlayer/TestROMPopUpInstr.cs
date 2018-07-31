using System;
using System.Windows.Forms;

namespace NesPlayer
{
    public partial class TestROMPopUpInstr : Form
    {
        public static string InstrCnt;
        public static string ROMName;

        public TestROMPopUpInstr()
        {
            InitializeComponent();
            InstructionTextBox.Text = InstrCnt;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string NumberOfInstructions = InstructionTextBox.Text;
            if (NumberOfInstructions.Length > 0)
            {
                var isNumeric = int.TryParse(NumberOfInstructions, out int n);
                if (isNumeric) { 
                  Debugger.TestROMList[ROMName][9] = NumberOfInstructions;
                  DebuggerForm.Dbug.DisplayTestROMList();
                }
            }
            this.Close();
        }
    }
}
