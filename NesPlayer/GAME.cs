using System;
using System.IO;

namespace NesPlayer
{
    class GAME
    {
        private static int numprgpages;
        public static int NUMPRGPAGES
        {  
            get { return numprgpages; }
            set { numprgpages = value; }
        }

        private static int numchrpages;
        public static int NUMCHRPAGES
        {
            get { return numchrpages; }
            set { numchrpages = value; }
        }

        private static string gamelocation;
        public static string GAMELOCATION
        {
            get { return gamelocation; }
            set { gamelocation = value; }
        }

        private static string gamename;
        public static string GAMENAME
        {
            get { return gamename; }
            set { gamename = value; }
        }

        private static string headerinformation;
        public static string HEADERINFORMATION
        {
            get { return headerinformation; }
            set { headerinformation = value; }
        }

        private static int mappernumber;
        public static int MAPPERNUMBER
        {
            get { return mappernumber; }
            set { mappernumber = value; }
        }

        private static int _Mirroring;
        public static int Mirroring
        {
            get { return _Mirroring; }
            set { _Mirroring = value; }
        }

        private static bool _IgnoreMirroring;
        public static bool IgnoreMirroring
        {
            get { return _IgnoreMirroring; }
            set { _IgnoreMirroring = value; }
        }

        private static bool _Battery;
        public static bool Battery
        {
            get { return _Battery; }
            set { _Battery = value; }
        }
        private static byte[] rombytedata;
        public static byte[] ROMBYTEDATA
        {
            get { return rombytedata; }
            set { rombytedata = value; }
        }
        private static int[] _PRGBankData = new int[8];
        public static int[] PRGBankData
        {
            get { return _PRGBankData; }
            set { _PRGBankData = value; }
        }
        private static int[] _CHRBankData = new int[8];
        public static int[] CHRBankData
        {
            get { return _CHRBankData; }
            set { _CHRBankData = value; }
        }
        public static int LowCHRRAMBank = 0;
        public static int HighCHRRAMBank = 1;
        public static int LowPRGRAMBank = 0;
        public static int HighPRGRAMBank = 1;
        private static int srampresent;
        public static int SRAMPRSENT
        {
            get { return srampresent; }
            set { srampresent = value; }
        }
        private static int[] _PRGRAM = new int[0x10000];
        public static int[] PRGRAM
        {
            get { return _PRGRAM; }
            set { _PRGRAM = value; }
        }

        private static int[] _CHRRAM = new int[0x4000];
        public static int[] CHRRAM
        {
            get { return _CHRRAM; }
            set { _CHRRAM = value; }
        }
        private static int[] _MMC5Sprite16 = new int[0x2000];
        public static int[] MMC5Sprite16
        {
            get { return _MMC5Sprite16; }
            set { _MMC5Sprite16 = value; }
        }
        private static GAME _TheMapper;
        public static GAME TheMapper
        {
            get { return _TheMapper; }
            set { _TheMapper = value; }
        }
       
        public static void LoadGame(string fileloc)
        {
            gamename = System.IO.Path.GetFileNameWithoutExtension(fileloc);
            string pathgame = System.IO.Path.GetFileName(fileloc);
            string srampath = fileloc.Replace(pathgame, "") + gamename+".sav";
            gamelocation = fileloc;
            ROMBYTEDATA = File.ReadAllBytes(fileloc);
            NUMPRGPAGES = ROMBYTEDATA[4];
            NUMCHRPAGES = ROMBYTEDATA[5];
            PPU.VERTICAL = ROMBYTEDATA[6] & 1;
            Mirroring= ROMBYTEDATA[6] & 1;
            if(((ROMBYTEDATA[6] >> 3) & 1) == 1) { IgnoreMirroring = true; } else { IgnoreMirroring = false; }
            SRAMPRSENT = (ROMBYTEDATA[6]>>1) & 1;
            if (((ROMBYTEDATA[6] >> 1) & 1) == 1) { Battery = true; } else { Battery = false; }
            if (SRAMPRSENT == 1)
            {
               if(!File.Exists(srampath))
                {
                    byte[] info = new byte[0x2000];
                    for (int i = 0; i < 0x2000; i++)
                    {
                        info[i] = 0;
                    }
                    using (var stream = new FileStream(srampath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (var writer = new BinaryWriter(stream))
                    {
                        foreach (byte item in info)
                        {
                            writer.Write(item);
                        }
                    }
                    
                }
                else
                {
                    string srampth = GAME.GAMELOCATION.Replace(".nes", ".sav");
                    byte[] sram = System.IO.File.ReadAllBytes(srampth);
                    for(int i = 0; i < 0x2000; i++)
                    {
                        CPU.CPUMemory[0x6000 + i] = sram[i];
                    }
                }
            }
            MAPPERNUMBER = ((ROMBYTEDATA[7] >> 4) << 4) + (ROMBYTEDATA[6] >> 4);            
            string orient = "";
            PPU.LoopyT |= 0 << 7;
            if (PPU.VERTICAL == 1) { orient = "Vertical"; } else { orient = "Horizontal"; }
            headerinformation = gamename + "  Mapper Number:" + MAPPERNUMBER + "  #PRG:" + NUMPRGPAGES + "  #CHR:" + NUMCHRPAGES + "  SRAM:" + SRAMPRSENT + "  Mirroring: " + orient;
            if (MAPPERNUMBER == 0) { TheMapper = new INES0(); }
            if (MAPPERNUMBER == 1) { TheMapper = new INES1(); }
            if (MAPPERNUMBER == 2) { TheMapper = new INES2(); }
            if (MAPPERNUMBER == 3) { TheMapper = new INES3(); }
            if (MAPPERNUMBER == 4) { TheMapper = new INES4(); }
            if (MAPPERNUMBER == 5) { TheMapper = new INES5(); }
            if (MAPPERNUMBER == 7) { TheMapper = new INES7(); }
            if (MAPPERNUMBER == 9) { TheMapper = new INES9(); }
            if (MAPPERNUMBER == 11) { TheMapper = new INES11(); }
            if (MAPPERNUMBER == 13) { TheMapper = new INES13(); }
            if (MAPPERNUMBER == 34) { TheMapper = new INES34(); }
            if (MAPPERNUMBER == 66) { TheMapper = new INES66(); }
            if (MAPPERNUMBER == 71) { TheMapper = new INES71(); }
            if (MAPPERNUMBER == 232) { TheMapper = new INES232(); }
            TheMapper.LoadRom();
            if (GAME.NUMCHRPAGES > 0)
            {
                TheMapper.loadvram();
            }
            CPU.ProgramCounter = CPU.CPUMemory[0xFFFD] << 8 | CPU.CPUMemory[0xFFFC];
        }

        public virtual int GetMirroringType()
        {
            return PPU.VERTICAL;
        }

        public virtual void SetTileLatch(int MemoryAddress)
        {
        }
        public virtual void WriteCHRRAM(int MemoryAddress,int val)
        {
            int tempad = MemoryAddress & 0xFFF;
            if (MemoryAddress < 0x1000)
            {
                GAME.CHRRAM[(LowCHRRAMBank * 0x1000) +tempad] = val;
                if (LowCHRRAMBank == HighCHRRAMBank)
                {
                    PPU.PPUMemory[MemoryAddress+0x1000] = val;
                }
            }
            else
            {
                GAME.CHRRAM[(HighCHRRAMBank * 0x1000) + tempad] = val;
                if (LowCHRRAMBank == HighCHRRAMBank)
                {
                    PPU.PPUMemory[MemoryAddress - 0x1000] = val;
                }
            }
        }
        public virtual void WritePRGRAM(int MemoryAddress, int val)
        {
            int tempad = MemoryAddress & 0xFFF;
            if (MemoryAddress < 0x7000)
            {
                GAME.PRGRAM[(LowPRGRAMBank * 0x1000) + tempad ] = val;
                if (LowPRGRAMBank == HighPRGRAMBank)
                {
                    CPU.CPUMemory[MemoryAddress + 0x1000] = val;
                }
            }
            else if (MemoryAddress < 0x8000)
            {
                GAME.PRGRAM[(HighPRGRAMBank * 0x1000) + tempad ] = val;
                if (LowPRGRAMBank == HighPRGRAMBank)
                {
                    CPU.CPUMemory[MemoryAddress - 0x1000] = val;
                }
            }
        }
        public static void SwitchPRGRAMBank(int BankNumber, int PRGSize, int BankSize, int StartAddress)
        {
            if (StartAddress == 0x6000)
            {
                LowPRGRAMBank = BankNumber;
                if (BankSize == 0x2000) {HighPRGRAMBank = BankNumber + 1;}              
            }else if (StartAddress == 0x7000) { HighPRGRAMBank = BankNumber; }
      
            int ROMAddress = (BankNumber * (PRGSize));
            for (int y = StartAddress; y < (StartAddress + BankSize); y++)
            {
                CPU.CPUMemory[y] = GAME.PRGRAM[ROMAddress];
                ROMAddress++;
            }
        }

        public static void SwitchPRGBank(int BankNumber,int PRGSize, int BankSize, int StartAddress,int OffSet)
        {
            int CurrentBank = (BankNumber & ((GAME.NUMPRGPAGES*(0x4000/PRGSize)) - 1));
            if (GAME.NUMPRGPAGES < 17) { OffSet = 0; }
            int off = (StartAddress - 0x8000) / 0x1000;
            for (int i = 0; i < (BankSize / 0x1000); i++)
            {
                PRGBankData[i+off] = (((CurrentBank * (PRGSize)) + OffSet + (i * 0x1000)) / 0x1000);
            }
            int ROMAddress = 16 + (CurrentBank * (PRGSize))+OffSet;
            for (int y = StartAddress; y < (StartAddress + BankSize); y++)
            {
                CPU.CPUMemory[y] = GAME.ROMBYTEDATA[ROMAddress];
                ROMAddress++;
            }
         }

        public static void SwitchCHRRAMBank(int BankNumber, int CHRSize, int BankSize, int StartAddress)
        {
            int CurrentBank = (BankNumber & ((GAME.NUMCHRPAGES * (0x2000 / CHRSize)) - 1));
            int ROMAddress = (CurrentBank * (CHRSize));
            if (StartAddress == 0)
            {
                LowCHRRAMBank = BankNumber;
            }
            else
            {
                HighCHRRAMBank = BankNumber;
            }
            for (int y = StartAddress; y < (StartAddress + BankSize); y++)
            {
                PPU.PPUMemory[y] = GAME.CHRRAM[ROMAddress];
                ROMAddress++;
            }
        }

        public static void SwitchCHRBank(int BankNumber, int CHRSize, int BankSize, int StartAddress)
        {
            int off = (StartAddress) / 0x0400;
            for (int i = 0; i < (BankSize / 0x0400); i++)
            {
                CHRBankData[i + off] = (((BankNumber * (CHRSize)) + (i * 0x0400)) / 0x0400);
            }
            BankNumber = BankNumber & ((GAME.NUMCHRPAGES * (0x2000/ CHRSize)) - 1);
            if (GAME.NUMCHRPAGES > 0)
            {
                int ROMAddress = 16 + (BankNumber * (CHRSize)) + (GAME.NUMPRGPAGES * 0x4000);
                for (int y = StartAddress; y < (StartAddress + BankSize); y++)
                {
                    PPU.PPUMemory[y] = GAME.ROMBYTEDATA[ROMAddress];
                    ROMAddress++;
                }
            }
        }

        public static void SwitchCHRMMC516Bank(int BankNumber, int CHRSize, int BankSize, int StartAddress)
        {
            int off = (StartAddress) / 0x0400;
            for (int i = 0; i < (BankSize / 0x0400); i++)
            {
                CHRBankData[i + off] = (((BankNumber * (CHRSize)) + (i * 0x0400)) / 0x0400);
            }
            BankNumber = BankNumber & ((GAME.NUMCHRPAGES * (0x2000 / CHRSize)) - 1);
            if (GAME.NUMCHRPAGES > 0)
            {
                int ROMAddress = 16 + (BankNumber * (CHRSize)) + (GAME.NUMPRGPAGES * 0x4000);
                for (int y = StartAddress; y < (StartAddress + BankSize); y++)
                {
                    GAME.MMC5Sprite16[y] = GAME.ROMBYTEDATA[ROMAddress];
                    ROMAddress++;
                }
            }
        }

        public virtual void SetA12Latch(int dot, bool addrwrite)
        {
        }
        public virtual void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
        }

        public virtual int ReadFromAddress(int MemoryAddress)
        {
            return CPU.CPUMemory[MemoryAddress];
        }
        public void loadvram()
        {
            GAME.SwitchCHRBank(0, 0x1000, 0x2000, 0x0000);
            for (int y = (0x2000); y < (0x4000); y++)
            {
                PPU.PPUMemory[y] = 0;
            }
        }

        public virtual void LoadRom()
        {
            GAME.SwitchPRGBank(0, 0x4000, 0x4000, 0x8000, 0);
            GAME.SwitchPRGBank((GAME.NUMPRGPAGES - 1), 0x4000, 0x4000, 0xC000, 0);
        }
        public static void Reset()
        {
            NUMPRGPAGES = 0;
            NUMCHRPAGES = 0;
            HEADERINFORMATION = null;
            MAPPERNUMBER = 0;
            Mirroring = 0;
            IgnoreMirroring = false;
            Battery = false;
            LowPRGRAMBank = 0;
            HighPRGRAMBank = 1;
            LowCHRRAMBank = 0;
            HighCHRRAMBank = 1;
            PRGBankData = new int[8];
            CHRBankData = new int[8];
            SRAMPRSENT = 0;
            PRGRAM = new int[0x10000];
            CHRRAM = new int[0x4000];
            INES1.MMC1COUNT = 0;
            INES1.MMC1FIVEBITVALUE = 0;
            INES1.MMC1REG1 = 0;
            INES1.MMC1REG2 = 0;
            INES1.MMC1REG3 = 0;
            INES1.MMC1REG0 = 0x0f;
            INES1.LastWriteCycle = -4;

            INES9.Mapper9Latch0 = 0xFE;
            INES9.Mapper9Latch1 = 0xFE;

            INES4.MMC3IRQ = false;
            INES4.MMC3IRQCOUNTER = 0;
            INES4.MMC3A12 = 0;
            INES4.MMC3RELOADVALUE = -2;
            INES4.MMC3IRQENABLE = false;
            INES4.MMC3RELOADFLAG = false;
            INES4.MMC3E001h = 0;
            INES4.MMC3E000h = 0;
            INES4.MMC3C001h = 0;
            INES4.MMC3C000h = 0;
            INES4.MMC3A001h = 0;
            INES4.MMC3A000h = 0;
            INES4.MMC38001h = 0;
            INES4.MMC38000h = 0;
            INES4.prginvert = 0;
            INES4.FIRE12LATCH = false;
            INES4.TARGETSCANLINE = -2;
            INES4.IRQDELAY = 8;
            INES4.TRANSDOT = 0;
            INES4.TRANSSCAN = 0;

            INES5.UseMMC5Banks = false;
            INES5.MMC5Sprite16 = new int[0x4000];
            INES5.PRGMODE5100 = 3;
            INES5.CHRMODE5101 = 0;
            INES5.PRGRAMPROTECT5102 = 0;
            INES5.PRGRAMPROTECT5103 = 0;
            INES5.EXTRAM5104 = 0;
            INES5.NAMETABLEMAPPING5105 = 0;
            INES5.FILLEMODETILE5106 = 0;
            INES5.FILLEMODECOLOR5107 = 0;
            INES5.PRGRAMBANK5113 = 0;
            INES5.PRGBANK05114 = 0;
            INES5.PRGBANK15115 = 0;
            INES5.PRGBANK25116 = 0;
            INES5.PRGROMBANK35117 = 0xFF;
            INES5.CHRBANKSWITCHING5120 = 0;
            INES5.CHRBANKSWITCHING5121 = 0;
            INES5.CHRBANKSWITCHING5122 = 0;
            INES5.CHRBANKSWITCHING5123 = 0;
            INES5.CHRBANKSWITCHING5124 = 0;
            INES5.CHRBANKSWITCHING5125 = 0;
            INES5.CHRBANKSWITCHING5126 = 0;
            INES5.CHRBANKSWITCHING5127 = 0;
            INES5.CHRBANKSWITCHING5128 = 0;
            INES5.CHRBANKSWITCHING5129 = 0;
            INES5.CHRBANKSWITCHING512A = 0;
            INES5.CHRBANKSWITCHING512B = 0;
            INES5.UPPERCHRBANKBITS5130 = 0;
            INES5.VERTICALSPLITMODE5200 = 0;
            INES5.VERTICALSPLITSCROLL5201 = 0;
            INES5.VERTICALSPLITBANK5202 = 0;
            INES5.IRQCOUNTER5203 = 0;
            INES5.IRQSTATUS5204 = 0;
            INES5.MULTIPLIER5205 = 0;
            INES5.MULTIPLIER5206 = 0;
            INES5.EXPANSIONRAM5C00 = 0;


        }
}

    class INES0 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
        }

        public override void LoadRom()
        {
            if (GAME.NUMPRGPAGES == 1)
            {
                GAME.SwitchPRGBank(0, 0x4000, 0x4000, 0x8000, 0);
                GAME.SwitchPRGBank(0, 0x4000, 0x4000, 0xC000, 0);              
            }
            else
            {
                GAME.SwitchPRGBank(0, 0x4000, 0x8000, 0x8000, 0);
            }
        }
    }

    class INES1 : GAME
    {
        private static int mmc1fivebitvalue;
        public static int MMC1FIVEBITVALUE
        {
            get { return mmc1fivebitvalue; }
            set { mmc1fivebitvalue = value; }
        }
        private static int mmc1count;
        public static int MMC1COUNT
        {
            get { return mmc1count; }
            set { mmc1count = value; }
        }
        private static int mmc1reg0;
        public static int MMC1REG0
        {
            get { return mmc1reg0; }
            set { mmc1reg0 = value; }
        }
        private static int mmc1reg1;
        public static int MMC1REG1
        {
            get { return mmc1reg1; }
            set { mmc1reg1 = value; }
        }
        private static int mmc1reg2;
        public static int MMC1REG2
        {
            get { return mmc1reg2; }
            set { mmc1reg2 = value; }
        }
        private static int mmc1reg3;
        public static int MMC1REG3
        {
            get { return mmc1reg3; }
            set { mmc1reg3 = value; }
        }
               
        private static int _LastWriteCycle = -4;
        public static int LastWriteCycle
        {
            get { return _LastWriteCycle; }
            set { _LastWriteCycle = value; }
        }

        public static bool SwitchLowHigh = false;
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress <= 0xFFFF && (((CPU.TotalCPUCycles - 1)!=LastWriteCycle) ))
            {
                LastWriteCycle = CPU.TotalCPUCycles;
                if (valuetobewritten > 127)
                {
                    MMC1COUNT = 0;
                    MMC1REG0 = MMC1REG0 | 12;
                }
                else
                {
                    MMC1COUNT++;
                    if ((valuetobewritten & 1) == 0)
                    {
                        MMC1FIVEBITVALUE &= ~(1 << 5);
                    }
                    else
                    {
                        MMC1FIVEBITVALUE |= 1 << 5;
                    }
                    MMC1FIVEBITVALUE = ((MMC1FIVEBITVALUE >> 1) & 0xFF);
                    if (MMC1COUNT == 5)
                    {
                        MMC1FIVEBITVALUE = MMC1FIVEBITVALUE & 31;
                        if (MemoryAddress >= 0x8000 && MemoryAddress < 0xA000)
                        {
                            MMC1REG0 = MMC1FIVEBITVALUE & 31;
                            if ((MMC1REG0 & 3) == 0)
                            {
                                PPU.VERTICAL = 2;
                                Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2400, 0x400);
                                Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2000, 0x400);
                                Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2c00, 0x400);
                            }
                            else if ((MMC1REG0 & 3) == 1)
                            {
                                PPU.VERTICAL = 3;
                                Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2000, 0x400);
                                Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2800, 0x400);
                                Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2c00, 0x400);
                            }
                            else if ((MMC1REG0 & 3) == 2)
                            {
                                PPU.VERTICAL = 1;
                                Array.Copy(PPU.PPUMemory, 0x2000, PPU.PPUMemory, 0x2800, 0x400);
                                Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2c00, 0x400);

                            }
                            else if ((MMC1REG0 & 3) == 3)
                            {
                                PPU.VERTICAL = 0;
                                Array.Copy(PPU.PPUMemory, 0x2000, PPU.PPUMemory, 0x2400, 0x400);
                                Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2c00, 0x400);
                            }
                            MMC1PRG();
                        }
                        if (MemoryAddress >= 0xA000 && MemoryAddress < 0xC000)
                        {
                            MMC1REG1 = MMC1FIVEBITVALUE &31;
                            MMC1PRG();
                            if (GAME.NUMCHRPAGES > 0)
                            {
                                if ((((MMC1REG0 >> 4) & 1) == 0))
                                {
                                    GAME.SwitchCHRBank((MMC1REG1 >> 1), 0x2000, 0x2000, 0x0000);
                                }
                                else if ((((MMC1REG0 >> 4) & 1) == 1))
                                {
                                    GAME.SwitchCHRBank(MMC1REG1, 0x1000, 0x1000, 0x0000);
                                }
                            }
                            else if (GAME.NUMCHRPAGES == 0 && (((MMC1REG0 >> 4) & 1) == 1))
                            {
                                GAME.SwitchCHRRAMBank((MMC1REG1 & 1), 0x1000, 0x1000, 0x0000);
                            }
                            if (GAME.NUMCHRPAGES <= 2)
                            {
                                GAME.SwitchPRGRAMBank(((MMC1REG1 >> 2) & 3), 0x2000, 0x2000, 0x6000);
                            }
                        }
                        if (MemoryAddress >= 0xc000 && MemoryAddress <= 0xdfff)
                        {
                            MMC1REG2 = MMC1FIVEBITVALUE & 31;
                            if (GAME.NUMCHRPAGES > 0 && (((MMC1REG0 >> 4) & 1) == 1))
                            {
                                GAME.SwitchCHRBank(MMC1REG2, 0x1000, 0x1000, 0x1000);
                            }
                            else if (GAME.NUMCHRPAGES == 0 && (((MMC1REG0 >> 4) & 1) == 1))
                            {
                                GAME.SwitchCHRRAMBank((MMC1REG2 & 1), 0x1000, 0x1000, 0x1000);
                            }
                            if(GAME.NUMCHRPAGES <= 2 && (((MMC1REG0 >> 4) & 1) == 1))
                            {
                                GAME.SwitchPRGRAMBank(((MMC1REG2 >> 2) & 3), 0x2000, 0x2000, 0x6000);
                            }
                        }
                        if (MemoryAddress >= 0xe000 && MemoryAddress < 0x10000)
                        {
                            MMC1REG3 = MMC1FIVEBITVALUE & 15;
                            MMC1PRG();                           
                        }
                    }
                    if (MMC1COUNT == 5) { MMC1COUNT = 0; }
                }
            }
            if(MemoryAddress>=0x6000 && MemoryAddress < 0x8000)
            {
                GAME.TheMapper.WritePRGRAM(MemoryAddress, valuetobewritten);
            }
        }

        public static void MMC1CHR()
        {
            if ((((MMC1REG0 >> 4) & 1) == 1))
            {
                GAME.SwitchCHRBank(MMC1REG1 & ((GAME.NUMCHRPAGES * 8) - 1), 0x1000, 0x1000, 0x0000);
                GAME.SwitchCHRBank(MMC1REG2 & ((GAME.NUMCHRPAGES * 8) - 1), 0x1000, 0x1000, 0x1000);
            }
            else
            {
                GAME.SwitchCHRBank(MMC1REG1 & ((GAME.NUMCHRPAGES * 8) - 1), 0x1000, 0x2000, 0x0000);
            }
        }
        public static void MMC1PRG()
        {
            int offset = (((MMC1REG1 >> 4) & 1) * 16 * (0x4000));
            if ((((MMC1REG0 >> 2) & 3) < 2))
            {
                GAME.SwitchPRGBank((MMC1REG3 & ~1), 0x4000, 0x4000, 0x8000, offset);
                GAME.SwitchPRGBank(((MMC1REG3 & ~1) | 0x01), 0x4000, 0x4000, 0xC000, offset);
            }
            else if ((((MMC1REG0 >> 2) & 3) == 2))
            {
                GAME.SwitchPRGBank(((MMC1REG3 & 15)), 0x4000, 0x4000, 0xC000, offset);
                GAME.SwitchPRGBank(0, 0x4000, 0x4000, 0x8000, offset);

            }
            else if ((((MMC1REG0 >> 2) & 3) == 3))
            {
                GAME.SwitchPRGBank(((MMC1REG3 & 15)), 0x4000, 0x4000, 0x8000, offset);
                GAME.SwitchPRGBank(((GAME.NUMPRGPAGES - 1) & 15), 0x4000, 0x4000, 0xC000, offset);
            }
        }

    }

    class INES2 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress < 0x10000)
            {
                GAME.SwitchPRGBank((valuetobewritten & 15), 0x4000, 0x4000, 0x8000, 0);
            }
        }
    }

    class INES3 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress <= 0xFFFF)
            {
                int numbanks = valuetobewritten & 3;
                if (GAME.NUMCHRPAGES < 3) { numbanks = valuetobewritten & 1; }
                GAME.SwitchCHRBank(numbanks, 0x2000, 0x2000, 0x0000);
            }
        }
    }

    class INES4 : GAME
    {
        private static bool _MMC3IRQ;
        public static bool MMC3IRQ
        {
            get { return _MMC3IRQ; }
            set { _MMC3IRQ = value; }
        }

        private static bool _FIRE12LATCH;
        public static bool FIRE12LATCH
        {
            get { return _FIRE12LATCH; }
            set { _FIRE12LATCH = value; }
        }

        private static int _MMC3IRQCOUNTER;
        public static int MMC3IRQCOUNTER
        {
            get { return _MMC3IRQCOUNTER; }
            set { _MMC3IRQCOUNTER = value; }
        }

        private static int _TARGETSCANLINE;
        public static int TARGETSCANLINE
        {
            get { return _TARGETSCANLINE; }
            set { _TARGETSCANLINE = value; }
        }

        private static int _MMC3A12;
        public static int MMC3A12
        {
            get { return _MMC3A12; }
            set { _MMC3A12 = value; }
        }

        private static int _MMC3RELOADVALUE;
        public static int MMC3RELOADVALUE
        {
            get { return _MMC3RELOADVALUE; }
            set { _MMC3RELOADVALUE = value; }
        }
        private static bool _MMC3IRQENABLE;
        public static bool MMC3IRQENABLE
        {
            get { return _MMC3IRQENABLE; }
            set { _MMC3IRQENABLE = value; }
        }

        private static bool _MMC3RELOADFLAG;
        public static bool MMC3RELOADFLAG
        {
            get { return _MMC3RELOADFLAG; }
            set { _MMC3RELOADFLAG = value; }
        }
        public static int IRQDELAY = 8;
        public static int TRANSDOT = 0;
        public static int TRANSSCAN = 0;

        private static int _MMC3E001h;
        public static int MMC3E001h
        {
            get { return _MMC3E001h; }
            set { _MMC3E001h = value; }
        }

        private static int _MMC3E000h;
        public static int MMC3E000h
        {
            get { return _MMC3E000h; }
            set { _MMC3E000h = value; }
        }

        private static int _MMC3C001h;
        public static int MMC3C001h
        {
            get { return _MMC3C001h; }
            set { _MMC3C001h = value; }
        }

        private static int _MMC3C000h;
        public static int MMC3C000h
        {
            get { return _MMC3C000h; }
            set { _MMC3C000h = value; }
        }

        private static int _MMC3A001h;
        public static int MMC3A001h
        {
            get { return _MMC3A001h; }
            set { _MMC3A001h = value; }
        }

        private static int _MMC3A000h;
        public static int MMC3A000h
        {
            get { return _MMC3A000h; }
            set { _MMC3A000h = value; }
        }

        private static int _MMC38001h;
        public static int MMC38001h
        {
            get { return _MMC38001h; }
            set { _MMC38001h = value; }
        }
        public static int prginvert = 0;
        private static int _MMC38000h;
        public static int MMC38000h
        {
            get { return _MMC38000h; }
            set { _MMC38000h = value; }
        }

        public override void LoadRom()
        {
            GAME.SwitchPRGBank((GAME.NUMPRGPAGES - 2), 0x4000, 0x8000, 0x8000, 0);
        }
        public override int GetMirroringType()
        {
            return PPU.VERTICAL;
        }
        public override void SetA12Latch(int dot, bool addrwrite)
        {
            if ((dot >=0 && dot < 256) || dot>321)
            {
                //if (((PPU.PPUCTRL2000 >> 5) & 1) == 0)
                //{
                    if (INES4.MMC3A12 != ((PPU.PPUCTRL2000 >> 4) & 1))
                    {
                        INES4.MMC3A12 ^= 1;
                        if (INES4.MMC3A12 == 1)
                        {
                            FIRE12LATCH = true;
                            TARGETSCANLINE = PPU.Scanlines;
                        }
                    }
                //}               
            }
            if (dot >= 257 && dot < 320)
            {
                if (((PPU.PPUCTRL2000 >> 5) & 1) == 0)
                {
                    if (INES4.MMC3A12 != ((PPU.PPUCTRL2000 >> 3) & 1))
                    {
                        INES4.MMC3A12 ^= 1;
                        if (INES4.MMC3A12 == 1)
                        {
                            FIRE12LATCH = true;
                        }
                        TARGETSCANLINE = PPU.Scanlines;
                    }                  
                }
                else
                {
                    if ((INES4.MMC3A12 != (PPU.SpriteOutputs[(((dot - 256) - (dot % 8)) / 8), 1] & 1)))
                    {
                        INES4.MMC3A12 ^= 1;
                        if (INES4.MMC3A12 == 1)
                        {
                            FIRE12LATCH = true;
                        }
                        TARGETSCANLINE = PPU.Scanlines;
                    }
                }
            }
            if((((PPU.PPUCTRL2000 >> 5) & 1) == 0 && ((((PPU.PPUCTRL2000 >> 3) & 1)==1) && dot==260 && (((PPU.PPUCTRL2000 >> 4) & 1) == 0) || (((PPU.PPUCTRL2000 >> 3) & 1) == 0) && dot == 320 && (((PPU.PPUCTRL2000 >> 4) & 1) == 1))) || (FIRE12LATCH && ((PPU.PPUCTRL2000 >> 5) & 1) == 1))
            {
                if (INES4.MMC3IRQCOUNTER == 0 || INES4.MMC3RELOADFLAG)
                { 
                    int prev = INES4.MMC3IRQCOUNTER;
                    INES4.MMC3IRQCOUNTER = INES4.MMC3RELOADVALUE;
                    INES4.MMC3RELOADFLAG = false;
                    if (INES4.MMC3IRQENABLE && ((prev == 0 && TARGETSCANLINE!=0) || (((PPU.PPUCTRL2000 >> 3) & 1) == 0) && dot == 320 && ((PPU.PPUCTRL2000 >> 4) & 1) == 1 && prev==1 && TARGETSCANLINE!=0  ))
                    {
                        INES4.MMC3IRQ = true;
                    }
                    else
                    {
                        INES4.MMC3IRQ = false;
                    }
                }
                else
                {
                    INES4.MMC3IRQCOUNTER--;
                    INES4.MMC3IRQ = false;
                }                
                FIRE12LATCH = false;
            }
        }
        public int ReadAddress(int MemoryAddress)
        {
            int retval = 0xff;
            if (MemoryAddress >= 0x8000 && MemoryAddress < 0xa000)
            {
                if (MemoryAddress % 2 == 0)
                {
                    retval = MMC38000h;
                }
                else
                {
                    retval = MMC38001h;
                }
            }
            else if (MemoryAddress >= 0xa000 && MemoryAddress < 0xc000)
            {
                if (MemoryAddress % 2 == 0)
                {
                    retval = MMC3A000h;
                }
                else
                {
                    retval = MMC3A001h;
                }
            }
            else if (MemoryAddress >= 0xc000 && MemoryAddress < 0xf000)
            {
                if (MemoryAddress % 2 == 0)
                {
                    retval = MMC3IRQCOUNTER;
                }
                else
                {
                    retval = MMC3C001h;
                }
            }
            return retval;
        }
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {

            if (MemoryAddress >= 0x8000 && MemoryAddress <= 0xFFFF && GAME.MAPPERNUMBER == 4)
            {
                if (MemoryAddress >= 0x8000 && MemoryAddress < 0xA000 && (MemoryAddress%2)==0)
                {                   
                    if ((MMC38000h >> 6 & 1) != (valuetobewritten >> 6 & 1))
                    {
                        for (int y = 0x8000; y < (0xA000); y++)
                        {
                            int temp = CPU.CPUMemory[y];
                            CPU.CPUMemory[y] = CPU.CPUMemory[y + 0x4000];
                            CPU.CPUMemory[y + 0x4000] = temp;
                        }
                    }
                    MMC38000h = valuetobewritten;
                }
                if (MemoryAddress >= 0x8000 && MemoryAddress < 0xA000 && (MemoryAddress % 2) == 1)
                {
                    if ((MMC38000h & 7) == 0 && GAME.NUMCHRPAGES>0)
                    {
                        GAME.SwitchCHRBank(valuetobewritten & 0xFE, 0x0400, 0x0400, ((MMC38000h >> 7 & 1) * 0x1000));
                        GAME.SwitchCHRBank(valuetobewritten | 0x01, 0x0400, 0x0400, 0x0400+((MMC38000h >> 7 & 1) * 0x1000));
                    }
                    else if ((MMC38000h & 7) == 1 && GAME.NUMCHRPAGES > 0)
                    {
                        GAME.SwitchCHRBank(valuetobewritten & 0xFE, 0x0400, 0x0400, 0x0800+((MMC38000h >> 7 & 1) * 0x1000));
                        GAME.SwitchCHRBank(valuetobewritten | 0x01, 0x0400, 0x0400, 0x0C00 + ((MMC38000h >> 7 & 1) * 0x1000));
                    }
                    else if ((MMC38000h & 7) == 2 && GAME.NUMCHRPAGES > 0)
                    {
                        GAME.SwitchCHRBank(valuetobewritten, 0x0400, 0x0400, 0x1000 - ((MMC38000h >> 7 & 1) * 0x1000));
                    }
                    else if ((MMC38000h & 7) == 3 && GAME.NUMCHRPAGES > 0)
                    {
                        GAME.SwitchCHRBank(valuetobewritten, 0x0400, 0x0400, 0x1400 - ((MMC38000h >> 7 & 1) * 0x1000));
                    }
                    else if ((MMC38000h & 7) == 4 && GAME.NUMCHRPAGES > 0)
                    {
                        GAME.SwitchCHRBank(valuetobewritten, 0x0400, 0x0400, 0x1800 - ((MMC38000h >> 7 & 1) * 0x1000));
                    }
                    else if ((MMC38000h & 7) == 5 && GAME.NUMCHRPAGES > 0)
                    {
                        GAME.SwitchCHRBank(valuetobewritten, 0x0400, 0x0400, 0x1C00 - ((MMC38000h >> 7 & 1) * 0x1000));
                    }
                    else if ((MMC38000h & 7) == 6)
                    {
                        GAME.SwitchPRGBank((valuetobewritten & ((GAME.NUMPRGPAGES * 2) - 1)), 0x2000, 0x2000, 0x8000 + ((MMC38000h >> 6 & 1) * 0x4000), 0);
                    }
                    else if ((MMC38000h & 7) == 7)
                    {
                        GAME.SwitchPRGBank((valuetobewritten & ((GAME.NUMPRGPAGES * 2) - 1)), 0x2000, 0x2000, 0xA000, 0);
                    }
                    MMC38001h = valuetobewritten;
                }
                if (MemoryAddress >= 0xa000 && MemoryAddress < 0xc000 && (MemoryAddress % 2) == 0)
                {
                    MMC3A000h = valuetobewritten;
                    if (PPU.VERTICAL < 2)
                    {
                        if ((MMC3A000h & 1) == 1)
                        {
                            PPU.VERTICAL = 0;
                        }
                        else
                        {
                            PPU.VERTICAL = 1;
                        }
                    }
                }
                if (MemoryAddress >= 0xa000 && MemoryAddress < 0xc000 && (MemoryAddress % 2) == 1)
                {
                    MMC3A001h = valuetobewritten;
                }
                if (MemoryAddress >= 0xc000 && MemoryAddress < 0xe000 && (MemoryAddress % 2) == 0)
                {
                    MMC3C000h = valuetobewritten;
                    MMC3RELOADVALUE = MMC3C000h;
                }
                if (MemoryAddress >= 0xC000 && MemoryAddress < 0xE000 && (MemoryAddress % 2) == 1)
                {
                    MMC3C001h = valuetobewritten;
                    //MMC3IRQCOUNTER = 0;
                    MMC3RELOADFLAG = true;
                }
                if (MemoryAddress >= 0xE000 && MemoryAddress < 0x10000 && (MemoryAddress % 2) == 0)
                {
                    MMC3E000h = valuetobewritten;
                    MMC3IRQENABLE = false;
                }
                if (MemoryAddress >= 0xE000 && MemoryAddress < 0x10000 && (MemoryAddress % 2) == 1)
                {
                    MMC3E001h = valuetobewritten;
                    MMC3IRQENABLE = true;
                }
            }
        }
    }

    class INES5 : GAME
    {
        private static int _PRGMODE5100;
        public static int PRGMODE5100
        {
            get { return _PRGMODE5100; }
            set { _PRGMODE5100 = value; }
        }
        private static int _CHRMODE5101;
        public static int CHRMODE5101
        {
            get { return _CHRMODE5101; }
            set { _CHRMODE5101 = value; }
        }

        private static int _PRGRAMPROTECT5102;
        public static int PRGRAMPROTECT5102
        {
            get { return _PRGRAMPROTECT5102; }
            set { _PRGRAMPROTECT5102 = value; }
        }

        private static int _PRGRAMPROTECT5103;
        public static int PRGRAMPROTECT5103
        {
            get { return _PRGRAMPROTECT5103; }
            set { _PRGRAMPROTECT5103 = value; }
        }

        private static int _EXTRAM5104;
        public static int EXTRAM5104
        {
            get { return _EXTRAM5104; }
            set { _EXTRAM5104 = value; }
        }

        private static int _NAMETABLEMAPPING5105;
        public static int NAMETABLEMAPPING5105
        {
            get { return _NAMETABLEMAPPING5105; }
            set { _NAMETABLEMAPPING5105 = value; }
        }

        private static int _FILLEMODETILE5106;
        public static int FILLEMODETILE5106
        {
            get { return _FILLEMODETILE5106; }
            set { _FILLEMODETILE5106 = value; }
        }

        private static int _FILLEMODECOLOR5107;
        public static int FILLEMODECOLOR5107
        {
            get { return _FILLEMODECOLOR5107; }
            set { _FILLEMODECOLOR5107 = value; }
        }

        private static int _PRGRAMBANK5113;
        public static int PRGRAMBANK5113
        {
            get { return _PRGRAMBANK5113; }
            set { _PRGRAMBANK5113 = value; }
        }

        private static int _PRGBANK05114;
        public static int PRGBANK05114
        {
            get { return _PRGBANK05114; }
            set { _PRGBANK05114 = value; }
        }

        private static int _PRGBANK15115;
        public static int PRGBANK15115
        {
            get { return _PRGBANK15115; }
            set { _PRGBANK15115 = value; }
        }

        private static int _PRGBANK25116;
        public static int PRGBANK25116
        {
            get { return _PRGBANK25116; }
            set { _PRGBANK25116 = value; }
        }

        private static int _PRGROMBANK35117;
        public static int PRGROMBANK35117
        {
            get { return _PRGROMBANK35117; }
            set { _PRGROMBANK35117 = value; }
        }



        private static int _CHRBANKSWITCHING5120;
        public static int CHRBANKSWITCHING5120
        {
            get { return _CHRBANKSWITCHING5120; }
            set { _CHRBANKSWITCHING5120 = value; }
        }

        private static int _CHRBANKSWITCHING5121;
        public static int CHRBANKSWITCHING5121
        {
            get { return _CHRBANKSWITCHING5121; }
            set { _CHRBANKSWITCHING5121 = value; }
        }

        private static int _CHRBANKSWITCHING5122;
        public static int CHRBANKSWITCHING5122
        {
            get { return _CHRBANKSWITCHING5122; }
            set { _CHRBANKSWITCHING5122 = value; }
        }

        private static int _CHRBANKSWITCHING5123;
        public static int CHRBANKSWITCHING5123
        {
            get { return _CHRBANKSWITCHING5123; }
            set { _CHRBANKSWITCHING5123 = value; }
        }

        private static int _CHRBANKSWITCHING5124;
        public static int CHRBANKSWITCHING5124
        {
            get { return _CHRBANKSWITCHING5124; }
            set { _CHRBANKSWITCHING5124 = value; }
        }

        private static int _CHRBANKSWITCHING5125;
        public static int CHRBANKSWITCHING5125
        {
            get { return _CHRBANKSWITCHING5125; }
            set { _CHRBANKSWITCHING5125 = value; }
        }

        private static int _CHRBANKSWITCHING5126;
        public static int CHRBANKSWITCHING5126
        {
            get { return _CHRBANKSWITCHING5126; }
            set { _CHRBANKSWITCHING5126 = value; }
        }

        private static int _CHRBANKSWITCHING5127;
        public static int CHRBANKSWITCHING5127
        {
            get { return _CHRBANKSWITCHING5127; }
            set { _CHRBANKSWITCHING5127 = value; }
        }

        private static int _CHRBANKSWITCHING5128;
        public static int CHRBANKSWITCHING5128
        {
            get { return _CHRBANKSWITCHING5128; }
            set { _CHRBANKSWITCHING5128 = value; }
        }

        private static int _CHRBANKSWITCHING5129;
        public static int CHRBANKSWITCHING5129
        {
            get { return _CHRBANKSWITCHING5129; }
            set { _CHRBANKSWITCHING5129 = value; }
        }

        private static int _CHRBANKSWITCHING512A;
        public static int CHRBANKSWITCHING512A
        {
            get { return _CHRBANKSWITCHING512A; }
            set { _CHRBANKSWITCHING512A = value; }
        }

        private static int _CHRBANKSWITCHING512B;
        public static int CHRBANKSWITCHING512B
        {
            get { return _CHRBANKSWITCHING512B; }
            set { _CHRBANKSWITCHING512B = value; }
        }

        private static int _UPPERCHRBANKBITS5130;
        public static int UPPERCHRBANKBITS5130
        {
            get { return _UPPERCHRBANKBITS5130; }
            set { _UPPERCHRBANKBITS5130 = value; }
        }

        private static int _VERTICALSPLITMODE5200;
        public static int VERTICALSPLITMODE5200
        {
            get { return _VERTICALSPLITMODE5200; }
            set { _VERTICALSPLITMODE5200 = value; }
        }

        private static int _VERTICALSPLITSCROLL5201;
        public static int VERTICALSPLITSCROLL5201
        {
            get { return _VERTICALSPLITSCROLL5201; }
            set { _VERTICALSPLITSCROLL5201 = value; }
        }

        private static int _VERTICALSPLITSCROLL5202;
        public static int VERTICALSPLITBANK5202
        {
            get { return _VERTICALSPLITSCROLL5202; }
            set { _VERTICALSPLITSCROLL5202 = value; }
        }

        private static int _IRQCOUNTER5203;
        public static int IRQCOUNTER5203
        {
            get { return _IRQCOUNTER5203; }
            set { _IRQCOUNTER5203 = value; }
        }

        private static int _IRQSTATUS5204;
        public static int IRQSTATUS5204
        {
            get { return _IRQSTATUS5204; }
            set { _IRQSTATUS5204 = value; }
        }

        private static int _MULTIPLIER5205;
        public static int MULTIPLIER5205
        {
            get { return _MULTIPLIER5205; }
            set { _MULTIPLIER5205 = value; }
        }

        private static bool _UseMMC5Banks;
        public static bool UseMMC5Banks
        {
            get { return _UseMMC5Banks; }
            set { _UseMMC5Banks = value; }
        }

        private static int _MULTIPLIER5206;
        public static int MULTIPLIER5206
        {
            get { return _MULTIPLIER5206; }
            set { _MULTIPLIER5206 = value; }
        }

        private static int _EXPANSIONRAM5C00;
        public static int EXPANSIONRAM5C00
        {
            get { return _EXPANSIONRAM5C00; }
            set { _EXPANSIONRAM5C00 = value; }
        }

        private static int _CURRENTSCANLINE;
        public static int CURRENTSCANLINE
        {
            get { return _CURRENTSCANLINE; }
            set { _CURRENTSCANLINE = value; }
        }


        public override int ReadFromAddress(int MemoryAddress)
        {
            switch (MemoryAddress)
            {
                case 0x5204:
                    if (((PPU.Scanlines < 241 && PPU.Scanlines >= 1) || (PPU.Scanlines == 0 && PPU.PPUCycleCount >= 339)) && (((PPU.PPUMASK2001 >> 4) & 1) == 1 || ((PPU.PPUMASK2001 >> 3) & 1) == 1))
                    {
                        int hgd = PPU.PPUCycleCount;
                        IRQSTATUS5204 |= 1 << 6;
                    }
                    else
                    {
                        IRQSTATUS5204 &= ~(1 << 6);
                    }
                    int tempirrq = IRQSTATUS5204;
                    IRQSTATUS5204 &= ~(1 << 7);
                    return tempirrq;
                case 0x5205:
                    int product = MULTIPLIER5205 * MULTIPLIER5206;
                    int lower = product & 0xFF;
                    return lower & 0xFF;
                case 0x5206:
                    int product2 = MULTIPLIER5205 * MULTIPLIER5206;
                    int upper = product2 >> 8;
                    return upper & 0xFF;

            }
            return CPU.CPUMemory[MemoryAddress];
        }
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x5000 && MemoryAddress < 0x6000)
            {
                if (MemoryAddress >= 0x5C00 && MemoryAddress < 0x6000)
                {
                    CPU.CPUMemory[MemoryAddress] = valuetobewritten;
                }
                switch (MemoryAddress)
                {
                    case 0x5100:
                        PRGMODE5100 = valuetobewritten & 3;
                        break;

                    case 0x5101:
                        CHRMODE5101 = valuetobewritten & 3;
                        MMC5SwitchCHRROM2();
                        break;
                    case 0x5102:
                        PRGRAMPROTECT5102 = valuetobewritten & 3;
                        break;
                    case 0x5103:
                        PRGRAMPROTECT5103 = valuetobewritten & 3;
                        break;
                    case 0x5104:
                        EXTRAM5104 = valuetobewritten & 3;
                        break;
                    case 0x5105:
                        NAMETABLEMAPPING5105 = valuetobewritten;
                        if (NAMETABLEMAPPING5105 == 0)
                        {
                            PPU.VERTICAL = 2;
                            Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2400, 0x400);
                            Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2000, 0x400);
                            Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2c00, 0x400);
                        }
                        else if (NAMETABLEMAPPING5105 == 0x55)
                        {
                            PPU.VERTICAL = 3;
                            Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2000, 0x400);
                            Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2800, 0x400);
                            Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2c00, 0x400);
                        }
                        else if (NAMETABLEMAPPING5105 == 0x44)
                        {
                            PPU.VERTICAL = 1;
                            Array.Copy(PPU.PPUMemory, 0x2000, PPU.PPUMemory, 0x2800, 0x400);
                            Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2c00, 0x400);

                        }
                        else if (NAMETABLEMAPPING5105 == 0x50)
                        {
                            PPU.VERTICAL = 0;
                            Array.Copy(PPU.PPUMemory, 0x2000, PPU.PPUMemory, 0x2400, 0x400);
                            Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2c00, 0x400);
                        }
                        break;
                    case 0x5106:
                        FILLEMODETILE5106 = valuetobewritten;
                        break;
                    case 0x5107:
                        FILLEMODECOLOR5107 = valuetobewritten & 3;
                        break;
                    case 0x5113:
                        PRGRAMBANK5113 = valuetobewritten & 7;
                        GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x2000, 0x6000);
                        break;
                    case 0x5114:
                        PRGBANK05114 = valuetobewritten;
                        if ((((valuetobewritten >> 7) & 1) == 1))
                        {
                            MMC5SwitchPRGROM(0x5114, valuetobewritten);
                        }
                        else
                        {
                            MMC5SwitchPRGRAM(0x5114);
                        }

                        break;
                    case 0x5115:
                        PRGBANK15115 = valuetobewritten;
                        if ((((valuetobewritten >> 7) & 1) == 1))
                        {
                            MMC5SwitchPRGROM(0x5115, valuetobewritten);
                        }
                        else
                        {
                            MMC5SwitchPRGRAM(0x5115);
                        }

                        break;
                    case 0x5116:
                        PRGBANK25116 = valuetobewritten;
                        if ((((valuetobewritten >> 7) & 1) == 1))
                        {
                            MMC5SwitchPRGROM(0x5116, valuetobewritten);
                        }
                        else
                        {
                            MMC5SwitchPRGRAM(0x5116);
                        }

                        break;
                    case 0x5117:
                        PRGROMBANK35117 = valuetobewritten & 127;
                        MMC5SwitchPRGROM(0x5117, valuetobewritten);
                        break;
                    case 0x5120:
                        CHRBANKSWITCHING5120 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        //MMC5SwitchCHRROM(0x5120, CHRBANKSWITCHING5120);
                        UseMMC5Banks = false;
                        break;
                    case 0x5121:
                        CHRBANKSWITCHING5121 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5121, CHRBANKSWITCHING5121);
                        UseMMC5Banks = false;
                        break;
                    case 0x5122:
                        CHRBANKSWITCHING5122 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5122, CHRBANKSWITCHING5122);
                        UseMMC5Banks = false;
                        break;
                    case 0x5123:
                        CHRBANKSWITCHING5123 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5123, CHRBANKSWITCHING5123);
                        UseMMC5Banks = false;
                        break;
                    case 0x5124:
                        CHRBANKSWITCHING5124 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5124, CHRBANKSWITCHING5124);
                        UseMMC5Banks = false;
                        break;
                    case 0x5125:
                        CHRBANKSWITCHING5125 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5125, CHRBANKSWITCHING5125);
                        UseMMC5Banks = false;
                        break;
                    case 0x5126:
                        CHRBANKSWITCHING5126 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5126, CHRBANKSWITCHING5126);
                        UseMMC5Banks = false;
                        break;
                    case 0x5127:
                        CHRBANKSWITCHING5127 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5127, CHRBANKSWITCHING5127);
                        UseMMC5Banks = false;
                        break;
                    case 0x5128:
                        CHRBANKSWITCHING5128 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x5128, CHRBANKSWITCHING5128);
                        UseMMC5Banks = true;
                        break;
                    case 0x5129:
                        CHRBANKSWITCHING5129 = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        //  MMC5SwitchCHRROM(0x5129, CHRBANKSWITCHING5129);
                        UseMMC5Banks = true;
                        break;
                    case 0x512A:
                        CHRBANKSWITCHING512A = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x512A, CHRBANKSWITCHING512A);
                        UseMMC5Banks = true;
                        break;
                    case 0x512B:
                        CHRBANKSWITCHING512B = valuetobewritten;
                        MMC5SwitchCHRROM2();
                        // MMC5SwitchCHRROM(0x512B, CHRBANKSWITCHING512B);
                        UseMMC5Banks = true;
                        break;
                    case 0x5130:
                        UPPERCHRBANKBITS5130 = valuetobewritten & 3;
                        break;
                    case 0x5200:
                        VERTICALSPLITMODE5200 = valuetobewritten;
                        break;
                    case 0x5201:
                        VERTICALSPLITSCROLL5201 = valuetobewritten;
                        break;
                    case 0x5202:
                        VERTICALSPLITBANK5202 = valuetobewritten;
                        break;
                    case 0x5203:
                        IRQCOUNTER5203 = valuetobewritten;
                        break;
                    case 0x5204:
                        IRQSTATUS5204 = valuetobewritten;
                        break;
                    case 0x5205:
                        MULTIPLIER5205 = valuetobewritten;
                        break;
                    case 0x5206:
                        MULTIPLIER5206 = valuetobewritten;
                        break;
                    case 0x5C00:
                        EXPANSIONRAM5C00 = valuetobewritten;
                        break;

                }
            }
            else if (MemoryAddress >= 0x6000 && MemoryAddress < 0x8000)
            {
                if ((PRGRAMPROTECT5102 & 3) == 2 && (PRGRAMPROTECT5103 & 3) == 1)
                {
                    GAME.TheMapper.WritePRGRAM(MemoryAddress, valuetobewritten);
                }
            }


        }
        public void MMC5SwitchPRGRAM(int Register)
        {
            switch (PRGMODE5100)
            {
                case 1:
                    switch (Register)
                    {
                        case 0x5115:
                            GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x4000, 0x8000);
                            break;
                    }
                    break;
                case 2:
                    switch (Register)
                    {
                        case 0x5115:
                            GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x4000, 0x8000);
                            break;
                        case 0x5116:
                            GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x2000, 0xC000);
                            break;
                    }
                    break;
                case 3:
                    switch (Register)
                    {
                        case 0x5114:
                            GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x2000, 0x8000);
                            break;
                        case 0x5115:
                            GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x2000, 0xA000);
                            break;
                        case 0x5116:
                            GAME.SwitchPRGRAMBank(PRGRAMBANK5113, 0x2000, 0x2000, 0xC000);
                            break;
                    }
                    break;
            }

        }
        public void MMC5SwitchCHRROM(int Register, int BankNumber)
        {
            int spritesize = (8 + (8 * ((PPU.PPUCTRL2000 >> 5 & 1))));
            switch (CHRMODE5101)
            {
                case 0: //SwitchCHRMMC516Bank
                    switch (Register)
                    {
                        case 0x5127:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x2000, 0x0000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x2000, 0x0000);
                            }
                            break;
                        case 0x512B:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x2000, 0x0000);
                            }
                            break;
                    }
                    break;
                case 1:
                    switch (Register)
                    {
                        case 0x5123:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x1000, 0x0000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x1000, 0x0000);
                            }
                            break;
                        case 0x5127:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x1000, 0x1000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x1000, 0x1000);
                            }
                            break;
                        case 0x512B:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x1000, 0x0000);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x1000, 0x1000);
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (Register)
                    {
                        case 0x5121:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x800, 0x0000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x0000);
                            }
                            break;
                        case 0x5123:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x800, 0x800);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x800);
                            }
                            break;
                        case 0x5125:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x800, 0x1000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x1000);
                            }
                            break;
                        case 0x5127:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x800, 0x1800);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x1800);
                            }
                            break;
                        case 0x5129:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x0000);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x1000);
                            }
                            break;
                        case 0x512B:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x800);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x800, 0x1800);
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (Register)
                    {
                        case 0x5120:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x0000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0000);
                            }
                            break;
                        case 0x5121:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x0400);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0400);
                            }
                            break;
                        case 0x5122:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x0800);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0800);
                            }
                            break;
                        case 0x5123:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x0C00);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0C00);
                            }
                            break;
                        case 0x5124:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x1000);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1000);
                            }
                            break;
                        case 0x5125:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x1400);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1400);
                            }
                            break;
                        case 0x5126:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x1800);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1800);
                            }
                            break;
                        case 0x5127:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRMMC516Bank(BankNumber, 0x400, 0x400, 0x1C00);
                            }
                            else
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1C00);
                            }
                            break;
                        case 0x5128:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0000);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1000);
                            }
                            break;
                        case 0x5129:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0400);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1400);
                            }
                            break;
                        case 0x512A:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0800);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1800);
                            }
                            break;
                        case 0x512B:
                            if (spritesize == 16)
                            {
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x0C00);
                                GAME.SwitchCHRBank(BankNumber, 0x400, 0x400, 0x1C00);
                            }
                            break;
                    }
                    break;

            }
        }
        public void MMC5SwitchCHRROM2()
        {
            int spritesize = (8 + (8 * ((PPU.PPUCTRL2000 >> 5 & 1))));
            switch (CHRMODE5101) //SwitchCHRMMC516Bank
            {
                case 0:
                    if (spritesize == 16)
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x2000, 0x0000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5127, 0x400, 0x2000, 0x0000);
                    }
                    else
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5127, 0x400, 0x2000, 0x0000);
                    }                 
                    break;
                case 1:
                    if (spritesize == 16)
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x1000, 0x0000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x1000, 0x1000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5123, 0x400, 0x1000, 0x0000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5127, 0x400, 0x1000, 0x1000);
                    }
                    else
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5123, 0x400, 0x1000, 0x0000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5127, 0x400, 0x1000, 0x1000);
                    }
                    break;
                case 2:
                    if (spritesize == 16)
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5129, 0x400, 0x800, 0x0000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5129, 0x400, 0x800, 0x1000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x800, 0x800);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x800, 0x1800);

                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5121, 0x400, 0x800, 0x0000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5123, 0x400, 0x800, 0x800);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5125, 0x400, 0x800, 0x1000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5127, 0x400, 0x800, 0x1800);
                    }
                    else
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5121, 0x400, 0x800, 0x0000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5123, 0x400, 0x800, 0x800);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5125, 0x400, 0x800, 0x1000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5127, 0x400, 0x800, 0x1800);
                    }
                    break;
                case 3:
                     if (spritesize == 16)
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5128, 0x400, 0x400, 0x0000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5128, 0x400, 0x400, 0x1000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5129, 0x400, 0x400, 0x0400);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5129, 0x400, 0x400, 0x1400);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512A, 0x400, 0x400, 0x0800);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512A, 0x400, 0x400, 0x1800);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x400, 0x0C00);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING512B, 0x400, 0x400, 0x1C00);

                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5120, 0x400, 0x400, 0x0000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5121, 0x400, 0x400, 0x0400);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5122, 0x400, 0x400, 0x0800);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5123, 0x400, 0x400, 0x0C00);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5124, 0x400, 0x400, 0x1000);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5125, 0x400, 0x400, 0x1400);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5126, 0x400, 0x400, 0x1800);
                        GAME.SwitchCHRMMC516Bank(CHRBANKSWITCHING5127, 0x400, 0x400, 0x1C00);
                    }
                    else
                    {
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5120, 0x400, 0x400, 0x0000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5121, 0x400, 0x400, 0x0400);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5122, 0x400, 0x400, 0x0800);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5123, 0x400, 0x400, 0x0C00);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5124, 0x400, 0x400, 0x1000);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5125, 0x400, 0x400, 0x1400);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5126, 0x400, 0x400, 0x1800);
                        GAME.SwitchCHRBank(CHRBANKSWITCHING5127, 0x400, 0x400, 0x1C00);
                    }
                    break;

            }
        }
        public void MMC5SwitchPRGROM(int Register, int BankNumber)
        {
            switch (PRGMODE5100)
            {
                case 0:
                    switch (Register)
                    {
                        case 0x5117:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x8000, 0x8000, 0);
                            break;
                    }
                    break;
                case 1:
                    switch (Register)
                    {
                        case 0x5115:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x4000, 0x8000, 0);
                            break;
                        case 0x5117:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x4000, 0xC000, 0);
                            break;
                    }
                    break;
                case 2:
                    switch (Register)
                    {
                        case 0x5115:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x4000, 0x8000, 0);
                            break;
                        case 0x5116:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x2000, 0xC000, 0);
                            break;
                        case 0x5117:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x2000, 0xE000, 0);
                            break;
                    }
                    break;
                case 3:
                    switch (Register)
                    {
                        case 0x5114:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x2000, 0x8000, 0);
                            break;
                        case 0x5115:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x2000, 0xA000, 0);
                            break;
                        case 0x5116:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x2000, 0xC000, 0);
                            break;
                        case 0x5117:
                            GAME.SwitchPRGBank((BankNumber & 127), 0x2000, 0x2000, 0xE000, 0);
                            break;
                    }
                    break;
            }
        }


    }


    class INES7 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress <= 0xFFFF && GAME.MAPPERNUMBER == 7)
            {
                GAME.SwitchPRGBank((valuetobewritten & 7), 0x8000, 0x8000, 0x8000, 0);
                if ((valuetobewritten >> 4 & 1) == 1) {PPU.VERTICAL = 2;}
            }
        }
    }

    class INES9 : GAME
    {
        private static int _Mapper9A000;
        public static int Mapper9A000
        {
            get { return _Mapper9A000; }
            set { _Mapper9A000 = value; }
        }

        private static int _Mapper9B000;
        public static int Mapper9B000
        {
            get { return _Mapper9B000; }
            set { _Mapper9B000 = value; }
        }

        private static int _Mapper9C000;
        public static int Mapper9C000
        {
            get { return _Mapper9C000; }
            set { _Mapper9C000 = value; }
        }

        private static int _Mapper9D000;
        public static int Mapper9D000
        {
            get { return _Mapper9D000; }
            set { _Mapper9D000 = value; }
        }

        private static int _Mapper9E000;
        public static int Mapper9E000
        {
            get { return _Mapper9E000; }
            set { _Mapper9E000 = value; }
        }

        private static int _Mapper9F000;
        public static int Mapper9F000
        {
            get { return _Mapper9F000; }
            set { _Mapper9F000 = value; }
        }
        private static int _Mapper9Latch0;
        public static int Mapper9Latch0
        {
            get { return _Mapper9Latch0; }
            set { _Mapper9Latch0 = value; }
        }
        private static int _Mapper9Latch1;
        public static int Mapper9Latch1
        {
            get { return _Mapper9Latch1; }
            set { _Mapper9Latch1 = value; }
        }
        private static int _CHRBANK1;
        public static int CHRBANK1
        {
            get { return _CHRBANK1; }
            set { _CHRBANK1 = value; }
        }
        private static int _CHRBANK2;
        public static int CHRBANK2
        {
            get { return _CHRBANK2; }
            set { _CHRBANK2 = value; }
        }
        public override void LoadRom()
        {
            GAME.SwitchPRGBank((GAME.NUMPRGPAGES - 2), 0x4000, 0x4000, 0x8000, 0);
            GAME.SwitchPRGBank((GAME.NUMPRGPAGES - 1), 0x4000, 0x4000, 0xC000, 0);
        }

        public override void SetTileLatch(int MemoryAddress)
        {
            int hgd = PPU.Scanlines;
            int latch= (PPU.LoopyT >> 12) & 0x1;
            switch (MemoryAddress+8+ ((PPU.PPUADDR2006 >> 12 & 7)))
            {
                case 0x0FD0:
                    Mapper9Latch0 = 0xFD;
                    break;
                case 0x0FE0:
                    Mapper9Latch0 = 0xFE;
                    break;
                case 0x1FD0:
                    Mapper9Latch1 = 0xFD;                   
                    break;
                case 0x1FE0:
                    Mapper9Latch1 = 0xFE;                    
                    break;
                case 0x0FD8:
                    Mapper9Latch0 = 0xFD;
                    break;
                case 0x0FE8:
                    Mapper9Latch0 = 0xFE;
                    break;
                case int n when (n <= 0x1FDF && n >= 0x1FD8):
                    Mapper9Latch1 = 0xFD;
                    if (CHRBANK2 != (Mapper9D000 & 31))
                    {
                        CHRBANK2 = (Mapper9D000 & 31);
                        GAME.SwitchCHRBank(CHRBANK2, 0x1000, 0x1000, 0x1000);
                    }
                        break;
                case int n when (n <= 0x1FEF && n >= 0x1FE8):
                    Mapper9Latch1 = 0xFE;
                    if(CHRBANK2 != (Mapper9E000 & 31)){
                        CHRBANK2 = (Mapper9E000 & 31);
                        GAME.SwitchCHRBank(CHRBANK2, 0x1000, 0x1000, 0x1000);
                    }
                    break;             
            }
        }

        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0xA000 && MemoryAddress <= 0xFFFF && GAME.MAPPERNUMBER == 9)
            {
                int hsgs = PPU.Scanlines;
                if (MemoryAddress >= 0xA000 && MemoryAddress < 0xB000)
                {
                    Mapper9A000 = valuetobewritten;
                    GAME.SwitchPRGBank((Mapper9A000 & 15), 0x2000, 0x2000, 0x8000, 0);
                }
                else if (MemoryAddress >= 0xB000 && MemoryAddress < 0xC000)
                {
                    Mapper9B000 = valuetobewritten;
                    if (Mapper9Latch0 == 0xFD)
                    {
                        CHRBANK1 = Mapper9B000 & 31;
                        GAME.SwitchCHRBank(CHRBANK1, 0x1000, 0x1000, 0x0000);
                    }
                    else
                    {
                        CHRBANK1 = Mapper9C000 & 31;
                        GAME.SwitchCHRBank(CHRBANK1, 0x1000, 0x1000, 0x0000);
                    }
                }
                else if (MemoryAddress >= 0xC000 && MemoryAddress < 0xD000)
                {
                    Mapper9C000 = valuetobewritten;
                    if (Mapper9Latch0 == 0xFE)
                    {
                        CHRBANK1 = Mapper9C000 & 31;
                        GAME.SwitchCHRBank(CHRBANK1, 0x1000, 0x1000, 0x0000);
                    }
                    else
                    {
                        CHRBANK1 = Mapper9B000 & 31;
                        GAME.SwitchCHRBank(CHRBANK1, 0x1000, 0x1000, 0x0000);
                    }
                }
                else if (MemoryAddress >= 0xD000 && MemoryAddress < 0xE000)
                {
                    Mapper9D000 = valuetobewritten;
                    if (Mapper9Latch1 == 0xFD)
                    {
                        CHRBANK2 = Mapper9D000 & 31;
                        GAME.SwitchCHRBank(CHRBANK2, 0x1000, 0x1000, 0x1000);
                    }
                    else
                    {
                        CHRBANK2 = Mapper9E000 & 31;
                        GAME.SwitchCHRBank(CHRBANK2, 0x1000, 0x1000, 0x1000);
                    }
                }
                else if (MemoryAddress >= 0xE000 && MemoryAddress < 0xF000)
                {
                    Mapper9E000 = valuetobewritten;
                    if (Mapper9Latch1 == 0xFE)
                    {
                        CHRBANK2 = Mapper9E000 & 31;
                        GAME.SwitchCHRBank(CHRBANK2, 0x1000, 0x1000, 0x1000);
                    }
                    else
                    {
                        CHRBANK2 = Mapper9D000 & 31;
                        GAME.SwitchCHRBank(CHRBANK2, 0x1000, 0x1000, 0x1000);
                    }
                }
                else if (MemoryAddress >= 0xF000 && MemoryAddress <= 0xFFFF)
                {
                    Mapper9F000 = valuetobewritten;
                    if ((Mapper9F000 & 1) == 0)
                    {
                        PPU.VERTICAL = 1;
                        Array.Copy(PPU.PPUMemory, 0x2000, PPU.PPUMemory, 0x2800, 0x400);
                        Array.Copy(PPU.PPUMemory, 0x2400, PPU.PPUMemory, 0x2c00, 0x400);

                    }
                    else if ((Mapper9F000 & 1) == 1)
                    {
                        PPU.VERTICAL = 0;
                        Array.Copy(PPU.PPUMemory, 0x2000, PPU.PPUMemory, 0x2400, 0x400);
                        Array.Copy(PPU.PPUMemory, 0x2800, PPU.PPUMemory, 0x2c00, 0x400);
                    }
                }
            }
        }
    }

    class INES11 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress < 0x10000)
            {               
                 GAME.SwitchPRGBank(((valuetobewritten) & 3), 0x8000, 0x8000, 0x8000, 0);
                 int numbanks = (valuetobewritten>>4) & 15;
                 if (GAME.NUMCHRPAGES < 3) { numbanks = valuetobewritten & 1; }
                 GAME.SwitchCHRBank(numbanks, 0x2000, 0x2000, 0x0000);
            }
        }
    }

    class INES13 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {        
            if (MemoryAddress >= 0x8000 && MemoryAddress < 0x10000)
            {
                GAME.SwitchCHRRAMBank((valuetobewritten) & 3, 0x1000, 0x1000, 0x1000);
            }
        }
    }

    class INES34 : GAME
    {
        
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x7000 && GAME.MAPPERNUMBER == 34)
            {
                if (MemoryAddress >= 0x8000 && MemoryAddress < 0x10000)
                {
                   GAME.SwitchPRGBank(((valuetobewritten) & 3), 0x8000, 0x8000, 0x8000, 0);
                }
                if (MemoryAddress == 0x7ffd)
                {
                   GAME.SwitchPRGBank(((valuetobewritten) & 1), 0x8000, 0x8000, 0x8000, 0);
                }
                if (MemoryAddress == 0x7ffe)
                {
                    GAME.SwitchCHRBank(((valuetobewritten) & 15), 0x1000, 0x1000, 0x0000);
                }
                if (MemoryAddress == 0x7fff)
                {
                    GAME.SwitchCHRBank(((valuetobewritten) & 15), 0x1000, 0x1000, 0x1000);
                }
            }
        }
    }

    class INES66 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress <= 0xFFFF && GAME.MAPPERNUMBER == 66)
            {
                 GAME.SwitchPRGBank(((valuetobewritten >> 4) & 3), 0x8000, 0x8000, 0x8000, 0);
                 int numbanks = valuetobewritten & 3;
                 if (GAME.NUMCHRPAGES < 3) { numbanks = valuetobewritten & 1; }
                 GAME.SwitchCHRBank(numbanks, 0x2000, 0x2000, 0x0000);
            }
        }
    }

    class INES71 : GAME
    {
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0xC000)
            {
                GAME.SwitchPRGBank(((valuetobewritten &15)), 0x4000, 0x4000, 0x8000, 0);                
            }
        }
    }
    class INES232 : GAME
    {
        private static int _Reg0;
        public static int Reg0
        {
            get { return _Reg0; }
            set { _Reg0 = value; }
        }
       
        public override void WriteToAddress(int MemoryAddress, int valuetobewritten)
        {
            if (MemoryAddress >= 0x8000 && MemoryAddress<0xC000)
            {
                Reg0 = (valuetobewritten >> 3) & 3;
            }
            else if (MemoryAddress >= 0xC000)
            {
                GAME.SwitchPRGBank(((valuetobewritten & 15)), 0x4000, 0x4000, 0x8000, Reg0*0x10000);
            }
        }
    }
}
