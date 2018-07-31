using System;
using System.Windows.Forms;

namespace NesPlayer
{
    public partial class SoundConfig : Form
    {
        public SoundConfig()
        {
            InitializeComponent();
        }
    
        private void SoundConfig_Load(object sender, EventArgs e)
        {
            Pulse1CheckBox.Checked = !APU.DisablePulse1FromMenu;
            Pulse2CheckBox.Checked = !APU.DisablePulse2FromMenu;
            TriangleCheckBox.Checked = !APU.DisableTriangleFromMenu;
            NoiseCheckBox.Checked = !APU.DisableNoiseFromMenu;
            DMCCheckBox.Checked = !APU.DisableDMC;
        }

        private void Pulse1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Pulse1CheckBox.Checked)
            {
                APU.DisablePulse1FromMenu = false;
            }
            else
            {
                APU.DisablePulse1FromMenu = true;
            }
        }

        private void Pulse2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Pulse2CheckBox.Checked)
            {
                APU.DisablePulse2FromMenu = false;
            }
            else
            {
                APU.DisablePulse2FromMenu = true;
            }
        }

        private void TriangleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TriangleCheckBox.Checked)
            {
                APU.DisableTriangleFromMenu = false;
            }
            else
            {
                APU.DisableTriangleFromMenu = true;
            }
        }

        private void NoiseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NoiseCheckBox.Checked)
            {
                APU.DisableNoiseFromMenu = false;
            }
            else
            {
                APU.DisableNoiseFromMenu = true;
            }
        }

        private void DMCCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DMCCheckBox.Checked)
            {
                APU.DisableDMC = false;
            }
            else
            {
                APU.DisableDMC = true;
            }
        }

      
    }
}
