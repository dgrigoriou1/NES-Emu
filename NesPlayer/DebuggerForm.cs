using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
namespace NesPlayer
{
    public partial class DebuggerForm : Form
    {
        public static DebuggerForm Dbug;
        public static bool ShowCompareLogs = false;
        public static bool StopOnComparison = false;
        public static List<string[]> LogTraceList = new List<string[]>();
       
        public DebuggerForm()
        {
            Dbug = this;
            for (int i = 0; i < 1000; i++) {
                string[] newarr = new string[11];
                Debugger.TraceList.Add(newarr);
            }
            InitializeComponent();
            LoadTraceColumns();
            Debugger.LoadLogList();
            DisplayLogList();
        }
     
        private void TraceLog_Load(object sender, EventArgs e)
        {
            NES.Trace = true;
            MemoryRichTextBox.Font = new Font(FontFamily.GenericMonospace, MemoryRichTextBox.Font.Size, FontStyle.Bold);
            MemoryLeftLabelRichTextBox.Font = new Font(FontFamily.GenericMonospace, MemoryLeftLabelRichTextBox.Font.Size, FontStyle.Bold);
            MemoryToplabelRichTextBox.Font = new Font(FontFamily.GenericMonospace, MemoryRichTextBox.Font.Size, FontStyle.Bold);
            UpdateDebugger();
        }
       
       
        private void TraceLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            NES.Trace = false;
        }

        private void OneStepButton_Click(object sender, EventArgs e)
        {
            NES.PlayLoop = false;
            Debugger.StopOnCount = true;
            Debugger.NumberInstructionsToExecute = 1.ToString();
            NES.nes.PlayNes();
        }

         public static void Reset()
        {
            Debugger.TraceList = new List<string[]>();
            LogTraceList = new List<string[]>();
            for (int i = 0; i < 1000; i++)
            {
                string[] newarr = new string[11];
                Debugger.TraceList.Add(newarr);
            }
            Debugger.TracePointer = 0;
            Debugger.StopLine = 0;
            Debugger.INSTRUCTIONCOUNT = 0;
         }
     
        public void LoadTraceColumns()
        {            
            TraceDataGridView.ColumnCount = 11;
            TraceDataGridView.Columns[0].Name = "Instr #";
            TraceDataGridView.Columns[1].Name = "PC";
            TraceDataGridView.Columns[2].Name = "Bytes";
            TraceDataGridView.Columns[3].Name = "Execution";
            TraceDataGridView.Columns[4].Name = "A";
            TraceDataGridView.Columns[5].Name = "X";
            TraceDataGridView.Columns[6].Name = "Y";
            TraceDataGridView.Columns[7].Name = "PS";
            TraceDataGridView.Columns[8].Name = "SP";
            TraceDataGridView.Columns[9].Name = "CYC";
            TraceDataGridView.Columns[10].Name = "SL";
          
            TraceDataGridView.RowHeadersVisible = false;
            TraceDataGridView.Columns[0].Width = 50;
            TraceDataGridView.Columns[1].Width = 40;
            TraceDataGridView.Columns[2].Width = 72;
            TraceDataGridView.Columns[3].Width = 180;
            TraceDataGridView.Columns[4].Width = 38;
            TraceDataGridView.Columns[5].Width = 38;
            TraceDataGridView.Columns[6].Width = 38;
            TraceDataGridView.Columns[7].Width = 42;
            TraceDataGridView.Columns[8].Width = 45;
            TraceDataGridView.Columns[9].Width = 60;
            TraceDataGridView.Columns[10].Width = 56;

            OutPutDataGridView.ColumnCount = 11;
            OutPutDataGridView.RowHeadersVisible = false;
            OutPutDataGridView.Columns[0].Width = 50;
            OutPutDataGridView.Columns[1].Width = 40;
            OutPutDataGridView.Columns[2].Width = 72;
            OutPutDataGridView.Columns[3].Width = 180;
            OutPutDataGridView.Columns[4].Width = 38;
            OutPutDataGridView.Columns[5].Width = 38;
            OutPutDataGridView.Columns[6].Width = 38;
            OutPutDataGridView.Columns[7].Width = 42;
            OutPutDataGridView.Columns[8].Width = 45;
            OutPutDataGridView.Columns[9].Width = 60;
            OutPutDataGridView.Columns[10].Width = 56;
            LogDataGridView.ColumnCount = 11;
            LogDataGridView.RowHeadersVisible = false;
            LogDataGridView.Columns[0].Width = 50;
            LogDataGridView.Columns[1].Width = 40;
            LogDataGridView.Columns[2].Width = 72;
            LogDataGridView.Columns[3].Width = 180;
            LogDataGridView.Columns[4].Width = 38;
            LogDataGridView.Columns[5].Width = 38;
            LogDataGridView.Columns[6].Width = 38;
            LogDataGridView.Columns[7].Width = 42;
            LogDataGridView.Columns[8].Width = 45;
            LogDataGridView.Columns[9].Width = 60;
            LogDataGridView.Columns[10].Width = 56;


            DataGridViewCheckBoxColumn SelectACheckBoxColumn = new DataGridViewCheckBoxColumn();
            SelectACheckBoxColumn.Name = "Select";
            SelectACheckBoxColumn.HeaderText = "Select";
            SelectACheckBoxColumn.Width = 41;
            SelectACheckBoxColumn.ReadOnly = false;
            SelectACheckBoxColumn.FillWeight = 10; 
            LogViewDataGridView.Columns.Add(SelectACheckBoxColumn);
            LogViewDataGridView.ColumnCount = 8;
            LogViewDataGridView.Columns[1].Name = "Log File";
            LogViewDataGridView.Columns[2].Name = "ROM Location";
            LogViewDataGridView.Columns[3].Name = "INES";
            LogViewDataGridView.Columns[4].Name = "Old Diff At";
            LogViewDataGridView.Columns[5].Name = "View Diff At";
            LogViewDataGridView.Columns[6].Name = "Execution At Diff";
            LogViewDataGridView.Columns[7].Name = "Analysis";

            DataGridViewCheckBoxColumn CheckboxSelectColumn = new DataGridViewCheckBoxColumn();
            DataGridViewImageColumn PassImageColumn = new DataGridViewImageColumn();
            DataGridViewImageColumn PreviousImageColumn = new DataGridViewImageColumn();
            DataGridViewImageColumn CurrentImageColumn = new DataGridViewImageColumn();
            DataGridViewTextBoxColumn ROMNameTextBoxColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ChangeTextBoxColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn INESTextBoxColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ResultTextBoxColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn NumberInstructionTextBoxColumn = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn CheckBoxTestROMColumn = new DataGridViewCheckBoxColumn();
            CheckboxSelectColumn.Name = "Select";
            CheckboxSelectColumn.HeaderText = "Select";
            CheckboxSelectColumn.Width = 41;
            CheckboxSelectColumn.ReadOnly = false;
            CheckboxSelectColumn.FillWeight = 10;
            CheckboxSelectColumn.Name = "Select";
            CheckBoxTestROMColumn.HeaderText = "Test";
            CheckBoxTestROMColumn.Width = 36;
            CheckBoxTestROMColumn.ReadOnly = false;
            CheckBoxTestROMColumn.FillWeight = 10;
            ROMNameTextBoxColumn.Name = "ROM";
            ROMNameTextBoxColumn.HeaderText = "ROM";
            ROMNameTextBoxColumn.Width = 45;
            ROMNameTextBoxColumn.ReadOnly = false;
            INESTextBoxColumn.Name = "INES";
            INESTextBoxColumn.HeaderText = "INES";
            INESTextBoxColumn.Width = 38;
            INESTextBoxColumn.ReadOnly = false;
            PassImageColumn.Name = "Pass Image";
            PassImageColumn.HeaderText = "Pass Image";
            PassImageColumn.Width = 96;
            
            PassImageColumn.ReadOnly = false;
            PreviousImageColumn.Name = "Old Image";
            PreviousImageColumn.HeaderText = "Old Image";
            PreviousImageColumn.Width = 96;
            PreviousImageColumn.ReadOnly = false;
            CurrentImageColumn.Name = "Current Image";
            CurrentImageColumn.HeaderText = "Current Image";
            CurrentImageColumn.Width = 96;
            CurrentImageColumn.ReadOnly = false;
            ChangeTextBoxColumn.Name = "Change";
            ChangeTextBoxColumn.HeaderText = "Change";
            ChangeTextBoxColumn.Width = 50;
            ChangeTextBoxColumn.ReadOnly = false;
            ResultTextBoxColumn.Name = "Pass";
            ResultTextBoxColumn.HeaderText = "Pass";
            ResultTextBoxColumn.Width = 50;
            ResultTextBoxColumn.ReadOnly = false;
            NumberInstructionTextBoxColumn.Name = "# Instr";
            NumberInstructionTextBoxColumn.HeaderText = "# Instr";
            NumberInstructionTextBoxColumn.Width = 56;
            NumberInstructionTextBoxColumn.ReadOnly = false;
            TestROMDataGridView.Columns.Add(CheckboxSelectColumn);
            TestROMDataGridView.Columns.Add(ROMNameTextBoxColumn);
            TestROMDataGridView.Columns.Add(INESTextBoxColumn);
            TestROMDataGridView.Columns.Add(PassImageColumn);
            TestROMDataGridView.Columns.Add(PreviousImageColumn);
            TestROMDataGridView.Columns.Add(CurrentImageColumn);
            TestROMDataGridView.Columns.Add(ChangeTextBoxColumn);
            TestROMDataGridView.Columns.Add(ResultTextBoxColumn);
            TestROMDataGridView.Columns.Add(NumberInstructionTextBoxColumn);
            TestROMDataGridView.Columns.Add(CheckBoxTestROMColumn);
            // dataGridView5.ColumnCount = 6;

            TestROMDataGridView.RowHeadersVisible = false;
            TestROMDataGridView.Columns[0].ReadOnly = false;
            TestROMDataGridView.Columns[1].ReadOnly = true;
            TestROMDataGridView.Columns[2].ReadOnly = true;
            TestROMDataGridView.Columns[3].ReadOnly = true;
            TestROMDataGridView.Columns[4].ReadOnly = true;
            TestROMDataGridView.Columns[5].ReadOnly = true;
            TestROMDataGridView.Columns[6].ReadOnly = false;
            TestROMDataGridView.Columns[1].Width = 100;
            
            LogViewDataGridView.RowHeadersVisible = false;
            LogViewDataGridView.Columns[0].ReadOnly = false;
            LogViewDataGridView.Columns[1].ReadOnly = true;
            LogViewDataGridView.Columns[2].ReadOnly = true;
            LogViewDataGridView.Columns[3].ReadOnly = true;
            LogViewDataGridView.Columns[4].ReadOnly = true;
            LogViewDataGridView.Columns[5].ReadOnly = true;
            LogViewDataGridView.Columns[6].ReadOnly = true;
            LogViewDataGridView.Columns[7].ReadOnly = true;
            LogViewDataGridView.Columns[1].Width = 120;
            LogViewDataGridView.Columns[2].Width = 110;
            LogViewDataGridView.Columns[3].Width = 36;
            LogViewDataGridView.Columns[4].Width = 60;
            LogViewDataGridView.Columns[5].Width = 60;
            LogViewDataGridView.Columns[6].Width = 110;
            LogViewDataGridView.Columns[7].Width = 120;                    
        }
       

        public static void Showdata()
        {
            UpdateDebugger();
            BackgroundWorker ShowTraceListWorker = new BackgroundWorker();
            ShowTraceListWorker.DoWork += Dbug.ShowTraceListBackGroundWorker_DoWork;
            ShowTraceListWorker.RunWorkerAsync();           
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            NES.PlayLoop = true;
            NES.nes.PlayNes();
        }
     
        private void AddBreakPointButton_Click(object sender, EventArgs e)
        {
            List<string> BPList = BreakPointsListBox.Items.Cast<String>().ToList();
            Debugger.DecodeBreakpoints(BPList);
            BreakPointPopUp.IsEdit = false;
            BreakPointPopUp BreakPointPopUpForm = new BreakPointPopUp();
            BreakPointPopUpForm.ShowDialog();
        }
      
        private void DeleteBreakPointButton_Click(object sender, EventArgs e)
        {
            BreakPointsListBox.Items.RemoveAt(BreakPointsListBox.SelectedIndex);
            List<string> BPList = BreakPointsListBox.Items.Cast<String>().ToList();
           Debugger.DecodeBreakpoints(BPList);
        }

        private void BreakPointListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BreakPointsListBox.SelectedIndex >= 0)
            {
                EditBreakpointButton.Enabled = true;
                DeleteBreakpointButton.Enabled = true;
            }
            else
            {
                EditBreakpointButton.Enabled = false;
                DeleteBreakpointButton.Enabled = false;
            }
        }

        private void EditBreakPointButton_Click(object sender, EventArgs e)
        {            
            BreakPointPopUp.IsEdit = true;
            BreakPointPopUp.BreakPointToEdit = BreakPointsListBox.SelectedItem.ToString();
            BreakPointPopUp BreakPointPopupForm = new BreakPointPopUp();
            BreakPointPopupForm.ShowDialog();
        }

        private void NumberInstructionButton_Click(object sender, EventArgs e)
        {
            if (NumInstructionsTextBox.Text.ToString().Trim().Length > 0 && int.TryParse(NumInstructionsTextBox.Text.ToString().Trim(), out int n))
            {
                Debugger.StopOnCount = true;
                Debugger.NumberInstructionsToExecute = NumInstructionsTextBox.Text.ToString().Trim();
                NES.PlayLoop = false;
                NES.nes.PlayNes();
            }
        }

        private void LoadTraceDatabackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> SelectedTestRomList = GetSelectedTestROMS2();
            int EOFCount = 0;
            for (int i = 0; i < SelectedTestRomList.Count; i++)
            {
                NES.PlayLoop = false;
                NES.Reset();
                int CurrentTestROMIndex = Convert.ToInt32(SelectedTestRomList[i]);
                string CurrentKey = TestROMDataGridView.Rows[CurrentTestROMIndex].Cells[1].Value.ToString();
                this.Invoke(new MethodInvoker(delegate { TraceDataGridView.Rows.Clear(); }));
                GAME.LoadGame(Debugger.TestROMList[CurrentKey][8]);
                int NumberInstructionToExecute = Convert.ToInt32(Debugger.TestROMList[CurrentKey][9]);
                this.Invoke(new MethodInvoker(delegate
                {
                    TestROMProgressLabel.Text = "Processing: " + (i + 1).ToString() + "/" + SelectedTestRomList.Count + " " + GAME.GAMENAME;
                }));
                
                IntPtr Iptr = IntPtr.Zero;
                Bitmap NESBMPFRAME3 = new Bitmap(256, 240, PixelFormat.Format32bppPArgb);
                BitmapData bitmapData = NESBMPFRAME3.LockBits(new Rectangle(0, 0, 256, 240), ImageLockMode.WriteOnly, NESBMPFRAME3.PixelFormat);
                Iptr = bitmapData.Scan0;
                Marshal.Copy(Iptr, PPU.Pixels, 0, 245760);
                for (int d = 0; d < NumberInstructionToExecute; d++)
                {
                    CPU.RunCPU();
                    APU.RunAPU(1);
                    PPU.RunPPU(1);
                }
                Marshal.Copy(PPU.Pixels, 0, Iptr, 245760);
                NESBMPFRAME3.UnlockBits(bitmapData);
                string CurrentImageName = Debugger.TestROMList[CurrentKey][8].Replace(".nes", "-CUR.bmp");
                string OldImageName = Debugger.TestROMList[CurrentKey][8].Replace(".nes", "-OLD.bmp");
                string PassImageName = Debugger.TestROMList[CurrentKey][8].Replace(".nes", "-PASS.bmp");
                System.IO.Directory.CreateDirectory("Test");
                string CurrentImagePath = Path.Combine(Environment.CurrentDirectory, @"Test\", CurrentImageName);
                string OldImagePath = Path.Combine(Environment.CurrentDirectory, @"Test\", OldImageName);
                string PassImagePath = Path.Combine(Environment.CurrentDirectory, @"Test\", PassImageName);
                Bitmap CurrentBitmap = (Bitmap)NESBMPFRAME3.GetThumbnailImage(96, 90, null, IntPtr.Zero);
                Debugger.TestROMList[CurrentKey][5] = CurrentImagePath;
                this.Invoke(new MethodInvoker(delegate
                {
                    TestROMDataGridView.Rows[CurrentTestROMIndex].Cells[5].Value = CurrentBitmap;
                }));
                if (File.Exists(CurrentImagePath))
                {
                    System.IO.File.Delete(OldImagePath);
                    System.IO.File.Move(CurrentImagePath, OldImagePath);
                    Debugger.TestROMList[CurrentKey][4] = OldImagePath;
                    Debugger.TestROMList[CurrentKey][5] = CurrentImagePath;
                    NESBMPFRAME3.Save(CurrentImagePath);
                    Bitmap OldBitmap = (Bitmap)(new Bitmap(OldImagePath).GetThumbnailImage(96,90,null,IntPtr.Zero));
                    bool Isequal = CompareMemCmp(CurrentBitmap, OldBitmap);
                    Debugger.TestROMList[CurrentKey][6] = (!Isequal).ToString().ToUpper();
                    OldBitmap.Dispose();
                }
                else
                {
                    CurrentBitmap.Save(CurrentImagePath);
                }
                if (File.Exists(PassImagePath) && Debugger.TestROMList[CurrentKey][3] != null)
                {
                    Bitmap PassBitmap = (Bitmap)(new Bitmap(PassImagePath).GetThumbnailImage(96, 90, null, IntPtr.Zero));
                    bool Isequal = CompareMemCmp(CurrentBitmap, PassBitmap);
                    Debugger.TestROMList[CurrentKey][7] = Isequal.ToString().ToUpper();
                    if (Isequal) { EOFCount++; }
                    PassBitmap.Dispose();
                }
                else
                {
                    Debugger.TestROMList[CurrentKey][7] = "FALSE";
                }
                this.Invoke(new MethodInvoker(delegate
                {
                    TestROMProgLabel.Text = "Passed: " + EOFCount.ToString() + "/" + SelectedTestRomList.Count;
                }));
                CurrentBitmap.Dispose();
                this.Invoke(new MethodInvoker(delegate
                {
                    TestROMDataGridView.Rows[CurrentTestROMIndex].Cells[6].Value = Debugger.TestROMList[CurrentKey][(6)];
                    TestROMDataGridView.Rows[CurrentTestROMIndex].Cells[7].Value = Debugger.TestROMList[CurrentKey][(7)];
                    TestROMDataGridView.Rows[CurrentTestROMIndex].Cells[8].Value = Debugger.TestROMList[CurrentKey][(9)];
                }));
            }
            this.Invoke(new MethodInvoker(delegate
            {
                DisplayTestROMList();
            }));
        }

        private void ShowMemorybackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int MemoryIndex = 0;
            StringBuilder MemoryDataStringBuilder = new StringBuilder();
            StringBuilder MemoryLabelStringBuilder = new StringBuilder();
            int ArraySize = 64* 1024;
            if (MemoryCPURadioButton.Checked)
            {
                ArraySize = 64 * 1024;
            }
            else if (MemoryPPURadioButton.Checked)           
            {
                ArraySize = 32 * 1024;
            }
            else if (MemoryROMRadioButton.Checked)
            {
                ArraySize = GAME.ROMBYTEDATA.Length;
            }
            else if (MemorySearchValueRadioButton.Checked)
            {
                ArraySize = GAME.PRGRAM.Length;
            }
            for (int y = 0; y < ArraySize; y++)
            {
                if (MemoryIndex == 0)
                {
                    MemoryLabelStringBuilder.Append(y.ToString("X5")).Append("\n");
                }
                if (MemoryCPURadioButton.Checked)
                {
                    MemoryDataStringBuilder.Append(CPU.CPUMemory[y].ToString("X2")).Append(" ");
                }
                else if (MemoryPPURadioButton.Checked)
                {
                    MemoryDataStringBuilder.Append(PPU.PPUMemory[y].ToString("X2")).Append(" ");
                }
                else if (MemoryROMRadioButton.Checked)
                {
                    MemoryDataStringBuilder.Append(GAME.ROMBYTEDATA[y].ToString("X2")).Append(" ");
                }
                else if (MemorySearchValueRadioButton.Checked)
                {
                    MemoryDataStringBuilder.Append(GAME.PRGRAM[y].ToString("X2")).Append(" ");
                }
                MemoryIndex++;
                if (MemoryIndex > 15) {
                    MemoryIndex = 0;
                    MemoryDataStringBuilder.Append("\n");
                } else if (MemoryIndex % 4 == 0) {
                    MemoryDataStringBuilder.Append(" ");
                }
            }
            this.Invoke(new MethodInvoker(delegate {
                MemoryRichTextBox.Text = MemoryDataStringBuilder.ToString();            
                MemoryLeftLabelRichTextBox.Text = MemoryLabelStringBuilder.ToString();           
                if (GAME.MAPPERNUMBER == 1)
                {
                    if (!RegisterTabControl.TabPages.Contains(MMC1TabPage))
                    {
                        RegisterTabControl.TabPages.Insert(3, MMC1TabPage);
                    }
                }
                else
                {
                    RegisterTabControl.TabPages.Remove(MMC1TabPage);
                }
                TimingTicksTextBox.Text = PPU.PPUCycleCount.ToString() + "/" + Convert.ToDouble(PPU.PPUCycleCount / 3.00).ToString("#.000");
                CPUBank1TextBox.Text = GAME.PRGBankData[0].ToString("X2");
                CPUBank2TextBox.Text = GAME.PRGBankData[1].ToString("X2");
                CPUBank3TextBox.Text = GAME.PRGBankData[2].ToString("X2");
                CPUBank4TextBox.Text = GAME.PRGBankData[3].ToString("X2");
                CPUBank5TextBox.Text = GAME.PRGBankData[4].ToString("X2");
                CPUBank6TextBox.Text = GAME.PRGBankData[5].ToString("X2");
                CPUBank7TextBox.Text = GAME.PRGBankData[6].ToString("X2");
                CPUBank8TextBox.Text = GAME.PRGBankData[7].ToString("X2");

                PPUBank1TextBox.Text = GAME.CHRBankData[0].ToString("X2");
                PPUBank2TextBox.Text = GAME.CHRBankData[1].ToString("X2");
                PPUBank3TextBox.Text = GAME.CHRBankData[2].ToString("X2");
                PPUBank4TextBox.Text = GAME.CHRBankData[3].ToString("X2");
                PPUBank5TextBox.Text = GAME.CHRBankData[4].ToString("X2");
                PPUBank6TextBox.Text = GAME.CHRBankData[5].ToString("X2");
                PPUBank7TextBox.Text = GAME.CHRBankData[6].ToString("X2");
                PPUBank8TextBox.Text = GAME.CHRBankData[7].ToString("X2");
                if (CPU.ProcessorStatus[7] == 1) { NFlagCheckBox.Checked = true; } else { NFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[6] == 1) { VFlagCheckBox.Checked = true; } else { VFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[5] == 1) { RFlagCheckBox.Checked = true; } else { RFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[4] == 1) { BFlagCheckBox.Checked = true; } else { BFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[3] == 1) { DFlagCheckBox.Checked = true; } else { DFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[2] == 1) { IFlagCheckBox.Checked = true; } else { IFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[1] == 1) { ZFlagCheckBox.Checked = true; } else { ZFlagCheckBox.Checked = false; }
                if (CPU.ProcessorStatus[0] == 1) { CFlagCheckBox.Checked = true; } else { CFlagCheckBox.Checked = false; }
                TimingScanLinesTextBox.Text = PPU.Scanlines.ToString();
                TimingAPUFramTextBox.Text = APU.APUFrameCounter.ToString();
                TimingVRAMTextBox.Text = PPU.PPUADDR2006.ToString("X4");
                if (PPU.NMIOCCURED) { NMICheckBox.Checked = true; } else { NMICheckBox.Checked = false; }
                if (DMC.IRQENABLE==1) { DMCCheckBox.Checked = true; } else { DMCCheckBox.Checked = false; }
                if (INES4.MMC3IRQ) { MMC3CheckBox.Checked = true; } else { MMC3CheckBox.Checked = false; }

                if (RegisterTabControl.SelectedTab == RegisterTabControl.TabPages["CPUTabPage"])
                {
                    StringBuilder ps = new StringBuilder();
                    foreach (int a in CPU.ProcessorStatus) { ps.Append(a); }
                    CPUAccumalatorTextBox.Text = CPU.Accumulator.ToString("X2");
                    CPUXRegisterTextBox.Text = CPU.IndexRegisterX.ToString("X2");
                    CPUYRegisterTextBox.Text = CPU.IndexRegisterY.ToString("X2");
                    CPUProccessorStatusTextBox.Text = Convert.ToInt32(ps.ToString(), 2).ToString("X2");
                    CPUStackPointerTextBox.Text = CPU.StackPointer.ToString("X2");
                    CPUProgramCounterTextBox.Text = CPU.ProgramCounter.ToString("X4");
                }
                else if (RegisterTabControl.SelectedTab == RegisterTabControl.TabPages["PPUTabPage"])
                {
                    PPUCNTRL2000TextBox.Text = PPU.PPUCTRL2000.ToString("X2");
                    PPUMask2001TextBox.Text = PPU.PPUMASK2001.ToString("X2");
                    PPUStatus2002TextBox.Text = PPU.PPUSTATUS2002.ToString("X2");
                    PPUOAMADDR2003TextBox.Text = PPU.OAMADDR2003.ToString("X2");
                    PPUOAMData2004TextBox.Text = PPU.OAMDATA2004.ToString("X2");
                    PPUScroll2005TextBox.Text = PPU.PPUSCROLL2005.ToString("X2");
                    PPUADDR2006TextBox.Text = PPU.PPUADDR2006.ToString("X2");
                    PPUData2007TextBox.Text = PPU.PPUDATA2007.ToString("X2");
                    PPUReadBufferTextBox.Text = PPU.InternalReadBuffer2007.ToString("X2");
                    PPUOddCycleTextBox.Text = PPU.OddCycle.ToString();
                    PPUVerticalTextBox.Text = PPU.VERTICAL.ToString("X2");
                    PPUScrollLowTextBox.Text = PPU.PPUScrollLow.ToString("X2");
                    PPUScrollHighTextBox.Text = PPU.PPUScrollHigh.ToString("X2");
                    PPUSpriteOverFlowTextBox.Text = PPU.SpriteOverflowCount.ToString("X2");
                    PPUTotalCyclesTextBox.Text = PPU.TOTALCYCLES.ToString("X2");
                    PPUCycleCountTextBox.Text = PPU.PPUCycleCount.ToString("X2");
                    PPULoopyTTextBox.Text = PPU.LoopyT.ToString("X4");
                    PPUXfineCountTextBox.Text = PPU.FineX.ToString("X2");
                    PPUVblankOccuredTextBox.Text = PPU.VBlankOccured.ToString();
                    PPUScanLinesTextBox.Text = PPU.Scanlines.ToString();
                    PPUOAMReadBufferTextBox.Text = PPU.OAMReadBuffer.ToString("X2");
                    PPUM2TextBox.Text = PPU.m2.ToString("X2");
                    PPUN2TextBox.Text = PPU.n2.ToString("X2");
                    PPUSprite2Textbox.Text = PPU.SpriteCount.ToString("X2");
                    PPUDisableOAMTextBox.Text = PPU.DISABLEWRITESTOSECONDOAM.ToString();
                    PPUNMIOccuredTextBox.Text = PPU.NMIOCCURED.ToString();
                    PPUVRAMTextBox.Text = PPU.VRAMLow.ToString("X2");
                    PPUVRAMHighTextBox.Text = PPU.VRAMHigh.ToString("X2");
                    PPUOAMIndexTextBox.Text = PPU.OAMIndex.ToString("X2");
                }
                else if (RegisterTabControl.SelectedTab == RegisterTabControl.TabPages["APUTabPage"])
                {
                    APU4015TextBox.Text = APU.APU4015.ToString("X2");
                    APU4017TextBox.Text = APU.APU4017.ToString();
                    FrameCounterTextBox.Text = APU.APUFrameCounter.ToString("X2");
                    JitterTextBox.Text = APU.Jitter.ToString("X2");

                    P1ConstantVolTextBox.Text = PULSE1.CONSTANTVOLUME.ToString("X2");
                    P1DecayLevelTextBox.Text = PULSE1.DECAYLEVELCOUNTER.ToString("X2");
                    P1DutyTextBox.Text = PULSE1.DUTY.ToString("X2");
                    P1DutyBitTextBox.Text = PULSE1.DUTYBIT.ToString("X2");
                    P1EnvelopeTextBox.Text = PULSE1.ENVELOPE.ToString("X2");
                    P1LoopHaltTextBox.Text = PULSE1.ENVELOPELOOPLENGTHHALT.ToString("X2");
                    P1lengthTextBox.Text = PULSE1.LENGTH.ToString("X2");
                    P1LengthCounterLoadTextBox.Text = PULSE1.LENGTHCOUNTERLOAD.ToString();
                    P1NegateTextBox.Text = PULSE1.NEGATE.ToString();
                    P1PeriodTextBox.Text = PULSE1.PERIOD.ToString("X2");
                    P1ShiftTextBox.Text = PULSE1.SHIFT.ToString("X2");
                    P1SweepDividerTextBox.Text = PULSE1.SWEEPDIVDERCOUNTER.ToString("X2");
                    P1SweepEnableTextBox.Text = PULSE1.SWEEPENABLE.ToString("X2");
                    P1SweepReloadTextBox.Text = PULSE1.SWEEPRELOAD.ToString();
                    P1TimerTextBox.Text = PULSE1.TIMER.ToString("X2");
                    P1TimerCounterTextBox.Text = PULSE1.TIMERCOUNTER.ToString("X2");
                    P1VolumeTextBox.Text = PULSE1.VOLUME.ToString("X2");

                    P2ConstantVolumeTextBox.Text = PULSE2.CONSTANTVOLUME.ToString("X2");
                    P2DecayLevelTextBox.Text = PULSE2.DECAYLEVELCOUNTER.ToString("X2");
                    P2dutyTextBox.Text = PULSE2.DUTY.ToString("X2");
                    P2DutyBitTextBox.Text = PULSE2.DUTYBIT.ToString("X2");
                    P2EnvelopeTextBox.Text = PULSE2.ENVELOPE.ToString("X2");
                    P2LoopHaltTextBox.Text = PULSE2.ENVELOPELOOPLENGTHHALT.ToString("X2");
                    P2LengthTextBox.Text = PULSE2.LENGTH.ToString("X2");
                    P2LengthCounterLoadTextBox.Text = PULSE2.LENGTHCOUNTERLOAD.ToString();
                    P2NegateTextBox.Text = PULSE2.NEGATE.ToString();
                    P2PeriodTextBox.Text = PULSE2.PERIOD.ToString("X2");
                    P2ShiftTextBox.Text = PULSE2.SHIFT.ToString("X2");
                    P2SweepDividerTextBox.Text = PULSE2.SWEEPDIVDERCOUNTER.ToString("X2");
                    P2SweepEnableTextBox.Text = PULSE2.SWEEPENABLE.ToString("X2");
                    P2SweepReloadTextBox.Text = PULSE2.SWEEPRELOAD.ToString();
                    P2TimerTextBox.Text = PULSE2.TIMER.ToString("X2");
                    P2TimerCounterTextBox.Text = PULSE2.TIMERCOUNTER.ToString("X2");
                    P2VolumeTextBox.Text = PULSE2.VOLUME.ToString("X2");

                    TriangleLengthTextBox.Text = TRIANGLE.LENGTH.ToString("X2");
                    TriangleLinearCounterHaltTextBox.Text = TRIANGLE.LINEARCONTROLLENGTHHALT.ToString("X2");
                    TriangleVolumeTextBox.Text = TRIANGLE.VOLUME.ToString("X2");
                    TriangleLinearCounterTextBox.Text = TRIANGLE.LINEARCOUNTER.ToString("X2");
                    TriangleLinearCounterLoadValueTextBox.Text = TRIANGLE.LINEARCOUNTERLOADVALUE.ToString("X2");
                    TriangleLinearCounterLoadTextBox.Text = TRIANGLE.LINEARCOUNTERLOAD.ToString();
                    TriangleTimerTextBox.Text = TRIANGLE.TIMER.ToString();
                    TriangleTimerCounterTextBox.Text = TRIANGLE.TIMERCOUNTER.ToString();
                    TriangleLengthCounterLoadTextBox.Text = TRIANGLE.LENGTHCOUNTERLOAD.ToString("X2");

                    NoiseDecalLevelTextBox.Text = NOISE.DECAYLEVELCOUNTER.ToString("X2");
                    NoiseLoopTextBox.Text = NOISE.LOOPNOISE.ToString("X2");
                    NoiseConstantVolumeTextBox.Text = NOISE.CONSTANTVOLUME.ToString("X2");
                    NoiseLoopHaltTextBox.Text = NOISE.ENVELOPELOOPLENGTHHALT.ToString("X2");
                    NoiseEnvelopeTextBox.Text = NOISE.ENVELOPE.ToString("X2");
                    NoisePeriodTextBox.Text = NOISE.NOISEPERIOD.ToString();
                    NoiseShiftRegisterTextBox.Text = NOISE.SHIFTREGISTER.ToString();
                    NoiseLengthTextBox.Text = NOISE.LENGTH.ToString("X2");
                    NoiseVolumeTextBox.Text = NOISE.VOLUME.ToString("X2");
                    NoiseTimerCounterTextBox.Text = NOISE.TIMERCOUNTER.ToString("X2");
                    NoiseLengthCounterLoadTextBox.Text = NOISE.LENGTHCOUNTERLOAD.ToString("X2");

                    DMCIRQEnableTextBox.Text = DMC.IRQENABLE.ToString();
                    DMCBytesRemainingTextBox.Text = DMC.BYTESREMAINING.ToString();
                    DMCBitsRemainingTextBox.Text = DMC.BITSREMAINING.ToString("X2");
                    DMCLoopTextBox.Text = DMC.LOOP.ToString("X2");
                    DMCFrequencyTextBox.Text = DMC.FREQUENCY.ToString("X2");
                    DMCLoadCounterTextBox.Text = DMC.LOADCOUNTER.ToString("X2");
                    DMCOutputTextBox.Text = DMC.DMCOUTPUT.ToString("X2");
                    DMCSampleAddressTextBox.Text = DMC.SAMPLEADDRESS.ToString("X2");
                    DMCCurrentAddressTextBox.Text = DMC.CURRENTADDRESS.ToString("X2");
                    DMCSampleLengthTextBox.Text = DMC.SAMPLELENGTH.ToString("X2");
                    DMCSampleBufferTextBox.Text = DMC.SAMPLEBUFFER.ToString("X2");
                    DMCTimerCounterTextBox.Text = DMC.TIMERCOUNTER.ToString();
                }
                if (GAME.GAMENAME != null)
                {
                    GameNameTextBox.Text = GAME.GAMENAME.ToString();
                }
                GameINESTextBox.Text = GAME.MAPPERNUMBER.ToString();
                GameMirroringTextBox.Text = GAME.Mirroring.ToString();
                GamePRGTextBox.Text = GAME.NUMPRGPAGES.ToString();
                GameCHRTextBox.Text = GAME.NUMCHRPAGES.ToString();
                IgnorMirroingCheckBox.Checked = GAME.IgnoreMirroring;
                BatteryCheckBox.Checked = GAME.Battery;

                if (GAME.MAPPERNUMBER == 1)
                {
                    MMC1CountTextBox.Text = INES1.MMC1COUNT.ToString("X2");
                    MMC15BitValueTextBox.Text = INES1.MMC1COUNT.ToString("X2");
                    MMC1Reg0TextBox.Text = INES1.MMC1REG0.ToString("X2");
                    MMC1Reg1TextBox.Text = INES1.MMC1REG1.ToString("X2");
                    MMC1Reg2TextBox.Text = INES1.MMC1REG2.ToString("X2");
                    MMC1Reg3TextBox.Text = INES1.MMC1REG3.ToString();                   
                }
            }));
        }

        private void CPURadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDebugger();
        }

        private void PPURadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDebugger();
        }
        public static void UpdateDebugger()
        {
            BackgroundWorker ShowMemoryWorker = new BackgroundWorker();
            ShowMemoryWorker.DoWork += Dbug.ShowMemorybackGroundWorker_DoWork;
            ShowMemoryWorker.RunWorkerAsync();
        }

        private void MemorySearchButton_Click(object sender, EventArgs e)
        {
            string SearchField = MemorySearchTextBox.Text.ToString().ToUpper();
            var IsHex = System.Text.RegularExpressions.Regex.IsMatch(SearchField, @"\A\b[0-9a-fA-F]+\b\Z");
            if (IsHex)
            {
                if (MemorySearchValueRadioButton.Checked)
                {

                    int StartingIndex = MemoryRichTextBox.SelectionStart;
                    this.MemoryRichTextBox.SelectionStart = 0;
                    int SearchIndex = MemoryRichTextBox.SelectionStart + 1;
                    MemoryOutTextBox.Text = "";
                    if (SearchIndex < MemoryRichTextBox.Text.Length)
                    {
                        int FoundIndex = this.MemoryRichTextBox.Find(SearchField, SearchIndex, MemoryRichTextBox.Text.Length, RichTextBoxFinds.None);
                        if (FoundIndex > 0)
                        {
                            this.MemoryRichTextBox.SelectionBackColor = Color.Yellow;
                            this.MemoryRichTextBox.Focus();
                            int LineFoundAt = MemoryRichTextBox.GetLineFromCharIndex(FoundIndex);
                            if (StartingIndex > FoundIndex) { MemoryLeftLabelRichTextBox.SelectionStart = MemoryLeftLabelRichTextBox.Find(MemoryLeftLabelRichTextBox.Lines[LineFoundAt]); }
                            else
                            {
                                MemoryLeftLabelRichTextBox.SelectionStart = MemoryLeftLabelRichTextBox.Find(MemoryLeftLabelRichTextBox.Lines[LineFoundAt - 8]);
                            }
                            MemoryLeftLabelRichTextBox.ScrollToCaret();
                            MemorySearchFindAgainButton.Visible = true;
                            MemorySearchCancelButton.Visible = true;
                            MemorySearchButton.Visible = false;
                        }
                        else
                        {
                            MemorySearchFindAgainButton.Visible = false;
                            MemorySearchButton.Visible = false;
                            MemoryOutTextBox.Text = "Not Found EOF";
                            MemorySearchCancelButton.Visible = true;
                        }
                    }
                    else
                    {
                        this.MemoryRichTextBox.SelectionStart = 0;
                        MemoryOutTextBox.Text = "Not Found EOF";
                        MemorySearchCancelButton.Visible = true;
                    }
                }
                else if (MemorySearchAddressRadioButton.Checked)
                {
                    int MemoryToFind = Convert.ToInt32(SearchField, 16) / 16;
                    if (MemoryToFind < MemoryLeftLabelRichTextBox.Lines.Count())
                    {
                        MemoryLeftLabelRichTextBox.SelectionStart = MemoryLeftLabelRichTextBox.Find(MemoryLeftLabelRichTextBox.Lines[MemoryToFind]);
                        MemoryLeftLabelRichTextBox.ScrollToCaret();

                        MemoryRichTextBox.SelectionStart = MemoryRichTextBox.Find(MemoryRichTextBox.Lines[MemoryToFind]);
                        MemoryRichTextBox.ScrollToCaret();
                    }
                }
            }
        }
        
        private void SearchFindAgainButton_Click(object sender, EventArgs e)
        {
            int StartIndex = MemoryRichTextBox.SelectionStart;
            string Searchfield = MemorySearchTextBox.Text.ToString().ToUpper();
            var IsHex = System.Text.RegularExpressions.Regex.IsMatch(Searchfield, @"\A\b[0-9a-fA-F]+\b\Z");
            if (IsHex)
            {
                int i = MemoryRichTextBox.SelectionStart + 1;
                if (i < MemoryRichTextBox.Text.Length)
                {
                    int FoundIndex = this.MemoryRichTextBox.Find(Searchfield, i, MemoryRichTextBox.Text.Length, RichTextBoxFinds.None);
                    if (FoundIndex > 0)
                    {
                        this.MemoryRichTextBox.SelectionBackColor = Color.Yellow;
                        this.MemoryRichTextBox.Focus();
                        int ValueFound = MemoryRichTextBox.GetLineFromCharIndex(FoundIndex);
                        if (StartIndex > FoundIndex)
                        {
                            MemoryLeftLabelRichTextBox.SelectionStart = MemoryLeftLabelRichTextBox.Find(MemoryLeftLabelRichTextBox.Lines[ValueFound]);
                        }
                        else
                        {
                            MemoryLeftLabelRichTextBox.SelectionStart = MemoryLeftLabelRichTextBox.Find(MemoryLeftLabelRichTextBox.Lines[ValueFound - 8]);
                        }
                        MemoryLeftLabelRichTextBox.ScrollToCaret();
                        MemorySearchFindAgainButton.Visible = true;
                        MemorySearchButton.Visible = false;
                        MemorySearchCancelButton.Visible = true;
                    }
                    else
                    {
                        MemorySearchFindAgainButton.Visible = false;
                        MemorySearchCancelButton.Visible = true;
                        MemoryOutTextBox.Text = "Not Found EOF";
                    }
                }
                else
                {
                    MemorySearchFindAgainButton.Visible = false;
                    MemorySearchCancelButton.Visible = true;
                    MemoryOutTextBox.Text = "Not Found EOF";
                }
            }
        }

        private void SearchCancelButton_Click(object sender, EventArgs e)
        {
            MemorySearchCancelButton.Visible = false;
            MemorySearchButton.Visible = true;
            MemorySearchFindAgainButton.Visible = false;
            this.MemoryRichTextBox.SelectionStart = 0;
            this.MemoryRichTextBox.SelectionLength = MemoryRichTextBox.Text.Length;
            this.MemoryRichTextBox.SelectionBackColor =  DebuggerForm.DefaultBackColor;
            this.MemoryRichTextBox.SelectionStart = 0;
            this.MemoryRichTextBox.SelectionLength = 0;
            this.MemoryLeftLabelRichTextBox.SelectionStart = 0;
            MemoryRichTextBox.ScrollToCaret();
            MemoryLeftLabelRichTextBox.ScrollToCaret();
            MemoryOutTextBox.Text = "";
        }

        private void ShowTraceListBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int CurrentTraceIndex = Debugger.TracePointer +1;
            int LastIndexShown = 0;
            this.Invoke(new MethodInvoker(delegate {
                if (this.TraceDataGridView.Rows.Count > 1)
                {
                    LastIndexShown = Convert.ToInt32(this.TraceDataGridView.Rows[this.TraceDataGridView.Rows.Count - 2].Cells[0].Value.ToString());
                }
            }));
            for (int i = CurrentTraceIndex; i < 1000; i++)
            {               
                this.Invoke(new MethodInvoker(delegate {
                    if (Debugger.TraceList[i][0] != null)
                    {
                        if (LastIndexShown < Convert.ToInt32(Debugger.TraceList[i][0]))
                        {
                            TraceDataGridView.Rows.Add(Debugger.TraceList[i]);
                        }
                    }
                }));
            }
            for (int i = 0; i < Debugger.TracePointer; i++)
            {
                this.Invoke(new MethodInvoker(delegate {
                    if (LastIndexShown < Convert.ToInt32(Debugger.TraceList[i][0]))
                    {
                        TraceDataGridView.Rows.Add(Debugger.TraceList[i]);
                    }
                }));
            }
            this.Invoke(new MethodInvoker(delegate {
                TraceDataGridView.ClearSelection();
                if (TraceDataGridView.Rows.Count > 1)
                {
                    TraceDataGridView.Rows[TraceDataGridView.Rows.Count - 2].Selected = true;
                    TraceDataGridView.CurrentCell = TraceDataGridView[0, TraceDataGridView.Rows.Count - 2];
                    TraceDataGridView.FirstDisplayedScrollingRowIndex = TraceDataGridView.Rows.Count - 2;
                }
            }));
        }

        private void ROMRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDebugger();
        }

        private void PowerButton_Click(object sender, EventArgs e)
        {
            NES.PlayLoop = false;
            NES.Reset();
            TraceDataGridView.Rows.Clear();
            GAME.LoadGame(GAME.GAMELOCATION);
            UpdateDebugger();
        }
        
        private void AddNewLogButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Log files (*.debug)|*.debug";
            DialogResult LogFileDialogResult = openFileDialog1.ShowDialog();
            if (LogFileDialogResult.ToString() != "Cancel")
            {
                Debugger.AddNewLog(openFileDialog1.FileName);
            }
            DisplayLogList();
        }
        
        public void DisplayLogList()
        {
            for (int f = 0; f < LogViewDataGridView.Rows.Count; f++)
            {
                string CurrentCellKey = LogViewDataGridView.Rows[f].Cells[1].Value.ToString();
                string Selection = LogViewDataGridView.Rows[f].Cells[0].Value.ToString().ToUpper();
                if (Debugger.LogList.ContainsKey(CurrentCellKey))
                {
                    Debugger.LogList[CurrentCellKey][0] = Selection;
                }
            }
            List<string> loglistkeys = Debugger.LogList.Keys.ToList();
            LogViewDataGridView.Rows.Clear();
            for (int i = 0; i < loglistkeys.Count; i++)
            {
                LogViewDataGridView.Rows.Add();
                bool IsSelected = false;
                if (Debugger.LogList[loglistkeys[i]][0] == "TRUE") { IsSelected = true; }
                LogViewDataGridView.Rows[i].Cells[0].Value = IsSelected;
                for (int d = 1; d < 8; d++)
                {
                    LogViewDataGridView.Rows[i].Cells[d].Value = Debugger.LogList[loglistkeys[i]][(d)];
                }
            }
           Debugger.PrintLogList();
           this.LogViewDataGridView.Sort(this.LogViewDataGridView.Columns["Analysis"], ListSortDirection.Ascending);
        }

        public void DisplayTestROMList() //
        {
            for (int f = 0; f < TestROMDataGridView.Rows.Count; f++)
            {
                string CurrentCellKey = TestROMDataGridView.Rows[f].Cells[1].Value.ToString();
                string Selection = TestROMDataGridView.Rows[f].Cells[0].Value.ToString().ToUpper();
                if (Debugger.TestROMList.ContainsKey(CurrentCellKey))
                {
                    Debugger.TestROMList[CurrentCellKey][0] = Selection;
                }
            }
            List<string> loglistkeys = Debugger.TestROMList.Keys.ToList();
            TestROMDataGridView.Rows.Clear();
            for (int i = 0; i < loglistkeys.Count; i++)
            {
                TestROMDataGridView.Rows.Add();
                bool IsSelected = false;
                if (Debugger.TestROMList[loglistkeys[i]][0] == "TRUE") { IsSelected = true; }
                TestROMDataGridView.Rows[i].Cells[0].Value = IsSelected;
                TestROMDataGridView.Rows[i].Cells[1].Value = Debugger.TestROMList[loglistkeys[i]][(1)];
                TestROMDataGridView.Rows[i].Cells[2].Value = Debugger.TestROMList[loglistkeys[i]][(2)];
                if (Debugger.TestROMList[loglistkeys[i]][(3)] != null && Debugger.TestROMList[loglistkeys[i]][(3)].Length>0)
                {
                    TestROMDataGridView.Rows[i].Cells[3].Value = new Bitmap(Debugger.TestROMList[loglistkeys[i]][(3)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                    TestROMDataGridView.Rows[i].Height = 90;
                }
                if (Debugger.TestROMList[loglistkeys[i]][(4)] != null && Debugger.TestROMList[loglistkeys[i]][(4)].Length > 0)
                {
                    TestROMDataGridView.Rows[i].Cells[4].Value = new Bitmap(Debugger.TestROMList[loglistkeys[i]][(4)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                    TestROMDataGridView.Rows[i].Height = 90;
                }
                if (Debugger.TestROMList[loglistkeys[i]][(5)] != null && Debugger.TestROMList[loglistkeys[i]][(5)].Length > 0)
                {
                    TestROMDataGridView.Rows[i].Cells[5].Value = new Bitmap(Debugger.TestROMList[loglistkeys[i]][(5)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                    TestROMDataGridView.Rows[i].Height = 90;
                }
                TestROMDataGridView.Rows[i].Cells[6].Value = Debugger.TestROMList[loglistkeys[i]][(6)];
                TestROMDataGridView.Rows[i].Cells[7].Value = Debugger.TestROMList[loglistkeys[i]][(7)];
                TestROMDataGridView.Rows[i].Cells[8].Value = Debugger.TestROMList[loglistkeys[i]][(9)];
                bool IsTest = false;
                if (Debugger.TestROMList[loglistkeys[i]][10] == "TRUE") { IsTest = true; }
                TestROMDataGridView.Rows[i].Cells[9].Value = IsTest;
            }
            Debugger.PrintTestROMList();            
        }

        private void ScanFolderLogsTestROMButton_Click(object sender, EventArgs e)
        {
            using (var ROMFolderDialog = new FolderBrowserDialog())
            {
                DialogResult SelectedFolder = ROMFolderDialog.ShowDialog();
                if (SelectedFolder == DialogResult.OK && !string.IsNullOrWhiteSpace(ROMFolderDialog.SelectedPath))
                {
                    List<string> Nesfiles = Directory.GetFiles(ROMFolderDialog.SelectedPath, "*.nes").ToList();
                    List<string> CurrentSelectedLogList = GetSelectedLogs();
                    for (int i = 0; i < CurrentSelectedLogList.Count; i++)
                    {
                        for (int d = 0; d < Nesfiles.Count; d++)
                        {
                            if(CurrentSelectedLogList[i]== System.IO.Path.GetFileNameWithoutExtension(Nesfiles[d]))
                            {
                                Debugger.AddRomToLog(CurrentSelectedLogList[i], Nesfiles[d]);
                            }
                        }
                    }
                    DisplayLogList();
                }
            }
        }

        public void LoadTestROMList()
        {
            if (File.Exists("TestROMList.txt"))
            {
                BackgroundWorker LoadTestROMWorker = new BackgroundWorker();
                LoadTestROMWorker.DoWork += LoadTestROMListBackGroundWorker_DoWork;               
                LoadTestROMWorker.WorkerReportsProgress = true;
                LoadTestROMWorker.RunWorkerAsync();
            }
        }
      
        private void SelectAllLogsButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LogViewDataGridView.Rows.Count; i++)
            {
                LogViewDataGridView.Rows[i].Cells[0].Value = "TRUE";               
            }
            DisplayLogList();
        }

        private void UnselectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LogViewDataGridView.Rows.Count; i++)
            {
                LogViewDataGridView.Rows[i].Cells[0].Value = "FALSE";
            }
            DisplayLogList();
        }

        private void LogViewDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Debugger.AddRomToLog(LogViewDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString(), dialog.FileName);
                    DisplayLogList();
                }
            }
            if (e.ColumnIndex == 5 && e.RowIndex != -1 && LogViewDataGridView.Rows[e.RowIndex].Cells[5].Value != null)
            {

                OutPutDataGridView.Visible = true;
                LogDataGridView.Visible = true;
                BackToLogButton.Visible = true;
                LogViewDataGridView.Visible = false;
                OutPutDataGridView.Rows.Clear();
                LogDataGridView.Rows.Clear();
                for (int i = 0; i < LogViewDataGridView.Rows.Count; i++)
                {
                    if (i != e.RowIndex)
                    {
                        LogViewDataGridView.Rows[i].Cells[0].Value = "FALSE";
                    }
                    else
                    {
                        LogViewDataGridView.Rows[i].Cells[0].Value = "TRUE";
                    }
                }
                DisplayLogList();
                BackgroundWorker CompareLogsWorker = new BackgroundWorker();
                CompareLogsWorker.DoWork += CompareLogsBackgroundWorker_DoWork;
                CompareLogsWorker.ProgressChanged += CompareLogsBackGroundWorker_ProgressChanged;
                CompareLogsWorker.WorkerReportsProgress = true;
                CompareLogsWorker.RunWorkerAsync();
                ShowCompareLogs = true;

            }
        }

        public List<string> GetSelectedLogs()
        {
            List<string> SelectedLogsList = new List<string>();
            for (int i = 0; i < LogViewDataGridView.Rows.Count; i++)
            {
                Debugger.LogList[LogViewDataGridView.Rows[i].Cells[1].Value.ToString()][0] = LogViewDataGridView.Rows[i].Cells[0].Value.ToString().ToUpper();
                if (LogViewDataGridView.Rows[i].Cells[0].Value.ToString().ToUpper() == "TRUE")
                {
                    SelectedLogsList.Add(LogViewDataGridView.Rows[i].Cells[1].Value.ToString());
                }
            }
            return SelectedLogsList;
        }
        public List<string> GetSelectedTestROMS()
        {
            List<string> SelectedROMList = new List<string>();
            for (int i = 0; i < TestROMDataGridView.Rows.Count; i++)
            {
                Debugger.TestROMList[TestROMDataGridView.Rows[i].Cells[1].Value.ToString()][0] = TestROMDataGridView.Rows[i].Cells[0].Value.ToString().ToUpper();
                if (TestROMDataGridView.Rows[i].Cells[0].Value.ToString().ToUpper() == "TRUE")
                {
                    SelectedROMList.Add(TestROMDataGridView.Rows[i].Cells[1].Value.ToString());
                }
            }
            return SelectedROMList;
        }
        public List<string> GetSelectedTestROMS2()
        {
            List<string> SelectedTestROMList = new List<string>();
            for (int i = 0; i < TestROMDataGridView.Rows.Count; i++)
            {
                Debugger.TestROMList[TestROMDataGridView.Rows[i].Cells[1].Value.ToString()][0] = TestROMDataGridView.Rows[i].Cells[0].Value.ToString().ToUpper();
                if (TestROMDataGridView.Rows[i].Cells[0].Value.ToString().ToUpper() == "TRUE")
                {
                    SelectedTestROMList.Add(i.ToString());
                }
            }
            return SelectedTestROMList;
        }
        private void RemoveSelectedLogsButton_Click(object sender, EventArgs e)
        {
            List<string> SelectedLogList = GetSelectedLogs();
            for (int i = 0; i < SelectedLogList.Count; i++)
            {
                Debugger.LogList.Remove(SelectedLogList[i]);
            }
            DisplayLogList();
        }

        private void ScanFolderForLogsbutton_Click(object sender, EventArgs e)
        {
            using (var LogFolderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult LogFolderResult = LogFolderBrowserDialog.ShowDialog();
                if (LogFolderResult == DialogResult.OK && !string.IsNullOrWhiteSpace(LogFolderBrowserDialog.SelectedPath))
                {
                    string[] LogFiles = Directory.GetFiles(LogFolderBrowserDialog.SelectedPath, "*.debug");
                    for(int i = 0; i < LogFiles.Length; i++)
                    {
                        Debugger.AddNewLog(LogFiles[i]);
                    }
                    DisplayLogList();
                }
            }
        }
        
        private void CompareLogsBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {            
            List<string> selectlist = GetSelectedLogs();
            for (int i = 0; i < selectlist.Count; i++)
            {
                StopOnComparison = true;
                if (File.Exists(Debugger.LogList[selectlist[i]][8]) && File.Exists(Debugger.LogList[selectlist[i]][2]))
                {
                    int Counter = 0;
                    NES.Reset();
                    Debugger.TracePointer = 0;
                    GAME.LoadGame(Debugger.LogList[selectlist[i]][2]);
                    this.Invoke(new MethodInvoker(delegate {
                        LogGameNameLabel.Text = (i + 1).ToString() + "/" + selectlist.Count+" "+ GAME.GAMENAME;
                    }));
                    int PercentageComplete = 0;
                    UpdateDebugger();
                    using (StreamReader LogStreamReader = File.OpenText(Debugger.LogList[selectlist[i]][8]))
                    {
                        int stopafter1 = -1;
                        while (StopOnComparison && LogTraceList.Count<300000)
                        {
                            if (NES.Trace && (CPU.CycleNumber == 0)) {
                                string CurrentLogLine = string.Empty;
                                CurrentLogLine = LogStreamReader.ReadLine();
                                if (CurrentLogLine != null)
                                {
                                    string[] CurrentlogLineArray = new string[11];
                                    string[] LogLineSplitArray = CurrentLogLine.Split(' ');
                                    string LogLineRegisters = (Counter + " " + CurrentLogLine);
                                    int LogOffSet = LogLineRegisters.IndexOf("A:");
                                    CurrentlogLineArray[0] = (Counter + 1).ToString().Trim();
                                    CurrentlogLineArray[1] = LogLineSplitArray[0].Trim();
                                    CurrentlogLineArray[2] = CurrentLogLine.Substring(5, 10).Trim();
                                    CurrentlogLineArray[3] = CurrentLogLine.Substring(16, 30).Trim();
                                    CurrentlogLineArray[4] = LogLineRegisters.Substring(LogOffSet, 4).Trim();
                                    CurrentlogLineArray[5] = LogLineRegisters.Substring(LogOffSet + 5, 4).Trim();
                                    CurrentlogLineArray[6] = LogLineRegisters.Substring(LogOffSet + 10, 4).Trim();
                                    CurrentlogLineArray[7] = LogLineRegisters.Substring(LogOffSet + 15, 4).Trim();
                                    CurrentlogLineArray[8] = LogLineRegisters.Substring(LogOffSet + 20, 5).Trim();
                                    CurrentlogLineArray[9] = LogLineRegisters.Substring(LogOffSet + 26, 7).Trim();
                                    CurrentlogLineArray[10] = LogLineRegisters.Substring(LogOffSet + 34).Trim();
                                    LogTraceList.Add(CurrentlogLineArray);
                                    Counter++;
                                    if (Counter % 3000 == 0) { PercentageComplete = Counter / 3000; CompareLogsBackgroundWorker.ReportProgress(PercentageComplete); }
                                    if (Debugger.TracePointer == 1000) { Debugger.TracePointer = 0; }

                                    StringBuilder ProccessorStatusStringBuilder = new StringBuilder();
                                    int[] ProccessorStatusArray = CPU.ProcessorStatus.Reverse().ToArray();
                                    foreach (int a in ProccessorStatusArray) { ProccessorStatusStringBuilder.Append(a); }

                                    string[] DecodedTraceDataArray = Debugger.DecodeOpcode(CPU.ProgramCounter);
                                    Debugger.INSTRUCTIONCOUNT++;
                                    string[] CurrentTraceLine = new string[11];
                                    CurrentTraceLine[0] = (Debugger.INSTRUCTIONCOUNT).ToString().Trim();
                                    CurrentTraceLine[1] = CPU.ProgramCounter.ToString("X4");
                                    CurrentTraceLine[2] = DecodedTraceDataArray[3].Trim();
                                    CurrentTraceLine[3] = "" + DecodedTraceDataArray[0].Trim();
                                    CurrentTraceLine[4] = "A:" + CPU.Accumulator.ToString("X2");
                                    CurrentTraceLine[5] = "X:" + CPU.IndexRegisterX.ToString("X2");
                                    CurrentTraceLine[6] = "Y:" + CPU.IndexRegisterY.ToString("X2");
                                    CurrentTraceLine[7] = "P:" + Convert.ToInt32(ProccessorStatusStringBuilder.ToString(), 2).ToString("X2");
                                    CurrentTraceLine[8] = "SP:" + CPU.StackPointer.ToString("X2");
                                    CurrentTraceLine[9] = "CYC:" + PPU.PPUCycleCount.ToString().PadLeft(3);
                                    CurrentTraceLine[10] = "SL:" + PPU.Scanlines;
                                    Debugger.TraceList[Debugger.TracePointer] = CurrentTraceLine;
                                    Debugger.TracePointer++;
                                    if (StopOnComparison)
                                    {                                        
                                        for (int i2 = 1; i2 < 11; i2++)
                                        {
                                            if (! (Debugger.INSTRUCTIONCOUNT < 1000070 && i2 == 8))                                            
                                            {
                                                if (i2 != 3)
                                                {
                                                    if (CurrentlogLineArray[i2].Trim() != CurrentTraceLine[i2].Trim() && stopafter1 < 0)
                                                    {
                                                        stopafter1 = 1;
                                                    }
                                                }
                                            }
                                        }
                                        if (stopafter1 > 0)
                                        {
                                            stopafter1--;
                                        }else if(stopafter1 == 0)
                                        {
                                            stopafter1 = -1;
                                            StopOnComparison = false;
                                        }
                                    }
                                }
                            }
                            CPU.RunCPU();
                            APU.RunAPU(1);
                            PPU.RunPPU(1);
                        }
                        CompareLogsBackgroundWorker.ReportProgress(0);
                        Debugger.LogList[selectlist[i]][4] = Debugger.LogList[selectlist[i]][5];
                        Debugger.LogList[selectlist[i]][5] = LogTraceList.Count.ToString();
                        Debugger.LogList[selectlist[i]][6] = LogTraceList[LogTraceList.Count - 1][3];
                        if (LogTraceList.Count > 2)
                        {
                            List<string[]> TraceOutput = new List<string[]>();
                            List<string[]> LogOutput = new List<string[]>();
                            for(int k = 3; k > 0; k--)
                            {
                                TraceOutput.Add(Debugger.TraceList[Debugger.TracePointer - k]);
                                LogOutput.Add(LogTraceList[LogTraceList.Count - k]);                                
                            }
                            Debugger.LogList[selectlist[i]][7] = Debugger.AnalyzeLogComparison(TraceOutput, LogOutput);
                        }
                        this.Invoke(new MethodInvoker(delegate {
                            DisplayLogList();
                            if (ShowCompareLogs)
                            {
                                int NumberOfRows = LogTraceList.Count();
                                if (NumberOfRows > 1000) { NumberOfRows = 1000; }
                                for (int d = Debugger.TracePointer; d < 1000; d++)
                                {
                                    if (Debugger.TraceList[d][0] != null)
                                    {
                                        OutPutDataGridView.Rows.Add(Debugger.TraceList[d]);
                                    }                                  
                                }
                                for (int b = 0; b < Debugger.TracePointer; b++)
                                {                                
                                    OutPutDataGridView.Rows.Add(Debugger.TraceList[b]);                                
                                }
                                for(int t = NumberOfRows; t >0; t--)
                                {
                                    LogDataGridView.Rows.Add(LogTraceList[(LogTraceList.Count-t)]);
                                }
                                OutPutDataGridView.Rows[OutPutDataGridView.Rows.Count - 2].Selected = true;
                                OutPutDataGridView.CurrentCell = OutPutDataGridView[0, OutPutDataGridView.Rows.Count - 2];
                                OutPutDataGridView.FirstDisplayedScrollingRowIndex = OutPutDataGridView.Rows.Count - 2;
                                LogDataGridView.Rows[LogDataGridView.Rows.Count - 2].Selected = true;
                                LogDataGridView.CurrentCell = LogDataGridView[0, LogDataGridView.Rows.Count - 2];
                                LogDataGridView.FirstDisplayedScrollingRowIndex = LogDataGridView.Rows.Count - 2;
                            }
                        }));
                        
                    }
                }
            }
        }

        private void CompareSelectedLogsButton_Click(object sender, EventArgs e)
        {
            NES.PlayLoop = true;
            BackgroundWorker CompareLogsWorker = new BackgroundWorker();
            CompareLogsWorker.DoWork += CompareLogsBackgroundWorker_DoWork;
            CompareLogsBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(CompareLogsBackGroundWorker_ProgressChanged);
            CompareLogsWorker.ProgressChanged += CompareLogsBackGroundWorker_ProgressChanged;
            CompareLogsWorker.WorkerReportsProgress = true;
            CompareLogsWorker.RunWorkerAsync();
            ShowCompareLogs = false;
        }

        private void BackToLogViewButton_Click(object sender, EventArgs e)
        {
            OutPutDataGridView.Visible = false;
            LogDataGridView.Visible = false;
            LogViewDataGridView.Visible = true;
            BackToLogButton.Visible = false;
        }

        private void CompareLogsBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate {
                LogsProgressBar.Value = e.ProgressPercentage;
            }));
        }

        private void HexConverterButton_Click(object sender, EventArgs e)
        {
            string val = HexConverterTextBox.Text.ToUpper().Trim();
            
            if (val.Length > 0)
            {
                if (HexRadioButton.Checked && System.Text.RegularExpressions.Regex.IsMatch(val, @"\A\b[0-9a-fA-F]+\b\Z"))
                {
                    DecimalTextBox.Text = Convert.ToInt32(val, 16).ToString();
                    BinaryTextBox.Text = Convert.ToString(Convert.ToInt32(val, 16), 2).PadLeft(8, '0');
                    HexTextBox.Text = val;
                }
                else if(int.TryParse(val, out int n))
                {
                    HexTextBox.Text = Convert.ToInt32(val).ToString("X2");
                    BinaryTextBox.Text=  Convert.ToString(Convert.ToInt32(val), 2).PadLeft(8, '0');
                    DecimalTextBox.Text = val;
                }
            }
        }

        private void AddNewTestROMButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "NES files (*.nes)|*.NES";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result.ToString() != "Cancel")
            {
                Debugger.AddNewTestROM(openFileDialog1.FileName);
            }
            DisplayTestROMList();
        }

        private void ScanFolderForTestROMButton_Click(object sender, EventArgs e)
        {
            using (var TestROMFolderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult SelectedTestROMFolder = TestROMFolderBrowserDialog.ShowDialog();
                if (SelectedTestROMFolder == DialogResult.OK && !string.IsNullOrWhiteSpace(TestROMFolderBrowserDialog.SelectedPath))
                {
                    string[] NESFilesFound = Directory.GetFiles(TestROMFolderBrowserDialog.SelectedPath, "*.nes");
                    for (int i = 0; i < NESFilesFound.Length; i++)
                    {
                        Debugger.AddNewTestROM(NESFilesFound[i]);
                    }
                    DisplayTestROMList();
                }
            }
        }

        private void TestROMSelectionButton_Click(object sender, EventArgs e)
        {
            string SelectedText = SelectComboBox.SelectedItem.ToString().Trim();
            for (int i = 0; i < TestROMDataGridView.Rows.Count; i++)
            {
                if (SelectedText == "Select All" || SelectedText == TestROMDataGridView.Rows[i].Cells[2].Value.ToString() ||(SelectedText == "Test ROMS" && TestROMDataGridView.Rows[i].Cells[9].Value.ToString().ToUpper() == "TRUE") )
                {
                    TestROMDataGridView.Rows[i].Cells[0].Value = "TRUE";
                }
            }
        }

        private void UnselectAllTestROMButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < TestROMDataGridView.Rows.Count; i++)
            {
                TestROMDataGridView.Rows[i].Cells[0].Value = "FALSE";
            }
        }

        private void RemoveSelectedTestROMSButton_Click(object sender, EventArgs e)
        {
            List<string> SelectedTestROMList = GetSelectedTestROMS();
            for (int i = 0; i < SelectedTestROMList.Count; i++)
            {
                Debugger.TestROMList.Remove(SelectedTestROMList[i]);
            }
            DisplayTestROMList();
        }

        private void RunSelectedTestROMTestButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker TraceDataWorker = new BackgroundWorker();
            TraceDataWorker.DoWork += LoadTraceDatabackGroundWorker_DoWork;
            TraceDataWorker.WorkerReportsProgress = true;
            TraceDataWorker.RunWorkerAsync();
           
        }
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        public static bool CompareMemCmp(Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) != (b2 == null)) return false;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }

        private void TestROMDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex!=-1)
            {
                string ROMName= TestROMDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                string imagenameold1 = Debugger.TestROMList[ROMName][4];
                string imagenameold = Debugger.TestROMList[ROMName][4].Replace("-OLD.bmp", "-PASS.bmp");
                Debugger.TestROMList[ROMName][3] = Path.Combine(Environment.CurrentDirectory, @"Test\", imagenameold);
                System.IO.File.Copy(Path.Combine(Environment.CurrentDirectory, @"Test\", imagenameold1), Debugger.TestROMList[ROMName][3], true);
                TestROMDataGridView.Rows[e.RowIndex].Cells[3].Value = new Bitmap(Debugger.TestROMList[ROMName][(3)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                TestROMDataGridView.Rows[e.RowIndex].Height = 90;
            }
            if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                string ROMName = TestROMDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                string imagenameold1 = Debugger.TestROMList[ROMName][5];
                string imagename = Debugger.TestROMList[ROMName][5].Replace("-CUR.bmp", "-PASS.bmp");
                Debugger.TestROMList[ROMName][3] = Path.Combine(Environment.CurrentDirectory, @"Test\", imagename);
                System.IO.File.Copy(Path.Combine(Environment.CurrentDirectory, @"Test\", imagenameold1), Debugger.TestROMList[ROMName][3], true);
                TestROMDataGridView.Rows[e.RowIndex].Cells[3].Value = new Bitmap(Debugger.TestROMList[ROMName][(3)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                TestROMDataGridView.Rows[e.RowIndex].Height = 90;
                Debugger.PrintTestROMList();
             }
            if (e.ColumnIndex == 8 && e.RowIndex != -1)
            {
                TestROMPopUpInstr.InstrCnt= TestROMDataGridView.Rows[e.RowIndex].Cells[8].Value.ToString();
                TestROMPopUpInstr.ROMName = TestROMDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                TestROMPopUpInstr tr = new TestROMPopUpInstr();
                tr.ShowDialog();
            }
         }

       
        private void PRGRAMRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDebugger();
        }

        private void TestROMDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 9 && e.RowIndex != -1)
            {
                TestROMDataGridView.EndEdit();
            }
        }

        private void TestROMDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9 && e.RowIndex != -1)
            {
                string ROMName = TestROMDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                Debugger.TestROMList[ROMName][10] = TestROMDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString().ToUpper();
            }
        }
         
        private void ResetButton_Click(object sender, EventArgs e)
        {
            CPU.CurrentOpCode = 0x104;
            CPU.CycleNumber = 1;
        }

        private void LoadTestROMListBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> TestROMFileList = File.ReadAllLines("TestROMList.txt").ToList();
            Debugger.TestROMList = new Dictionary<string, string[]>();
            SelectComboBox.Items.Add("Select All");
            SelectComboBox.Items.Add("Test ROMS");
            Dictionary<string, string> INESDictionary = new Dictionary<string, string>();
            for (int i = 0; i < TestROMFileList.Count; i++)
            {
                TextFieldParser TestROMParser = new TextFieldParser(new StringReader(TestROMFileList[i]));
                TestROMParser.HasFieldsEnclosedInQuotes = true;
                TestROMParser.SetDelimiters(",");
                string[] NewTestROMLine;
                while (!TestROMParser.EndOfData)
                {
                    NewTestROMLine = TestROMParser.ReadFields();
                    if (NewTestROMLine[3] == null || NewTestROMLine[3].Length < 1)
                    {
                        int NumberInstructionsToExecute = Convert.ToInt32(NewTestROMLine[9]);
                        NumberInstructionsToExecute += 100000;
                        NewTestROMLine[9] = NumberInstructionsToExecute.ToString();
                    }
                    if (!INESDictionary.ContainsKey(NewTestROMLine[2]))
                    {
                        INESDictionary.Add(NewTestROMLine[2], NewTestROMLine[2]);
                        this.Invoke(new MethodInvoker(delegate {
                            SelectComboBox.Items.Add(NewTestROMLine[2]);
                        }));
                    }
                    Debugger.TestROMList.Add(NewTestROMLine[1], NewTestROMLine);
                }
                TestROMParser.Close();
            }
            for (int f = 0; f < TestROMDataGridView.Rows.Count; f++)
            {
                string CurrentCellKey = TestROMDataGridView.Rows[f].Cells[1].Value.ToString();
                string Selection = TestROMDataGridView.Rows[f].Cells[0].Value.ToString().ToUpper();
                if (Debugger.TestROMList.ContainsKey(CurrentCellKey))
                {
                    Debugger.TestROMList[CurrentCellKey][0] = Selection;
                }
            }
            List<string> loglistkeys = Debugger.TestROMList.Keys.ToList();
            this.Invoke(new MethodInvoker(delegate {
                SelectComboBox.SelectedIndex = 0;
                TestROMDataGridView.Rows.Clear();
            }));
            for (int i = 0; i < loglistkeys.Count; i++)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TestROMDataGridView.Rows.Add();
                    bool IsSelected = false;
                    if (Debugger.TestROMList[loglistkeys[i]][0] == "TRUE") { IsSelected = true; }
                    TestROMDataGridView.Rows[i].Cells[0].Value = IsSelected;
                    TestROMDataGridView.Rows[i].Cells[1].Value = Debugger.TestROMList[loglistkeys[i]][(1)];
                    TestROMDataGridView.Rows[i].Cells[2].Value = Debugger.TestROMList[loglistkeys[i]][(2)];
                    if (Debugger.TestROMList[loglistkeys[i]][(3)] != null && Debugger.TestROMList[loglistkeys[i]][(3)].Length > 0)
                    {
                        TestROMDataGridView.Rows[i].Cells[3].Value = new Bitmap(Debugger.TestROMList[loglistkeys[i]][(3)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                        TestROMDataGridView.Rows[i].Height = 90;
                    }
                    if (Debugger.TestROMList[loglistkeys[i]][(4)] != null && Debugger.TestROMList[loglistkeys[i]][(4)].Length > 0)
                    {
                        TestROMDataGridView.Rows[i].Cells[4].Value = new Bitmap(Debugger.TestROMList[loglistkeys[i]][(4)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                        TestROMDataGridView.Rows[i].Height = 90;
                    }
                    if (Debugger.TestROMList[loglistkeys[i]][(5)] != null && Debugger.TestROMList[loglistkeys[i]][(5)].Length > 0)
                    {
                        TestROMDataGridView.Rows[i].Cells[5].Value = new Bitmap(Debugger.TestROMList[loglistkeys[i]][(5)]).GetThumbnailImage(96, 90, null, IntPtr.Zero);
                        TestROMDataGridView.Rows[i].Height = 90;
                    }
                    TestROMDataGridView.Rows[i].Cells[6].Value = Debugger.TestROMList[loglistkeys[i]][(6)];
                    TestROMDataGridView.Rows[i].Cells[7].Value = Debugger.TestROMList[loglistkeys[i]][(7)];
                    TestROMDataGridView.Rows[i].Cells[8].Value = Debugger.TestROMList[loglistkeys[i]][(9)];
                    TestROMDataGridView.Rows[i].Cells[9].Value = false;
                    if (Debugger.TestROMList[loglistkeys[i]][10] == "TRUE") { TestROMDataGridView.Rows[i].Cells[9].Value = true; }
                }));
            }
        }

        private void TraceLog_Shown(object sender, EventArgs e)
        {
            LoadTestROMList();
        }
        
        private void PPUViewerTimer_Tick(object sender, EventArgs e)
        {
            PallettePictureBox.Image = Debugger.DrawNESPallette();
            SpritePictureBox.Image = Debugger.DrawNESSprites();
            PatternTablePictureBox.Image = Debugger.DrawPatternTables();
            int StartIndex = 0x2000;
            if (radioButton2400.Checked) { StartIndex = 0x2400; }
            else if (radioButton2800.Checked) { StartIndex = 0x2800; }
            else if (radioButton2C00.Checked) { StartIndex = 0x2C00; }
            NameTablePictureBox.Image = Debugger.DrawNameTable(StartIndex);
        }

        private void TraceTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TraceTabControl.SelectedTab == TraceTabControl.TabPages["NameTableTabPage"])
            {
                PPUViewerTimer.Start();
            }
            else
            {
                PPUViewerTimer.Stop();
            }
        }

        private void RegisterTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDebugger();
        }
    }
    class SyncTextBox : RichTextBox
    {
        public SyncTextBox()
        {
            this.Multiline = true;
            this.ScrollBars = ScrollBars;
        }
        public Control Buddy { get; set; }

        private static bool scrolling;   // In case buddy tries to scroll us
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // Trap WM_VSCROLL message and pass to buddy
            if ((m.Msg == 0x115 || m.Msg == 0x20a ) && !scrolling && Buddy != null && Buddy.IsHandleCreated)
            {
                scrolling = true;
                SendMessage(Buddy.Handle, m.Msg, m.WParam, m.LParam);
                scrolling = false;
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
