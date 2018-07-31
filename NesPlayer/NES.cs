using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NesPlayer
{
    public partial class NES : Form
    {
        IntPtr Iptr = IntPtr.Zero;
        public static BitmapData bitmapData = null;
        public static Bitmap NESBMP = new Bitmap(256, 240, PixelFormat.Format32bppPArgb);
        public static int FramesPerSecond = 0;
        public static bool Trace = false;
        public static bool PlayLoop = false;
        public static bool DrawFrame = false;
        public static NES nes;
        public NES()
        {
            nes = this;
            InitializeComponent();
            Size = new System.Drawing.Size(272, 305);
            Inputs.LoadInputs();
          
          
            CPU.CycleArray[0x101] = new int[8] { 1, 6, 6, 21, 22, 98, 77, 78 }; //NMI
            CPU.CycleArray[0x102] = new int[8] { 1, 6, 6, 21, 22, 98, 95, 96 }; //IRQ
            CPU.CycleArray[0x104] = new int[8] { 1, 6, 6, 21, 22, 98, 99, 100 }; //Reset
            CPU.CycleArray[0x103] = new int[259] { 1, 6, 6,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54,
                                                 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 54, 53, 84 }; //oam

            CPU.CycleArray[0x69] = new int[2] { 1, 2 };
            CPU.CycleArray[0x65] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0x75] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0x6D] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0x7D] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0x79] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0x61] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0x71] = new int[6] { 1, 16, 91, 92, 93, 75 };
            CPU.CycleArray[0x29] = new int[2] { 1, 2 };
            CPU.CycleArray[0x25] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0x35] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0x2D] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0x3D] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0x39] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0x21] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0x31] = new int[6] { 1, 16, 91, 92, 93, 75 };
            CPU.CycleArray[0x0A] = new int[2] { 1, 90 };
            CPU.CycleArray[0x06] = new int[5] { 1, 9, 5, 6, 7 };
            CPU.CycleArray[0x16] = new int[6] { 1, 9, 10, 5, 6, 7 };
            CPU.CycleArray[0x0E] = new int[6] { 1, 3, 4, 5, 97, 7 };
            CPU.CycleArray[0x1E] = new int[7] { 1, 3, 12, 83, 5, 97, 7 };
            CPU.CycleArray[0x90] = new int[4] { 1,38, 39, 40 };
            CPU.CycleArray[0xB0] = new int[4] { 1, 41, 39, 40 };
            CPU.CycleArray[0xF0] = new int[4] { 1, 42, 39, 40 };
            CPU.CycleArray[0x24] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0x2C] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0x30] = new int[4] { 1, 43, 39, 40 };
            CPU.CycleArray[0xD0] = new int[4] { 1, 44, 39, 40 };
            CPU.CycleArray[0x10] = new int[4] { 1, 45, 39, 40 };
            CPU.CycleArray[0x00] = new int[7] { 1, 20, 21, 22, 23, 24, 25 }; //break
            CPU.CycleArray[0x50] = new int[4] { 1, 46, 39, 40 };
            CPU.CycleArray[0x70] = new int[4] { 1, 47, 39, 40 };
            CPU.CycleArray[0x18] = new int[2] { 1, 58 };
            CPU.CycleArray[0xD8] = new int[2] { 1, 59 };
            CPU.CycleArray[0x58] = new int[2] { 1, 60 };
            CPU.CycleArray[0xB8] = new int[2] { 1, 61 };
            CPU.CycleArray[0xC9] = new int[2] { 1, 2 };
            CPU.CycleArray[0xC5] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xD5] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0xCD] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xDD] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0xD9] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0xC1] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0xD1] = new int[6] { 1, 16, 91, 92, 93, 75 };
            CPU.CycleArray[0xE0] = new int[2] { 1, 2 };
            CPU.CycleArray[0xE4] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xEC] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xC0] = new int[2] { 1, 2 };
            CPU.CycleArray[0xC4] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xCC] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xC6] = new int[5] { 1, 9, 5, 6, 7 };
            CPU.CycleArray[0xD6] = new int[6] { 1, 9, 10, 5, 6, 7 };
            CPU.CycleArray[0xCE] = new int[6] { 1, 3, 4, 5, 97, 7 };
            CPU.CycleArray[0xDE] = new int[7] { 1, 3, 12, 83, 5, 97, 7 };
            CPU.CycleArray[0xCA] = new int[2] { 1, 62 };
            CPU.CycleArray[0x88] = new int[2] { 1, 63 };
            CPU.CycleArray[0x49] = new int[2] { 1, 2 };
            CPU.CycleArray[0x45] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0x55] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0x4D] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0x5D] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0x59] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0x41] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0x51] = new int[6] { 1, 16, 91, 92, 93, 75 };          
            CPU.CycleArray[0xE6] = new int[5] { 1, 9, 5, 6, 7 };
            CPU.CycleArray[0xF6] = new int[6] { 1, 9, 10, 5, 6, 7 };
            CPU.CycleArray[0xEE] = new int[6] { 1, 3, 4, 5, 97, 7 };
            CPU.CycleArray[0xFE] = new int[7] { 1, 3, 12, 83, 5, 97, 7 };
            CPU.CycleArray[0xE8] = new int[2] { 1, 64 };
            CPU.CycleArray[0xC8] = new int[2] { 1, 65 };
            CPU.CycleArray[0x4C] = new int[3] { 1, 3, 79 };
            CPU.CycleArray[0x6C] = new int[5] { 1, 3, 4, 88, 89 };
            CPU.CycleArray[0x20] = new int[6] { 1, 3, 36, 37, 22, 79 };
            CPU.CycleArray[0xA9] = new int[2] { 1, 2 };
            CPU.CycleArray[0xA5] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xB5] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0xAD] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xBD] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0xB9] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0xA1] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0xB1] = new int[6] { 1, 16, 91, 92, 93, 75 };
            CPU.CycleArray[0xA2] = new int[2] { 1, 2 };
            CPU.CycleArray[0xA6] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xB6] = new int[4] { 1, 9, 11, 75 };
            CPU.CycleArray[0xAE] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xBE] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0xA0] = new int[2] { 1, 2 };
            CPU.CycleArray[0xA4] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xB4] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0xAC] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xBC] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0x4A] = new int[2] { 1, 57 };
            CPU.CycleArray[0x46] = new int[5] { 1, 9, 5, 6, 7 };
            CPU.CycleArray[0x56] = new int[6] { 1, 9, 10, 5, 6, 7 };
            CPU.CycleArray[0x4E] = new int[6] { 1, 3, 4, 5, 97, 7 };
            CPU.CycleArray[0x5E] = new int[7] { 1, 3, 12, 83, 5, 97, 7 };
            CPU.CycleArray[0xEA] = new int[2] { 1, 84 };
            CPU.CycleArray[0x09] = new int[2] { 1, 2 };
            CPU.CycleArray[0x05] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0x15] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0x0D] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0x1D] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0x19] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0x01] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0x11] = new int[6] { 1, 16, 91, 92, 93, 75 };
            CPU.CycleArray[0x48] = new int[3] { 1, 26, 32 };
            CPU.CycleArray[0x08] = new int[3] { 1, 26, 33 };
            CPU.CycleArray[0x68] = new int[4] { 1, 26, 27, 34 };
            CPU.CycleArray[0x28] = new int[4] { 1, 26, 27, 35 };
            CPU.CycleArray[0x2A] = new int[2] { 1, 80 };
            CPU.CycleArray[0x26] = new int[5] { 1, 9, 5, 6, 7 };
            CPU.CycleArray[0x36] = new int[6] { 1, 9, 10, 5, 6, 7 };
            CPU.CycleArray[0x2E] = new int[6] { 1, 3, 4, 5, 97, 7 };
            CPU.CycleArray[0x3E] = new int[7] { 1, 3, 12, 83, 5, 97, 7 };
            CPU.CycleArray[0x6A] = new int[2] { 1, 80 };
            CPU.CycleArray[0x66] = new int[5] { 1, 9, 5, 6, 7 };
            CPU.CycleArray[0x76] = new int[6] { 1, 9, 10, 5, 6, 7 };
            CPU.CycleArray[0x6E] = new int[6] { 1, 3, 4, 5, 97, 7 };
            CPU.CycleArray[0x7E] = new int[7] { 1, 3, 12, 83, 5, 97, 7 };
            CPU.CycleArray[0x40] = new int[6] { 1, 26, 27, 28, 29, 30 };
            CPU.CycleArray[0x60] = new int[6] { 1, 26, 27, 29, 82, 31 };
            CPU.CycleArray[0xE9] = new int[2] { 1, 2 };
            CPU.CycleArray[0xE5] = new int[3] { 1, 9, 75 };
            CPU.CycleArray[0xF5] = new int[4] { 1, 9, 10, 75 };
            CPU.CycleArray[0xED] = new int[4] { 1, 3, 4, 75 };
            CPU.CycleArray[0xFD] = new int[5] { 1, 3, 12, 14, 75 };
            CPU.CycleArray[0xF9] = new int[5] { 1, 3, 13, 15, 75 };
            CPU.CycleArray[0xE1] = new int[6] { 1, 16, 17, 18, 19, 75 };
            CPU.CycleArray[0xF1] = new int[6] { 1, 16, 91, 92, 93, 75 };
            CPU.CycleArray[0x38] = new int[2] { 1, 66 };
            CPU.CycleArray[0xF8] = new int[2] { 1, 67 };
            CPU.CycleArray[0x78] = new int[2] { 1, 68 };
            CPU.CycleArray[0x85] = new int[3] { 1, 9, 49 };
            CPU.CycleArray[0x95] = new int[4] { 1, 9, 10, 81 };
            CPU.CycleArray[0x8D] = new int[4] { 1, 3, 4, 81 };
            CPU.CycleArray[0x9D] = new int[5] { 1, 3, 12, 83, 81 };
            CPU.CycleArray[0x99] = new int[5] { 1, 3, 13, 87, 81 };
            CPU.CycleArray[0x81] = new int[6] { 1, 16, 17, 18, 19, 81 };
            CPU.CycleArray[0x91] = new int[6] { 1, 16, 91, 92, 94, 81 };
            CPU.CycleArray[0x86] = new int[3] { 1, 9, 48 };
            CPU.CycleArray[0x96] = new int[4] { 1, 9, 11, 48 };
            CPU.CycleArray[0x8E] = new int[4] { 1, 3, 4, 48 };
            CPU.CycleArray[0x84] = new int[3] { 1, 9, 86 };
            CPU.CycleArray[0x94] = new int[4] { 1, 9, 10, 86 };
            CPU.CycleArray[0x8C] = new int[4] { 1, 3, 4, 86 };
            CPU.CycleArray[0xAA] = new int[2] { 1, 69 };
            CPU.CycleArray[0xA8] = new int[2] { 1, 70 };
            CPU.CycleArray[0xBA] = new int[2] { 1, 71 };
            CPU.CycleArray[0x8A] = new int[2] { 1, 72 };
            CPU.CycleArray[0x98] = new int[2] { 1, 73 };
            CPU.CycleArray[0x9A] = new int[2] { 1, 74 };
           
        }

        public static void Reset()
        {
            DrawFrame = false;
            bitmapData = null;
            nes.OutputPictureBox.Image = null;
            NESBMP= new Bitmap(256, 240, PixelFormat.Format32bppPArgb);
            DebuggerForm.Reset();
            APU.Reset();
            CPU.Reset();
            GAME.Reset();
            PPU.Reset();
            FramesPerSecond = 0;
            Debugger.Initialize();
            GC.Collect();
        }
       
        public Boolean LoadROM()
        {
            Reset();
            openFileDialog1.Filter = "NES files (*.nes)|*.nes";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result.ToString() != "Cancel")
            {
                GAME.GAMELOCATION = openFileDialog1.FileName;
                GAME.LoadGame(GAME.GAMELOCATION);
                this.Text = GAME.HEADERINFORMATION;
                return true;
            }
            return false;
        }      
        
        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            GAME.LoadGame(GAME.GAMELOCATION);
            this.Text = GAME.HEADERINFORMATION;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayLoop = false;
            if (LoadROM())
            {
                PlayNes();
                resetToolStripMenuItem.Enabled = true;
                if (DebuggerForm.Dbug!=null&&DebuggerForm.Dbug.Visible)
                {
                    DebuggerForm.UpdateDebugger();
                }
            }
        }

        public void PlayNes()
        {
            PlayLoop = true;
            BackgroundWorker RunNESWorker = new BackgroundWorker();
            RunNESWorker.DoWork += RunNESBackGroundWorker1_DoWork;
            RunNESWorker.ProgressChanged += new ProgressChangedEventHandler(RunNESBackGroundWorker_ProgressChanged);
            RunNESWorker.ProgressChanged += RunNESBackGroundWorker_ProgressChanged;
            RunNESWorker.WorkerSupportsCancellation = true;
            RunNESWorker.RunWorkerCompleted += RunNESBackGroundWorker1_RunWorkerCompleted;
            RunNESWorker.RunWorkerAsync();
        }
        
        private void StandardSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size = new System.Drawing.Size(272, 305);
            OutputPictureBox.Width = 256;
            OutputPictureBox.Height = 240;
            xToolStripMenuItem.Checked = true;
            xToolStripMenuItem1.Checked = false;
            fULLSCREENToolStripMenuItem.Checked = false;
        }

        private void DoubleSizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Size = new System.Drawing.Size(528, 545);
            OutputPictureBox.Width = 512;
            OutputPictureBox.Height = 480;
            xToolStripMenuItem1.Checked = true;
            xToolStripMenuItem.Checked = false;
            fULLSCREENToolStripMenuItem.Checked = false;
        }

        private void InputConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputConfig InputConfigurationForm = new InputConfig();
            InputConfigurationForm.ShowDialog();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["Debugger"] == null)           
            {
                DebuggerForm DebuggerForm = new DebuggerForm();
                DebuggerForm.Show();
            }          
        }

        private void pauseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PlayLoop = false;
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (LoadROM())
            {
                resetToolStripMenuItem.Enabled = true;
            }
        }

        private void FullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xToolStripMenuItem1.Checked = false;
            xToolStripMenuItem.Checked = false;
            fULLSCREENToolStripMenuItem.Checked = true;
            const int margin = 0;
            Rectangle rect = new Rectangle(
                Screen.PrimaryScreen.WorkingArea.X + margin,
                Screen.PrimaryScreen.WorkingArea.Y + margin,
                Screen.PrimaryScreen.WorkingArea.Width - 2 * margin,
                Screen.PrimaryScreen.WorkingArea.Height - 2 * margin);
            decimal StandardWidth = 2400m / (Screen.PrimaryScreen.WorkingArea.Height-60);
            decimal width = StandardWidth * 256; ;
            Size = new System.Drawing.Size(Convert.ToInt32(width-190), (Screen.PrimaryScreen.WorkingArea.Height));
            OutputPictureBox.Height = Screen.PrimaryScreen.WorkingArea.Height-60;
            OutputPictureBox.Width = Convert.ToInt32(width-205);
            this.CenterToScreen();
        }
        
        private void EnableSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableToolStripMenuItem.Checked == true)
            {
                APU.DisableAllSoundFromMenu = true;
                enableToolStripMenuItem.Checked = false;
            }
            else
            {
                APU.DisableAllSoundFromMenu = false;
                enableToolStripMenuItem.Checked = true;
            }
        }

        private void SoundChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundConfig SoundConfigForm = new SoundConfig();
            SoundConfigForm.Show();
        }

        private void RunNESBackGroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (PlayLoop && !e.Cancel)
            {
                bitmapData = NESBMP.LockBits(new Rectangle(0, 0, 256, 240), ImageLockMode.WriteOnly, NESBMP.PixelFormat);
                Iptr = bitmapData.Scan0;
                Marshal.Copy(Iptr, PPU.Pixels, 0, 245760);
                while (!DrawFrame && PlayLoop)
                {
                    if (Trace && (CPU.CycleNumber == 0)) { Debugger.GetTracedata(); }
                    CPU.RunCPU();
                    APU.RunAPU(1);
                    PPU.RunPPU(1);
                }
                Marshal.Copy(PPU.Pixels, 0, Iptr, 245760);
                try
                {
                    if (bitmapData != null)
                    {
                        NESBMP.UnlockBits(bitmapData);
                        RunNESBackGroundWorker.ReportProgress(0);
                    }
                 }
                finally
                {
                    DrawFrame = false;
                }              
            }
        }

        private void FramesPerSecondTimer_Tick(object sender, EventArgs e)
        {
            this.Text = "FPS: " + FramesPerSecond.ToString();
            FramesPerSecond = 0;
        }

        private bool closePending;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (RunNESBackGroundWorker.IsBusy)
            {
                closePending = true;
                RunNESBackGroundWorker.WorkerSupportsCancellation = true;
                RunNESBackGroundWorker.CancelAsync();
                e.Cancel = true;
                this.Enabled = false;
                return;
            }
            base.OnFormClosing(e);
        }
       
        private void RunNESBackGroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (closePending) this.Close();
            closePending = false;
        }

        private void resetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CPU.CurrentOpCode = 0x104;
            CPU.CycleNumber = 1;
        }

        private void NES_KeyDown(object sender, KeyEventArgs e)
        {
            if (Inputs.InputKeyBoardDictionary.ContainsKey(e.KeyCode.ToString()))
            {
                Inputs.NewInputDictionary[Inputs.InputKeyBoardDictionary[e.KeyCode.ToString()]] = 1;
            }
        }

        private void RunNESBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OutputPictureBox.Image = (Bitmap)NESBMP.Clone();
            FramesPerSecond++;
        }

        private void NES_KeyUp(object sender, KeyEventArgs e)
        {
            if (Inputs.InputKeyBoardDictionary.ContainsKey(e.KeyCode.ToString()))
            {
                Inputs.NewInputDictionary[Inputs.InputKeyBoardDictionary[e.KeyCode.ToString()]] = 0;
            }
        }
    }
}
