using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesPlayer
{
    class Debugger
    {
        public static Tuple<int, string, bool, bool, int>[] OpArray = new Tuple<int, string, bool, bool, int>[256];
        public static string NumberInstructionsToExecute;
        public static int TracePointer = 0;
        public static List<string[]> TraceList = new List<string[]>();
        public static int CountToStop = -1;
        public static bool StopOnCount = false;
        public static int INSTRUCTIONCOUNT = 0;
        public static int StopLine = 0;
        public static List<string[]> BreakPointList = new List<string[]>();
        public static Dictionary<string, string[]> LogList = new Dictionary<string, string[]>();
        public static Dictionary<string, string[]> TestROMList = new Dictionary<string, string[]>();
        private static int[,,] PatterntableTiles = new int[512, 8, 8];

        public static void Initialize()
        {
            OpArray[0x69] = new Tuple<int, string, bool, bool, int>(2, "ADC", false, false, 2);
            OpArray[0x65] = new Tuple<int, string, bool, bool, int>(2, "ADC", false, false, 4);
            OpArray[0x75] = new Tuple<int, string, bool, bool, int>(2, "ADC", false, false, 11);
            OpArray[0x6D] = new Tuple<int, string, bool, bool, int>(3, "ADC", false, false, 3);
            OpArray[0x7D] = new Tuple<int, string, bool, bool, int>(3, "ADC", false, false, 5);
            OpArray[0x79] = new Tuple<int, string, bool, bool, int>(3, "ADC", false, false, 6);
            OpArray[0x61] = new Tuple<int, string, bool, bool, int>(2, "ADC", false, false, 8);
            OpArray[0x71] = new Tuple<int, string, bool, bool, int>(2, "ADC", false, false, 9);
            OpArray[0x29] = new Tuple<int, string, bool, bool, int>(2, "AND", false, false, 2);
            OpArray[0x25] = new Tuple<int, string, bool, bool, int>(2, "AND", false, false, 4);
            OpArray[0x35] = new Tuple<int, string, bool, bool, int>(2, "AND", false, false, 11);
            OpArray[0x2D] = new Tuple<int, string, bool, bool, int>(3, "AND", false, false, 3);
            OpArray[0x3D] = new Tuple<int, string, bool, bool, int>(3, "AND", false, false, 5);
            OpArray[0x39] = new Tuple<int, string, bool, bool, int>(3, "AND", false, false, 6);
            OpArray[0x21] = new Tuple<int, string, bool, bool, int>(2, "AND", false, false, 8);
            OpArray[0x31] = new Tuple<int, string, bool, bool, int>(2, "AND", false, false, 9);
            OpArray[0x0A] = new Tuple<int, string, bool, bool, int>(1, "ASL", true, false, 14);
            OpArray[0x06] = new Tuple<int, string, bool, bool, int>(2, "ASL", true, false, 4);
            OpArray[0x16] = new Tuple<int, string, bool, bool, int>(2, "ASL", true, false, 11);
            OpArray[0x0E] = new Tuple<int, string, bool, bool, int>(3, "ASL", true, false, 3);
            OpArray[0x1E] = new Tuple<int, string, bool, bool, int>(3, "ASL", true, false, 5);
            OpArray[0x90] = new Tuple<int, string, bool, bool, int>(2, "BCC", false, true, 42);
            OpArray[0xB0] = new Tuple<int, string, bool, bool, int>(2, "BCS", false, true, 42);
            OpArray[0xF0] = new Tuple<int, string, bool, bool, int>(2, "BEQ", false, true, 42);
            OpArray[0x24] = new Tuple<int, string, bool, bool, int>(2, "BIT", false, false, 4);
            OpArray[0x2C] = new Tuple<int, string, bool, bool, int>(3, "BIT", false, false, 3);
            OpArray[0x30] = new Tuple<int, string, bool, bool, int>(2, "BMI", false, true, 42);
            OpArray[0xD0] = new Tuple<int, string, bool, bool, int>(2, "BNE", false, true, 42);
            OpArray[0x10] = new Tuple<int, string, bool, bool, int>(2, "BPL", false, true, 42);
            OpArray[0x00] = new Tuple<int, string, bool, bool, int>(2, "BRK", false, false, 2);
            OpArray[0x50] = new Tuple<int, string, bool, bool, int>(2, "BVC", false, true, 42);
            OpArray[0x70] = new Tuple<int, string, bool, bool, int>(2, "BVS", false, true, 42);
            OpArray[0x18] = new Tuple<int, string, bool, bool, int>(1, "CLC", false, false, 0);
            OpArray[0xD8] = new Tuple<int, string, bool, bool, int>(1, "CLD", false, false, 0);
            OpArray[0x58] = new Tuple<int, string, bool, bool, int>(1, "CLI", false, false, 0);
            OpArray[0xB8] = new Tuple<int, string, bool, bool, int>(1, "CLV", false, false, 0);
            OpArray[0xC9] = new Tuple<int, string, bool, bool, int>(2, "CMP", false, false, 2);
            OpArray[0xC5] = new Tuple<int, string, bool, bool, int>(2, "CMP", false, false, 4);
            OpArray[0xD5] = new Tuple<int, string, bool, bool, int>(2, "CMP", false, false, 11);
            OpArray[0xCD] = new Tuple<int, string, bool, bool, int>(3, "CMP", false, false, 3);
            OpArray[0xDD] = new Tuple<int, string, bool, bool, int>(3, "CMP", false, false, 5);
            OpArray[0xD9] = new Tuple<int, string, bool, bool, int>(3, "CMP", false, false, 6);
            OpArray[0xC1] = new Tuple<int, string, bool, bool, int>(2, "CMP", false, false, 8);
            OpArray[0xD1] = new Tuple<int, string, bool, bool, int>(2, "CMP", false, false, 9);
            OpArray[0xE0] = new Tuple<int, string, bool, bool, int>(2, "CPX", false, false, 2);
            OpArray[0xE4] = new Tuple<int, string, bool, bool, int>(2, "CPX", false, false, 4);
            OpArray[0xEC] = new Tuple<int, string, bool, bool, int>(3, "CPX", false, false, 3);

            OpArray[0xC0] = new Tuple<int, string, bool, bool, int>(2, "CPY", false, false, 2);
            OpArray[0xC4] = new Tuple<int, string, bool, bool, int>(2, "CPY", false, false, 4);
            OpArray[0xCC] = new Tuple<int, string, bool, bool, int>(3, "CPY", false, false, 3);

            OpArray[0xC6] = new Tuple<int, string, bool, bool, int>(2, "DEC", true, false, 4);
            OpArray[0xD6] = new Tuple<int, string, bool, bool, int>(2, "DEC", true, false, 11);
            OpArray[0xCE] = new Tuple<int, string, bool, bool, int>(3, "DEC", true, false, 3);
            OpArray[0xDE] = new Tuple<int, string, bool, bool, int>(3, "DEC", true, false, 5);
            OpArray[0xCA] = new Tuple<int, string, bool, bool, int>(1, "DEX", false, false, 0);
            OpArray[0x88] = new Tuple<int, string, bool, bool, int>(1, "DEY", false, false, 0);
            OpArray[0x49] = new Tuple<int, string, bool, bool, int>(2, "EOR", false, false, 2);
            OpArray[0x45] = new Tuple<int, string, bool, bool, int>(2, "EOR", false, false, 4);
            OpArray[0x55] = new Tuple<int, string, bool, bool, int>(2, "EOR", false, false, 11);
            OpArray[0x4D] = new Tuple<int, string, bool, bool, int>(3, "EOR", false, false, 3);
            OpArray[0x5D] = new Tuple<int, string, bool, bool, int>(3, "EOR", false, false, 5);
            OpArray[0x59] = new Tuple<int, string, bool, bool, int>(3, "EOR", false, false, 6);
            OpArray[0x41] = new Tuple<int, string, bool, bool, int>(2, "EOR", false, false, 8);
            OpArray[0x51] = new Tuple<int, string, bool, bool, int>(2, "EOR", false, false, 9);
            OpArray[0xE6] = new Tuple<int, string, bool, bool, int>(2, "INC", true, false, 4);
            OpArray[0xF6] = new Tuple<int, string, bool, bool, int>(2, "INC", true, false, 11);
            OpArray[0xEE] = new Tuple<int, string, bool, bool, int>(3, "INC", true, false, 3);
            OpArray[0xFE] = new Tuple<int, string, bool, bool, int>(3, "INC", true, false, 5);
            OpArray[0xE8] = new Tuple<int, string, bool, bool, int>(1, "INX", false, false, 0);
            OpArray[0xC8] = new Tuple<int, string, bool, bool, int>(1, "INY", false, false, 0);
            OpArray[0x4C] = new Tuple<int, string, bool, bool, int>(3, "JMP", false, false, 10);
            OpArray[0x6C] = new Tuple<int, string, bool, bool, int>(3, "JMP", false, false, 13);
            OpArray[0x20] = new Tuple<int, string, bool, bool, int>(3, "JSR", false, false, 15);
            OpArray[0xA9] = new Tuple<int, string, bool, bool, int>(2, "LDA", false, false, 2);
            OpArray[0xA5] = new Tuple<int, string, bool, bool, int>(2, "LDA", false, false, 4);
            OpArray[0xB5] = new Tuple<int, string, bool, bool, int>(2, "LDA", false, false, 11);
            OpArray[0xAD] = new Tuple<int, string, bool, bool, int>(3, "LDA", false, false, 3);
            OpArray[0xBD] = new Tuple<int, string, bool, bool, int>(3, "LDA", false, false, 5);
            OpArray[0xB9] = new Tuple<int, string, bool, bool, int>(3, "LDA", false, false, 6);
            OpArray[0xA1] = new Tuple<int, string, bool, bool, int>(2, "LDA", false, false, 8);
            OpArray[0xB1] = new Tuple<int, string, bool, bool, int>(2, "LDA", false, false, 9);
            OpArray[0xA2] = new Tuple<int, string, bool, bool, int>(2, "LDX", false, false, 2);
            OpArray[0xA6] = new Tuple<int, string, bool, bool, int>(2, "LDX", false, false, 4);
            OpArray[0xB6] = new Tuple<int, string, bool, bool, int>(2, "LDX", false, false, 12);
            OpArray[0xAE] = new Tuple<int, string, bool, bool, int>(3, "LDX", false, false, 3);
            OpArray[0xBE] = new Tuple<int, string, bool, bool, int>(3, "LDX", false, false, 6);

            OpArray[0xA0] = new Tuple<int, string, bool, bool, int>(2, "LDY", false, false, 2);
            OpArray[0xA4] = new Tuple<int, string, bool, bool, int>(2, "LDY", false, false, 4);
            OpArray[0xB4] = new Tuple<int, string, bool, bool, int>(2, "LDY", false, false, 11);
            OpArray[0xAC] = new Tuple<int, string, bool, bool, int>(3, "LDY", false, false, 3);
            OpArray[0xBC] = new Tuple<int, string, bool, bool, int>(3, "LDY", false, false, 5);
            OpArray[0x4A] = new Tuple<int, string, bool, bool, int>(1, "LSR", false, false, 14);
            OpArray[0x46] = new Tuple<int, string, bool, bool, int>(2, "LSR", true, false, 4);
            OpArray[0x56] = new Tuple<int, string, bool, bool, int>(2, "LSR", true, false, 11);
            OpArray[0x4E] = new Tuple<int, string, bool, bool, int>(3, "LSR", true, false, 3);
            OpArray[0x5E] = new Tuple<int, string, bool, bool, int>(3, "LSR", true, false, 5);
            OpArray[0xEA] = new Tuple<int, string, bool, bool, int>(1, "NOP", false, false, 0);
            OpArray[0x09] = new Tuple<int, string, bool, bool, int>(2, "ORA", false, false, 2);
            OpArray[0x05] = new Tuple<int, string, bool, bool, int>(2, "ORA", false, false, 4);
            OpArray[0x15] = new Tuple<int, string, bool, bool, int>(2, "ORA", false, false, 11);
            OpArray[0x0D] = new Tuple<int, string, bool, bool, int>(3, "ORA", false, false, 3);
            OpArray[0x1D] = new Tuple<int, string, bool, bool, int>(3, "ORA", false, false, 5);
            OpArray[0x19] = new Tuple<int, string, bool, bool, int>(3, "ORA", false, false, 6);
            OpArray[0x01] = new Tuple<int, string, bool, bool, int>(2, "ORA", false, false, 8);
            OpArray[0x11] = new Tuple<int, string, bool, bool, int>(2, "ORA", false, false, 9);
            OpArray[0x48] = new Tuple<int, string, bool, bool, int>(1, "PHA", false, false, 0);
            OpArray[0x08] = new Tuple<int, string, bool, bool, int>(1, "PHP", false, false, 0);
            OpArray[0x68] = new Tuple<int, string, bool, bool, int>(1, "PLA", false, false, 0);
            OpArray[0x28] = new Tuple<int, string, bool, bool, int>(1, "PLP", false, false, 0);
            OpArray[0x2A] = new Tuple<int, string, bool, bool, int>(1, "ROL", true, false, 14);
            OpArray[0x26] = new Tuple<int, string, bool, bool, int>(2, "ROL", true, false, 4);
            OpArray[0x36] = new Tuple<int, string, bool, bool, int>(2, "ROL", true, false, 11);
            OpArray[0x2E] = new Tuple<int, string, bool, bool, int>(3, "ROL", true, false, 3);
            OpArray[0x3E] = new Tuple<int, string, bool, bool, int>(3, "ROL", true, false, 5);
            OpArray[0x6A] = new Tuple<int, string, bool, bool, int>(1, "ROR", true, false, 14);
            OpArray[0x66] = new Tuple<int, string, bool, bool, int>(2, "ROR", true, false, 4);
            OpArray[0x76] = new Tuple<int, string, bool, bool, int>(2, "ROR", true, false, 11);
            OpArray[0x6E] = new Tuple<int, string, bool, bool, int>(3, "ROR", true, false, 3);
            OpArray[0x7E] = new Tuple<int, string, bool, bool, int>(3, "ROR", true, false, 5);
            OpArray[0x40] = new Tuple<int, string, bool, bool, int>(1, "RTI", false, false, 0);
            OpArray[0x60] = new Tuple<int, string, bool, bool, int>(1, "RTS", false, false, 0);
            OpArray[0xE9] = new Tuple<int, string, bool, bool, int>(2, "SBC", true, false, 2);
            OpArray[0xE5] = new Tuple<int, string, bool, bool, int>(2, "SBC", true, false, 4);
            OpArray[0xF5] = new Tuple<int, string, bool, bool, int>(2, "SBC", true, false, 11);
            OpArray[0xED] = new Tuple<int, string, bool, bool, int>(3, "SBC", true, false, 3);
            OpArray[0xFD] = new Tuple<int, string, bool, bool, int>(3, "SBC", true, false, 5);
            OpArray[0xF9] = new Tuple<int, string, bool, bool, int>(3, "SBC", true, false, 6);
            OpArray[0xE1] = new Tuple<int, string, bool, bool, int>(2, "SBC", true, false, 8);
            OpArray[0xF1] = new Tuple<int, string, bool, bool, int>(2, "SBC", true, false, 9);
            OpArray[0x38] = new Tuple<int, string, bool, bool, int>(1, "SEC", false, false, 0);
            OpArray[0xF8] = new Tuple<int, string, bool, bool, int>(1, "SED", false, false, 0);
            OpArray[0x78] = new Tuple<int, string, bool, bool, int>(1, "SEI", false, false, 0);
            OpArray[0x85] = new Tuple<int, string, bool, bool, int>(2, "STA", true, false, 4);
            OpArray[0x95] = new Tuple<int, string, bool, bool, int>(2, "STA", true, false, 11);
            OpArray[0x8D] = new Tuple<int, string, bool, bool, int>(3, "STA", true, false, 3);
            OpArray[0x9D] = new Tuple<int, string, bool, bool, int>(3, "STA", true, false, 5);
            OpArray[0x99] = new Tuple<int, string, bool, bool, int>(3, "STA", true, false, 6);
            OpArray[0x81] = new Tuple<int, string, bool, bool, int>(2, "STA", true, false, 8);
            OpArray[0x91] = new Tuple<int, string, bool, bool, int>(2, "STA", true, false, 9);
            OpArray[0x86] = new Tuple<int, string, bool, bool, int>(2, "STX", true, false, 4);
            OpArray[0x96] = new Tuple<int, string, bool, bool, int>(2, "STX", true, false, 12);
            OpArray[0x8E] = new Tuple<int, string, bool, bool, int>(3, "STX", true, false, 3);
            OpArray[0x84] = new Tuple<int, string, bool, bool, int>(2, "STY", true, false, 4);
            OpArray[0x94] = new Tuple<int, string, bool, bool, int>(2, "STY", true, false, 11);
            OpArray[0x8C] = new Tuple<int, string, bool, bool, int>(3, "STY", true, false, 3);
            OpArray[0xAA] = new Tuple<int, string, bool, bool, int>(1, "TAX", false, false, 0);
            OpArray[0xA8] = new Tuple<int, string, bool, bool, int>(1, "TAY", false, false, 0);
            OpArray[0xBA] = new Tuple<int, string, bool, bool, int>(1, "TSX", false, false, 0);
            OpArray[0x8A] = new Tuple<int, string, bool, bool, int>(1, "TXA", false, false, 0);
            OpArray[0x98] = new Tuple<int, string, bool, bool, int>(1, "TYA", false, false, 0);
            OpArray[0x9A] = new Tuple<int, string, bool, bool, int>(1, "TXS", false, false, 0);
        }
        public static string[] DecodeOpcode(int MemoryAddress)
        {
            int AddressingMode = 0;
            int OpCodeByteLength = 0;
            string OpCodeName = "";
            if (OpArray[CPU.CPUMemory[MemoryAddress & 0xFFFF]] != null)
            {
                AddressingMode = OpArray[CPU.CPUMemory[MemoryAddress & 0xFFFF]].Item5;
                OpCodeByteLength = OpArray[CPU.CPUMemory[MemoryAddress & 0xFFFF]].Item1;
                OpCodeName = OpArray[CPU.CPUMemory[MemoryAddress & 0xFFFF]].Item2;
            }
            string OpCodeByteString = "";
            for (int i = 0; i < OpCodeByteLength; i++)
            {
                OpCodeByteString += " " + CPU.CPUMemory[(MemoryAddress + i) & 0xFFFF].ToString("X2");
            }
            int EffectiveAddress = 0;
            int OutPutEffectiveAddress = 0;
            int TempLow = 0;
            int TempHigh = 0;
            int TempAdd = 0;
            int Operand = 0;
            int Pointer = 0;
            int Val = 0;
            int BranchToAddress = 0;
            string[] OutPutArray = new string[4];
            OutPutArray[3] = OpCodeByteString.Trim();
            MemoryAddress++;
            switch (AddressingMode)
            {
                //Accumulator or implied addressing
                case 1:
                    break;
                //Immediate addressing
                case 2:
                    Val = CPU.CPUMemory[MemoryAddress & 0xFFFF]; ;
                    EffectiveAddress = Val;
                    OutPutArray[0] = OpCodeName + " " + "#$" + Val.ToString("X2");
                    //LDX #$00
                    OutPutEffectiveAddress = Val & 0xFFFF;
                    break;
                //Absolute addressing
                case 3:
                    TempLow = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempHigh = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    EffectiveAddress = TempHigh << 8 | TempLow;
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + "$" + EffectiveAddress.ToString("X4") + " = " + Val.ToString("X2");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Zero page addressing
                case 4:
                    EffectiveAddress = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + "$" + EffectiveAddress.ToString("X2") + " = " + Val.ToString("X2");
                    //STX $90 = 00 
                    OutPutEffectiveAddress = EffectiveAddress & 0xff;
                    break;
                //Absolute indexed X addressing
                case 5:
                    TempLow = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempHigh = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    EffectiveAddress = (TempHigh << 8 | ((TempLow + CPU.IndexRegisterX) & 0xFF)) & 0xFFFF;
                    if ((TempLow + CPU.IndexRegisterX) > 0xFF)
                    {
                        EffectiveAddress += 0x100;
                    }
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + @"$" + (EffectiveAddress - CPU.IndexRegisterX).ToString("X4") + ",X @ " + EffectiveAddress.ToString("X4") + " = " + Val.ToString("X2").Replace("\"", "\"\"");
                    OutPutEffectiveAddress = EffectiveAddress & 0xff;
                    //LDA $0104,X @ 01EA = 22
                    break;
                //fetch operand, increment PC BCS
                case 41:
                    BranchToAddress = 0;
                    Operand = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    if (CPU.ProcessorStatus[0] != 0)
                    {
                        BranchToAddress = ((MemoryAddress) + (Operand - ((Operand >> 7 & 1) * 256))) & 0xFFFF;
                        MemoryAddress++;
                        if (((MemoryAddress - 1) & 0xff00) != (BranchToAddress & 0xFF00))
                        {
                        }
                        else
                        {
                            MemoryAddress = BranchToAddress;
                        }
                    }
                    OutPutArray[0] = OpCodeName + " " + "$" + MemoryAddress.ToString("X4");
                    EffectiveAddress = MemoryAddress;
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //fetch operand, increment PC BEQ
                case 42:
                    BranchToAddress = 0;
                    Operand = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    BranchToAddress = ((MemoryAddress) + (Operand - ((Operand >> 7 & 1) * 256))) & 0xFFFF;
                    MemoryAddress = BranchToAddress;
                    OutPutArray[0] = OpCodeName + " " + "$" + MemoryAddress.ToString("X4");
                    EffectiveAddress = MemoryAddress;
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;

                //Absolute indexed Y addressing
                case 6:
                    TempLow = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempHigh = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    EffectiveAddress = (TempHigh << 8 | ((TempLow + CPU.IndexRegisterY) & 0xFF)) & 0xFFFF;
                    if ((TempLow + CPU.IndexRegisterY) > 0xFF)
                    {
                        EffectiveAddress += 0x100;
                    }
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + @"$" + (EffectiveAddress - CPU.IndexRegisterY).ToString("X4") + ",Y @ " + EffectiveAddress.ToString("X4") + " = " + Val.ToString("X2").Replace("\"", "\"\"");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Relative addressing (BCC)
                case 7:
                    BranchToAddress = 0;
                    Operand = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    if (CPU.ProcessorStatus[0] == 0)
                    {
                        BranchToAddress = ((MemoryAddress) + (Operand - ((Operand >> 7 & 1) * 256))) & 0xFFFF;
                        MemoryAddress++;
                        if (((MemoryAddress - 1) & 0xff00) != (BranchToAddress & 0xFF00))
                        {
                            // memorad = branchtoaddress+0x100;
                        }
                        else
                        {
                            MemoryAddress = BranchToAddress;
                        }
                    }
                    OutPutArray[0] = OpCodeName + " " + "$" + MemoryAddress.ToString("X4");
                    EffectiveAddress = MemoryAddress;
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    //BNE $FF3E
                    break;
                //Indexed indirect addressing (Indirect,X) 1, 16, 17, 18, 19, 75
                case 8:
                    Pointer = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    Pointer = (Pointer + CPU.IndexRegisterX) & 0xFF;
                    TempLow = CPU.CPUMemory[Pointer & 0xff];
                    TempHigh = CPU.CPUMemory[(Pointer + 1) & 0xff];
                    EffectiveAddress = TempHigh << 8 | TempLow;
                    OutPutArray[0] = OpCodeName + " " + "($" + CPU.CPUMemory[MemoryAddress - 1].ToString("X2") + ",X) @ " + (Pointer).ToString("X2") + " = " + EffectiveAddress.ToString("X4") + " = " + CPU.CPUMemory[EffectiveAddress & 0xFFFF].ToString("X2").Replace("\"", "\"\"");
                    //LDA ($2A,X) @ 2A = CDD4 = FF
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Indirect indexed addressing (Indirect),Y1, 16, 91, 92, 93, 75
                case 9:
                    Pointer = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempLow = CPU.CPUMemory[Pointer];
                    TempHigh = CPU.CPUMemory[(Pointer + 1) % 256];
                    EffectiveAddress = (TempHigh << 8 | TempLow) + CPU.IndexRegisterY;
                    //LDA($00),Y = 9209 @ 9209 = 00
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + "($" + Pointer.ToString("X2") + "),Y = " + (EffectiveAddress - CPU.IndexRegisterY).ToString("X4") + " @ " + EffectiveAddress.ToString("X4") + " = " + Val.ToString("X2").Replace("\"", "\"\"");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Absolute indirect addressing (JMP)
                case 10:
                    TempLow = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempHigh = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    EffectiveAddress = TempHigh << 8 | TempLow;
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + "$" + EffectiveAddress.ToString("X4");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    //BNE $FF3E
                    break;
                //Zero page X addressing
                case 11:
                    TempAdd = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    EffectiveAddress = (TempAdd + CPU.IndexRegisterX) & 0xFF;
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + ("$" + TempAdd.ToString("X2") + ",X @ " + EffectiveAddress.ToString("X2") + " = " + Val.ToString("X2")).Replace("\"", "\"\"");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Zero page Y addressing
                case 12:
                    TempAdd = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    EffectiveAddress = (TempAdd + CPU.IndexRegisterY) & 0xFF;
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                    }
                    OutPutArray[0] = OpCodeName + " " + ("$" + TempAdd.ToString("X2") + ",Y @ " + EffectiveAddress.ToString("X2") + " = " + Val.ToString("X2")).Replace("\"", "\"\""); ;
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Absolute indirect addressing (JMP)
                case 13:
                    TempLow = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempHigh = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    TempAdd = TempHigh << 8 | TempLow;

                    TempLow = CPU.CPUMemory[TempAdd];
                    if (((TempAdd & 0xFF) + 1) == 0x100) { TempAdd -= 0x100; }
                    TempHigh = CPU.CPUMemory[TempAdd + 1];
                    EffectiveAddress = TempHigh << 8 | TempLow;
                    EffectiveAddress = TempAdd;
                    if ((EffectiveAddress >= 0x2000 && EffectiveAddress <= 0x4007) || (EffectiveAddress >= 0x4000 && EffectiveAddress < 0x6000) || (GAME.SRAMPRSENT == 0 && EffectiveAddress >= 0x6000 && EffectiveAddress < 0x8000))
                    {
                        Val = 0xFF;
                    }
                    else
                    {
                        Val = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                        TempLow = CPU.CPUMemory[EffectiveAddress & 0xFFFF];
                        TempHigh = CPU.CPUMemory[EffectiveAddress + 1];
                        Val = TempHigh << 8 | TempLow;
                    }
                    OutPutArray[0] = OpCodeName + " " + "($" + EffectiveAddress.ToString("X4") + ") = " + Val.ToString("X4");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                //Accumaltor
                case 14:
                    OutPutArray[0] = OpCodeName + " " + "A";
                    break;
                //JSR
                case 15:
                    TempLow = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    MemoryAddress++;
                    TempHigh = CPU.CPUMemory[MemoryAddress & 0xFFFF];
                    EffectiveAddress = TempHigh << 8 | TempLow;
                    OutPutArray[0] = OpCodeName + " " + "$" + EffectiveAddress.ToString("X4");
                    OutPutEffectiveAddress = EffectiveAddress & 0xFFFF;
                    break;
                case 0:
                    break;
            }
            if (OutPutArray[0] == null)
            {
                OutPutArray[0] = OpCodeName;
            }
            OutPutArray[1] = OutPutEffectiveAddress.ToString().Trim();
            OutPutArray[2] = Val.ToString().Trim();
            return OutPutArray;
        }

        public static void GetTracedata()
        {
            StringBuilder ProccessorStatucStringBuilder = new StringBuilder();
            int[] ProccessorStatusArray = CPU.ProcessorStatus.Reverse().ToArray();
            foreach (int Flag in ProccessorStatusArray) { ProccessorStatucStringBuilder.Append(Flag); }
            string[] OutPutArray = Debugger.DecodeOpcode(CPU.ProgramCounter);
            if (TracePointer == 1000) { TracePointer = 0; }
            INSTRUCTIONCOUNT++;
            string[] NewTraceLine = new string[11];
            NewTraceLine[0] = (INSTRUCTIONCOUNT).ToString();
            NewTraceLine[1] = CPU.ProgramCounter.ToString("X4");
            NewTraceLine[2] = OutPutArray[3];
            NewTraceLine[3] = "" + OutPutArray[0];
            NewTraceLine[4] = "A:" + CPU.Accumulator.ToString("X2");
            NewTraceLine[5] = "X:" + CPU.IndexRegisterX.ToString("X2");
            NewTraceLine[6] = "Y:" + CPU.IndexRegisterY.ToString("X2");
            NewTraceLine[7] = "P:" + Convert.ToInt32(ProccessorStatucStringBuilder.ToString(), 2).ToString("X2");
            NewTraceLine[8] = "SP:" + CPU.StackPointer.ToString("X2");
            NewTraceLine[9] = "CYC:" + PPU.PPUCycleCount.ToString().PadLeft(3);
            NewTraceLine[10] = "SL:" + PPU.Scanlines;
            TraceList[TracePointer] = NewTraceLine;
            if (StopOnCount && NumberInstructionsToExecute != null && NumberInstructionsToExecute.Length > 0 && CountToStop < 0)
            {
                CountToStop = Convert.ToInt32(NumberInstructionsToExecute);
            }
            else
            {
                StopOnCount = false;
            }
            int CurrentEffectiveAddress = -1;
            CountToStop--;
            bool BreakPointReached = false;
            string[] TraceData = Debugger.DecodeOpcode(CPU.ProgramCounter);
            if (TraceData[1].Trim().Length > 1)
            {
                CurrentEffectiveAddress = Convert.ToInt32(TraceData[1].Trim());
            }

            if (StopOnCount)
            {
                if (CountToStop <= 0)
                {
                    StopLine = TraceList.Count;
                    BreakPointReached = true;
                    CountToStop = -1;
                    StopOnCount = false;
                    NES.PlayLoop = false;
                }
            }

            for (int i = 0; i < BreakPointList.Count; i++)
            {
                int MemoryLow = Convert.ToInt32(BreakPointList[i][2], 16);
                int MemoryHigh = Convert.ToInt32(BreakPointList[i][3], 16);
                if (MemoryHigh == 0) { MemoryHigh = MemoryLow; }
                if (BreakPointList[i][0] == "READ")
                {
                    if (CurrentEffectiveAddress >= MemoryLow && CurrentEffectiveAddress <= MemoryHigh)
                    {
                        StopLine = TraceList.Count;
                        BreakPointReached = true;
                        CountToStop = -1;
                        StopOnCount = false;
                        NES.PlayLoop = false;

                    }
                }
                else if (BreakPointList[i][0] == "WRITE" && Debugger.OpArray[CPU.CPUMemory[CPU.ProgramCounter]] != null && Debugger.OpArray[CPU.CPUMemory[CPU.ProgramCounter]].Item3)
                {
                    if (CurrentEffectiveAddress >= MemoryLow && CurrentEffectiveAddress <= MemoryHigh)
                    {
                        StopLine = TraceList.Count;
                        BreakPointReached = true;
                        CountToStop = -1;
                        StopOnCount = false;
                        NES.PlayLoop = false;
                    }
                }
                else if (BreakPointList[i][0] == "OPCODE")
                {
                    if (BreakPointList[i][4] == OutPutArray[3].Split(' ')[0])
                    {
                        StopLine = TraceList.Count;
                        BreakPointReached = true;
                        CountToStop = -1;
                        StopOnCount = false;
                        NES.PlayLoop = false;
                    }
                }
            }
            if (BreakPointReached)
            {
                NES.PlayLoop = false;
                DebuggerForm.Showdata();
            }
            TracePointer++;
        }

        public static void DecodeBreakpoints(List<string> BPList)
        {
            Debugger.BreakPointList.Clear();
            for (int i = 0; i < BPList.Count; i++)
            {
                string CurrentBreakPoint = BPList[i].ToString().ToUpper().Trim();
                string BreakType = CurrentBreakPoint.Split(':')[0].Trim();
                string[] BreakArray = CurrentBreakPoint.Split(' ');
                string BreakEnabled = BreakArray[BreakArray.Length - 1].Trim();
                string[] NewBreakPoint = new string[5];
                NewBreakPoint[0] = BreakType;
                NewBreakPoint[1] = BreakEnabled;
                if (BreakType == "WRITE")
                {
                    NewBreakPoint[2] = BreakArray[1].Split('-')[0];
                    if (BreakArray[1].Split('-')[0] != BreakArray[1].Split('-')[1])
                    {
                        NewBreakPoint[3] = BreakArray[1].Split('-')[1];
                    }
                }
                else if (BreakType == "READ")
                {
                    NewBreakPoint[2] = BreakArray[1].Split('-')[0];
                    if (BreakArray[1].Split('-')[0] != BreakArray[1].Split('-')[1])
                    {
                        NewBreakPoint[3] = BreakArray[1].Split('-')[1];
                    }
                }
                else if (BreakType == "OPCODE")
                {
                    NewBreakPoint[4] = BreakArray[1].Trim();
                }
                Debugger.BreakPointList.Add(NewBreakPoint);
            }
        }

        public static void AddNewLog(String LogPath)
        {
            string[] NewLogLine = new string[9];
            NewLogLine[0] = "False";
            NewLogLine[1] = System.IO.Path.GetFileNameWithoutExtension(LogPath);
            NewLogLine[8] = LogPath;
            if (!LogList.ContainsKey(NewLogLine[1]))
            {
                LogList.Add(NewLogLine[1], NewLogLine);
            }
        }
        public static void AddNewTestROM(String LogPath)
        {
            string[] NewTestROM = new string[11]; //
            NewTestROM[0] = "FALSE";
            NewTestROM[1] = System.IO.Path.GetFileNameWithoutExtension(LogPath);
            NewTestROM[8] = LogPath;
            NewTestROM[9] = "300000";
            NewTestROM[10] = "FALSE";
            if (!TestROMList.ContainsKey(NewTestROM[1]))
            {
                byte[] Header = new byte[16];
                using (FileStream HeaderFileStream = new FileStream(LogPath, FileMode.Open, FileAccess.Read))
                {
                    HeaderFileStream.Read(Header, 0, Header.Length);
                    HeaderFileStream.Close();
                }
                NewTestROM[2] = ((Header[7] >> 4) << 4) + (Header[6] >> 4).ToString();
                TestROMList.Add(NewTestROM[1], NewTestROM);
            }
        }

        public static void PrintLogList()
        {
            TextWriter LogListTextWriter = new StreamWriter("LogList.txt");
            List<string> LogListKeys = Debugger.LogList.Keys.ToList();
            for (int i = 0; i < LogListKeys.Count; i++)
            {
                LogListTextWriter.WriteLine("\"" + string.Join("\", \"", Debugger.LogList[LogListKeys[i]]) + "\"");
            }
            LogListTextWriter.Close();
        }
        public static void PrintTestROMList()
        {
            TextWriter TestROMTextWriter = new StreamWriter("TestROMList.txt");
            List<string> LogKeys = TestROMList.Keys.ToList();
            for (int i = 0; i < LogKeys.Count; i++)
            {
                TestROMTextWriter.WriteLine("\"" + string.Join("\", \"", TestROMList[LogKeys[i]]) + "\"");
            }
            TestROMTextWriter.Close();
        }

        public static void AddRomToLog(String LogKey, String ROMPath)
        {
            LogList[LogKey][2] = ROMPath;
            byte[] INESHeader = new byte[16];
            using (FileStream ROMFileStream = new FileStream(ROMPath, FileMode.Open, FileAccess.Read))
            {
                ROMFileStream.Read(INESHeader, 0, INESHeader.Length);
                ROMFileStream.Close();
            }
            LogList[LogKey][3] = ((INESHeader[7] >> 4) << 4) + (INESHeader[6] >> 4).ToString();
        }

        public static string AnalyzeLogComparison(List<string[]> SystemTrace, List<string[]> LogTrace)
        {
            int Trace1CycleCount = Convert.ToInt32(SystemTrace[1][9].Split(':')[1]);
            int Trace2CycleCount = Convert.ToInt32(LogTrace[(1)][9].Split(':')[1]);
            int CycleCountDifference = Math.Abs(Trace2CycleCount - Trace1CycleCount);
            int CycleCountDifference2 = (341 - Trace1CycleCount) + Trace2CycleCount;
            int CycleCountDifference3 = (341 - Trace2CycleCount) + Trace1CycleCount;
            string Trace1Execution = LogTrace[0][3].Trim();
            string Trace2Execution = SystemTrace[1][3].Trim();
            int MMC5204Difference = 0;
            int OpenBusDifference = 0;
            int InputReadDifference = 0;
            int PPU2002FlagDifference = 0;
            int MemoryAddress = 0;
            if (Trace2Execution.Contains("STA $"))
            {
                MemoryAddress = int.Parse(Trace2Execution.Split('$')[1].Split(' ')[0].Split(',')[0], System.Globalization.NumberStyles.HexNumber);

            }
            if (Trace2Execution.Contains(",Y ") && Trace2Execution.Contains("@"))
            {
                MemoryAddress = int.Parse(Trace2Execution.Split('@')[1].Trim().Split(' ')[0].Trim(), System.Globalization.NumberStyles.HexNumber);
            }
            if (Trace1Execution.Contains("2002"))
            {
                if (Trace1Execution.Contains("LDA"))
                {
                   PPU2002FlagDifference = Math.Abs(Convert.ToInt32(LogTrace[1][4].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][4].Trim().Split(':')[1], 16));
                }
                if (Trace1Execution.Contains("BIT"))
                {
                    PPU2002FlagDifference = Math.Abs(Convert.ToInt32(LogTrace[1][7].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][7].Trim().Split(':')[1], 16));
                }
            }
            
            if (Trace1Execution.Contains("5204"))
            {
                if (Trace1Execution.Contains("LDA"))
                {
                    MMC5204Difference = Math.Abs(Convert.ToInt32(LogTrace[1][4].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][4].Trim().Split(':')[1], 16));
                }
                if (Trace1Execution.Contains("BIT"))
                {
                    MMC5204Difference = Math.Abs(Convert.ToInt32(LogTrace[1][7].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][7].Trim().Split(':')[1], 16));
                }
            }
            
            if (Trace1Execution.Contains("4016"))
            {
                if (Trace1Execution.Contains("LDA"))
                {
                    InputReadDifference = Math.Abs(Convert.ToInt32(LogTrace[1][4].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][4].Trim().Split(':')[1], 16));
                }
            }
            
            if (Trace1Execution.Contains("2000") || Trace1Execution.Contains("2001"))
            {
                if (Trace1Execution.Contains("LDA"))
                {
                    OpenBusDifference = Math.Abs(Convert.ToInt32(LogTrace[1][4].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][4].Trim().Split(':')[1], 16));
                }
                if (Trace1Execution.Contains("BIT"))
                {
                    OpenBusDifference = Math.Abs(Convert.ToInt32(LogTrace[1][7].Trim().Split(':')[1], 16) - Convert.ToInt32(SystemTrace[1][7].Trim().Split(':')[1], 16));
                }
            }
            
            if (CycleCountDifference == 21 || CycleCountDifference2 == 21 || CycleCountDifference3 == 21)
            {
                return "Missed IRQ";
            }
            else if ((SystemTrace[0][1].Trim() == LogTrace[0][1].Trim()) && SystemTrace[1][2].Trim().Split(' ')[0] != LogTrace[1][2].Trim().Split(' ')[0])
            {
                return "Bank #";
            }
            else if (CycleCountDifference == 12 || CycleCountDifference2 == 12 || CycleCountDifference3 == 12)
            {
                return "Cycle Stealing";
            }
            else if (PPU2002FlagDifference == 32)
            {
                return "Sprite Overflow";
            }
            else if (PPU2002FlagDifference == 64)
            {
                return "Sprite Zero";
            }
            else if (PPU2002FlagDifference == 128)
            {
                return "VBL Flag";
            }
            else if (MMC5204Difference == 64)
            {
                return "MMC5 In Frame";
            }
            else if (MMC5204Difference == 128)
            {
                return "MMC5 IRQ Pending";
            }
            else if (InputReadDifference == 1)
            {
                return "Input Read";
            }
            else if (OpenBusDifference > 0)
            {
                return "Open Bus";
            }
            else if (Convert.ToInt32(SystemTrace[2][0].Trim()) >= 300000)
            {
                return "PASS";
            }
            else if (MemoryAddress >= 0x6000 && MemoryAddress <= 0x8000)
            {
                return "PRG RAM";
            }
            else if (SystemTrace[0][3].Trim() == LogTrace[0][3].Trim() && Trace2Execution != LogTrace[1][3].Trim())
            {
                return LogTrace[1][3].Trim();
            }
            else
            {
                return "UNKNOWN";
            }
        }

        public static Bitmap DrawNESPallette()
        {
            Bitmap PalletteBitmap = new Bitmap(256, 128);
            int xcoord = 0;
            int ycoord = 0;
            int PalletteMemoryLocation = 16128;
            for (int q2 = 0; q2 < 2; q2++)
            {
                for (int q3 = 0; q3 < 16; q3++)
                {
                    for (int q4 = 0; q4 < 16; q4++)
                    {
                        for (int q5 = 0; q5 < 16; q5++)
                        {
                            int ColorIndex = PPU.PPUMemory[PalletteMemoryLocation];
                            PalletteBitmap.SetPixel(xcoord + q4, ycoord + q5, Color.FromArgb(PPU.NESPallette[ColorIndex, 0], PPU.NESPallette[ColorIndex, 1], PPU.NESPallette[ColorIndex, 2]));
                        }
                    }
                    PalletteMemoryLocation++;
                    xcoord = xcoord + 16;
                }
                ycoord = ycoord + 16;
                xcoord = 0;
            }            
            return PalletteBitmap;
        }

        public static Bitmap DrawNESSprites()
        {
            Bitmap SpriteBitmap = new Bitmap(256, 240);
            int xoffset = 0;
            int yoffset = 0;
            int OAMIndex = 1;
            int MemoryAddress = 0;
            for (int xcor = 0; xcor < 8; xcor++)
            {
                for (int ycor = 0; ycor < 8; ycor++)
                {
                    if ((PPU.PPUCTRL2000 >> 5 & 1) == 0)
                    {
                        //8 bit sprite
                        MemoryAddress = ((PPU.OAMMEM[OAMIndex]) * 16) + ((PPU.PPUCTRL2000 >> 3 & 1) * 0x1000);
                    }
                    else
                    {
                        //16 bit sprite
                        if ((PPU.OAMMEM[OAMIndex] & 1) == 0)
                        {
                            MemoryAddress = ((PPU.OAMMEM[OAMIndex]) * 16);
                        }
                        else
                        {
                            MemoryAddress = ((((PPU.OAMMEM[OAMIndex]) - 1) * 16) + 0x1000);
                        }
                    }
                    for (int YCoordinate = 0; YCoordinate < (8 + (8 * ((PPU.PPUCTRL2000 >> 5 & 1)))); YCoordinate++)
                    {
                        if (YCoordinate > 7) { MemoryAddress++; }
                        int LowTileValue = PPU.PPUMemory[MemoryAddress];
                        int HighTileValue = PPU.PPUMemory[MemoryAddress + 8];
                        for (int XCoordinate = 0; XCoordinate < 8; XCoordinate++)
                        {
                            int ColorIndex = PPU.PPUMemory[(((LowTileValue >> XCoordinate & 1) | (HighTileValue >> XCoordinate & 1) << 1) + ((PPU.OAMMEM[OAMIndex + 1] & 3) * 4)) + 16128 + 16];
                            SpriteBitmap.SetPixel(7 - XCoordinate + (yoffset), YCoordinate + (xoffset), Color.FromArgb(PPU.NESPallette[ColorIndex, 0], PPU.NESPallette[ColorIndex, 1], PPU.NESPallette[ColorIndex, 2]));
                        }
                        MemoryAddress++;
                    }
                    OAMIndex += 4;
                    yoffset += 16;
                    if (yoffset == ((16 * 16)))
                    {
                        yoffset = 0;
                        xoffset += 32;
                    }
                }
            }            
            return SpriteBitmap;
        }

        public static Bitmap DrawPatternTables()
        {
            int CurrentTile = 0;
            int[,] PatternArray = new int[512, 16];
            Bitmap PatternBitmap = new Bitmap(256, 128);
            int xoffset = 0;
            int yoffset = 0;
            for (int MainTileIndex = 0; MainTileIndex < 512; MainTileIndex++)
            {
                if ((MainTileIndex > 0) && (MainTileIndex % 16 == 0))
                {
                    xoffset = 0;
                    yoffset = yoffset + 8;
                    if (MainTileIndex == 256)
                    {
                        yoffset = 0;
                        xoffset = 128;
                    }
                    if (MainTileIndex > 256)
                    {
                        xoffset = 128;
                    }
                }
                else if (MainTileIndex > 0)
                {
                    xoffset += 8;
                }
                CurrentTile = MainTileIndex * 16;
                for (int i = 0; i < 8; i++)
                {
                    int FirstTile = PPU.PPUMemory[CurrentTile + i];
                    int SecondTile = PPU.PPUMemory[CurrentTile + i + 8];
                    for (int t = 0; t < 8; t++)
                    {
                        int FirstTileBit = (FirstTile >> 7) & 1;
                        int SecondTileBit = (SecondTile >> 7) & 1;
                        int TileValue = SecondTileBit << 1 | FirstTileBit;
                        if (TileValue == 0)
                        {
                            PatternBitmap.SetPixel(xoffset + t, yoffset + i, Color.Black);
                        }
                        else if (TileValue == 1)
                        {
                            PatternBitmap.SetPixel(xoffset + t, yoffset + i, Color.White);
                        }
                        else if (TileValue == 2)
                        {
                            PatternBitmap.SetPixel(xoffset + t, yoffset + i, Color.Blue);
                        }
                        else if (TileValue == 3)
                        {
                            PatternBitmap.SetPixel(xoffset + t, yoffset + i, Color.Red);
                        }
                        FirstTile = FirstTile << 1;
                        SecondTile = SecondTile << 1;
                        PatterntableTiles[MainTileIndex, i, t] = TileValue;
                    }
                }
            }
            return PatternBitmap;
        }

        public static Bitmap DrawNameTable(int StartIndex)
        {
            Bitmap NameTableBitmap = new Bitmap(256, 240);
           
            int Endindex = StartIndex + 0x400;
            int xoffset = 0;
            int yoffset = 0;

            for (int i = StartIndex; i < Endindex; i++)
            {
                if ((i > 0) && (i % 32 == 0))
                {
                    xoffset = 0;
                    yoffset = yoffset + 8;
                }
                else if (i > 0)
                {
                    xoffset += 8;
                }
                int CurrentTile = PPU.PPUMemory[i];
                if (yoffset < 240)
                {
                    int at_bits = ((((PPU.PPUMemory[(0x23C0 | (i & 0x0C00) | ((i >> 4) & 0x38) | ((i >> 2) & 0x07))])) >> 2 * ((((((i >> 5)) & 0x1F)) & 0x02) | ((((i & 0x1F)) & 0x02) >> 1))) & 3);
                    for (int m2 = 0; m2 < 8; m2++)
                    {
                        for (int m3 = 0; m3 < 8; m3++)
                        {
                            int ColorIndex = PPU.PPUMemory[PatterntableTiles[((PPU.PPUMemory[i] + (((PPU.PPUCTRL2000 >> 4) & 1) * 256))), m2, m3] + (at_bits * 4) + 16128] & 0x3fff;
                            NameTableBitmap.SetPixel((m3) + xoffset, (m2) + yoffset, Color.FromArgb(PPU.NESPallette[ColorIndex, 0], PPU.NESPallette[ColorIndex, 1], PPU.NESPallette[ColorIndex, 2]));
                        }
                    }
                }
            }
            return NameTableBitmap;
        }

        public static void LoadLogList()
        {
            if (File.Exists("LogList.txt"))
            {
                List<string> LogListFile = File.ReadAllLines("LogList.txt").ToList();
                Debugger.LogList = new Dictionary<string, string[]>();
                Dictionary<string, string> INESDictionary = new Dictionary<string, string>();
                for (int i = 0; i < LogListFile.Count; i++)
                {
                    TextFieldParser LogListParser = new TextFieldParser(new StringReader(LogListFile[i]));
                    LogListParser.HasFieldsEnclosedInQuotes = true;
                    LogListParser.SetDelimiters(",");
                    string[] CurrentLogLine;
                    while (!LogListParser.EndOfData)
                    {
                        CurrentLogLine = LogListParser.ReadFields();
                        Debugger.LogList.Add(CurrentLogLine[1], CurrentLogLine);
                    }
                    LogListParser.Close();
                }
            }
        }

    }
}
