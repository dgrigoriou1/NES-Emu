namespace NesPlayer
{
    class CPU
    {
        public static int[] CPUMemory = new int[0x10000];
        public static int[][] CycleArray = new int[275][];
        public static int CycleNumber = 0;
        public static int CurrentOpCode = 0;
        public static int TempValue = 0;
        public static int TempValue2 = 0;
        public static int IRQDelay = 0;
        public static int EffectiveAddress = 0;
        public static int AddressLow = 0;
        public static int AddressHigh = 0;
        public static int Pointer = 0;
        public static int Operand = 0;
        public static int PCL = 0;
        public static int PCH = 0;
        private static int _ProgramCounter;
        public static int ProgramCounter
        {
            get { return _ProgramCounter; }
            set { _ProgramCounter = value; }
        }
        private static int _StackPointer;
        public static int StackPointer
        {
            get { return _StackPointer; }
            set { _StackPointer = value; }
        }
        private static int _TotalCPUCycles;
        public static int TotalCPUCycles
        {
            get { return _TotalCPUCycles; }
            set { _TotalCPUCycles = value; }
        }

        private static int _Accumulator;
        public static int Accumulator
        {
            get { return _Accumulator; }
            set { _Accumulator = value; }
        }

        private static int _IndexRegisterX;
        public static int IndexRegisterX
        {
            get { return _IndexRegisterX; }
            set { _IndexRegisterX = value; }
        }

        private static int _IndexRegisterY;
        public static int IndexRegisterY
        {
            get { return _IndexRegisterY; }
            set { _IndexRegisterY = value; }
        }

        // Bit No.       7   6   5   4   3   2   1   0
        //               n   v       B   D   I   z   C
        private static int[] _ProcessorStatus = new int[8];
        public static int[] ProcessorStatus
        {
            get { return _ProcessorStatus; }
            set { _ProcessorStatus = value; }
        }
       
        public static void Execute()
        {           
            switch (CurrentOpCode)
            {
                //ADC
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   ADC #Oper           |    69   |    2    |    2     |
                //|  Zero Page     |   ADC Oper            |    65   |    2    |    3     |
                //|  Zero Page,X   |   ADC Oper,X          |    75   |    2    |    4     |
                //|  Absolute      |   ADC Oper            |    6D   |    3    |    4     |
                //|  Absolute,X    |   ADC Oper,X          |    7D   |    3    |    4*    |
                //|  Absolute,Y    |   ADC Oper,Y          |    79   |    3    |    4*    |
                //|  (Indirect,X)  |   ADC (Oper,X)        |    61   |    2    |    6     |
                //|  (Indirect),Y  |   ADC (Oper),Y        |    71   |    2    |    5*    |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x75:
                case 0x69:
                case 0x65:
                case 0x6D:
                case 0x7D:
                case 0x79:
                case 0x61:
                case 0x71:
                    TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    TempValue2 = (Accumulator + TempValue + ProcessorStatus[0]);
                    ProcessorStatus[0] = ((TempValue2 >> 8) & 1);
                    TempValue2 &= 0xFF;
                    ProcessorStatus[6] = (((~(Accumulator ^ TempValue) & (Accumulator ^ TempValue2) & 0x80) >> 7) & 1);
                    Accumulator = TempValue2;
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    break;
                //AND
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   AND #Oper           |    29   |    2    |    2     |
                //|  Zero Page     |   AND Oper            |    25   |    2    |    3     |
                //|  Zero Page,X   |   AND Oper,X          |    35   |    2    |    4     |
                //|  Absolute      |   AND Oper            |    2D   |    3    |    4     |
                //|  Absolute,X    |   AND Oper,X          |    3D   |    3    |    4*    |
                //|  Absolute,Y    |   AND Oper,Y          |    39   |    3    |    4*    |
                //|  (Indirect,X)  |   AND (Oper,X)        |    21   |    2    |    6     |
                //|  (Indirect,Y)  |   AND (Oper),Y        |    31   |    2    |    5     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x25:
                case 0x29:
                case 0x35:
                case 0x2D:
                case 0x3D:
                case 0x39:
                case 0x21:
                case 0x31:
                    Accumulator &= ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    break;
                //BIT
                case 0x24:
                case 0x2C:
                    TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    ProcessorStatus[6] = (TempValue >> 6) & 1;
                    ProcessorStatus[1] = ~(((TempValue & Accumulator) | (~(TempValue & Accumulator) + 1)) >> 31) & 1;
                    break;
                //CMP
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   CMP #Oper           |    C9   |    2    |    2     |
                //|  Zero Page     |   CMP Oper            |    C5   |    2    |    3     |
                //|  Zero Page,X   |   CMP Oper,X          |    D5   |    2    |    4     |
                //|  Absolute      |   CMP Oper            |    CD   |    3    |    4     |
                //|  Absolute,X    |   CMP Oper,X          |    DD   |    3    |    4*    |
                //|  Absolute,Y    |   CMP Oper,Y          |    D9   |    3    |    4*    |
                //|  (Indirect,X)  |   CMP (Oper,X)        |    C1   |    2    |    6     |
                //|  (Indirect),Y  |   CMP (Oper),Y        |    D1   |    2    |    5*    |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xC5:
                case 0xC9:
                case 0xD5:
                case 0xCD:
                case 0xDD:
                case 0xD9:
                case 0xC1:
                case 0xD1:
                    TempValue = Accumulator - ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[0] = (~(TempValue >> 31) & 1);
                    TempValue &= 0xFF;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    break;
                //CPX
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   CPX *Oper           |    E0   |    2    |    2     |
                //|  Zero Page     |   CPX Oper            |    E4   |    2    |    3     |
                //|  Absolute      |   CPX Oper            |    EC   |    3    |    4     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xE0:
                case 0xE4:
                case 0xEC:
                    TempValue = IndexRegisterX - ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[0] = (~(TempValue >> 31) & 1);
                    TempValue &= 0xFF;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    break;
                //CPY
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   CPY *Oper           |    C0   |    2    |    2     |
                //|  Zero Page     |   CPY Oper            |    C4   |    2    |    3     |
                //|  Absolute      |   CPY Oper            |    CC   |    3    |    4     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xC0:
                case 0xC4:
                case 0xCC:
                    TempValue = IndexRegisterY - ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[0] = (~(TempValue >> 31) & 1);
                    TempValue &= 0xFF;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    break;
                //EOR
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   EOR #Oper           |    49   |    2    |    2     |
                //|  Zero Page     |   EOR Oper            |    45   |    2    |    3     |
                //|  Zero Page,X   |   EOR Oper,X          |    55   |    2    |    4     |
                //|  Absolute      |   EOR Oper            |    4D   |    3    |    4     |
                //|  Absolute,X    |   EOR Oper,X          |    5D   |    3    |    4*    |
                //|  Absolute,Y    |   EOR Oper,Y          |    59   |    3    |    4*    |
                //|  (Indirect,X)  |   EOR (Oper,X)        |    41   |    2    |    6     |
                //|  (Indirect),Y  |   EOR (Oper),Y        |    51   |    2    |    5*    |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x49:
                case 0x45:
                case 0x55:
                case 0x4D:
                case 0x5D:
                case 0x59:
                case 0x41:
                case 0x51:
                    Accumulator ^= ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    break;
                //LDA
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   LDA #Oper           |    A9   |    2    |    2     |
                //|  Zero Page     |   LDA Oper            |    A5   |    2    |    3     |
                //|  Zero Page,X   |   LDA Oper,X          |    B5   |    2    |    4     |
                //|  Absolute      |   LDA Oper            |    AD   |    3    |    4     |
                //|  Absolute,X    |   LDA Oper,X          |    BD   |    3    |    4*    |
                //|  Absolute,Y    |   LDA Oper,Y          |    B9   |    3    |    4*    |
                //|  (Indirect,X)  |   LDA (Oper,X)        |    A1   |    2    |    6     |
                //|  (Indirect),Y  |   LDA (Oper),Y        |    B1   |    2    |    5*    |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xA9:
                case 0xA5:
                case 0xB5:
                case 0xAD:
                case 0xBD:
                case 0xB9:
                case 0xA1:
                case 0xB1:
                    Accumulator = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    break;
                //LDX
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   LDX #Oper           |    A2   |    2    |    2     |
                //|  Zero Page     |   LDX Oper            |    A6   |    2    |    3     |
                //|  Zero Page,Y   |   LDX Oper,Y          |    B6   |    2    |    4     |
                //|  Absolute      |   LDX Oper            |    AE   |    3    |    4     |
                //|  Absolute,Y    |   LDX Oper,Y          |    BE   |    3    |    4*    |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xA2:
                case 0xA6:
                case 0xB6:
                case 0xAE:
                case 0xBE:
                    IndexRegisterX = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = ((IndexRegisterX >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterX | (~IndexRegisterX + 1)) >> 31) & 1;
                    break;
                //LDY
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   LDY #Oper           |    A0   |    2    |    2     |
                //|  Zero Page     |   LDY Oper            |    A4   |    2    |    3     |
                //|  Zero Page,X   |   LDY Oper,X          |    B4   |    2    |    4     |
                //|  Absolute      |   LDY Oper            |    AC   |    3    |    4     |
                //|  Absolute,X    |   LDY Oper,X          |    BC   |    3    |    4*    |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xA0:
                case 0xA4:
                case 0xB4:
                case 0xAC:
                case 0xBC:
                    IndexRegisterY = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = ((IndexRegisterY >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterY | (~IndexRegisterY + 1)) >> 31) & 1;
                    break;
                //ORA
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   ORA #Oper           |    09   |    2    |    2     |
                //|  Zero Page     |   ORA Oper            |    05   |    2    |    3     |
                //|  Zero Page,X   |   ORA Oper,X          |    15   |    2    |    4     |
                //|  Absolute      |   ORA Oper            |    0D   |    3    |    4     |
                //|  Absolute,X    |   ORA Oper,X          |    1D   |    3    |    4*    |
                //|  Absolute,Y    |   ORA Oper,Y          |    19   |    3    |    4*    |
                //|  (Indirect,X)  |   ORA (Oper,X)        |    01   |    2    |    6     |
                //|  (Indirect),Y  |   ORA (Oper),Y        |    11   |    2    |    5     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x09:
                case 0x05:
                case 0x15:
                case 0x0D:
                case 0x1D:
                case 0x19:
                case 0x01:
                case 0x11:
                    Accumulator = Accumulator | ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    break;
                //SBC
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Immediate     |   SBC #Oper           |    E9   |    2    |    2     |
                //|  Zero Page     |   SBC Oper            |    E5   |    2    |    3     |
                //|  Zero Page,X   |   SBC Oper,X          |    F5   |    2    |    4     |
                //|  Absolute      |   SBC Oper            |    ED   |    3    |    4     |
                //|  Absolute,X    |   SBC Oper,X          |    FD   |    3    |    4*    |
                //|  Absolute,Y    |   SBC Oper,Y          |    F9   |    3    |    4*    |
                //|  (Indirect,X)  |   SBC (Oper,X)        |    E1   |    2    |    6     |
                //|  (Indirect),Y  |   SBC (Oper),Y        |    F1   |    2    |    5     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xE9:
                case 0xE5:
                case 0xF5:
                case 0xED:
                case 0xFD:
                case 0xF9:
                case 0xE1:
                case 0xF1:
                    TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    TempValue2 = Accumulator;
                    Accumulator = Accumulator - (TempValue) - (1 - ProcessorStatus[0]);
                    ProcessorStatus[0] = (~(Accumulator >> 31) & 1);
                    Accumulator = Accumulator & 0xFF;
                    ProcessorStatus[6] = (((~(Accumulator ^ TempValue) & (Accumulator ^ TempValue2) & 0x80) >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    break;
                //ASL
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Accumulator   |   ASL A               |    0A   |    1    |    2     |
                //|  Zero Page     |   ASL Oper            |    06   |    2    |    5     |
                //|  Zero Page,X   |   ASL Oper,X          |    16   |    2    |    6     |
                //|  Absolute      |   ASL Oper            |    0E   |    3    |    6     |
                //|  Absolute, X   |   ASL Oper,X          |    1E   |    3    |    7     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x0A:
                case 0x06:
                case 0x16:
                case 0x0E:
                case 0x1E:
                    TempValue = (CPUMemory[EffectiveAddress] * 2);
                    ProcessorStatus[0] = (TempValue >> 8) & 1;
                    TempValue &= 0xFF;
                    WriteToCPUMemory(EffectiveAddress, TempValue);
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    break;
                //DEC
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Zero Page     |   DEC Oper            |    C6   |    2    |    5     |
                //|  Zero Page,X   |   DEC Oper,X          |    D6   |    2    |    6     |
                //|  Absolute      |   DEC Oper            |    CE   |    3    |    6     |
                //|  Absolute,X    |   DEC Oper,X          |    DE   |    3    |    7     |
                //+----------------+-----------------------+---------+---------+----------/
                case 0xC6:
                case 0xD6:
                case 0xCE:
                case 0xDE:
                    TempValue = (CPUMemory[EffectiveAddress&0xFFFF] - 1) & 0xFF;
                    WriteToCPUMemory(EffectiveAddress & 0xFFFF, TempValue);
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    break;
                //INC
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Zero Page     |   INC Oper            |    E6   |    2    |    5     |
                //|  Zero Page,X   |   INC Oper,X          |    F6   |    2    |    6     |
                //|  Absolute      |   INC Oper            |    EE   |    3    |    6     |
                //|  Absolute,X    |   INC Oper,X          |    FE   |    3    |    7     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0xE6:
                case 0xF6:
                case 0xEE:
                case 0xFE:
                    TempValue = (CPUMemory[EffectiveAddress & 0xFFFF] + 1) & 0xFF;
                    WriteToCPUMemory(EffectiveAddress & 0xFFFF, TempValue);
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    break;
                //LSR
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Accumulator   |   LSR A               |    4A   |    1    |    2     |
                //|  Zero Page     |   LSR Oper            |    46   |    2    |    5     |
                //|  Zero Page,X   |   LSR Oper,X          |    56   |    2    |    6     |
                //|  Absolute      |   LSR Oper            |    4E   |    3    |    6     |
                //|  Absolute,X    |   LSR Oper,X          |    5E   |    3    |    7     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x4A:
                case 0x46:
                case 0x56:
                case 0x4E:
                case 0x5E:
                    ProcessorStatus[0] = CPUMemory[EffectiveAddress & 0xFFFF] % 2;
                    CPUMemory[EffectiveAddress & 0xFFFF] = (CPUMemory[EffectiveAddress & 0xFFFF] >> 1);
                    ProcessorStatus[7] = ((CPUMemory[EffectiveAddress & 0xFFFF] >> 7) & 1);
                    ProcessorStatus[1] = ~((CPUMemory[EffectiveAddress & 0xFFFF] | (~CPUMemory[EffectiveAddress & 0xFFFF] + 1)) >> 31) & 1;
                    break;
                //ROL
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Accumulator   |   ROL A               |    2A   |    1    |    2     |
                //|  Zero Page     |   ROL Oper            |    26   |    2    |    5     |
                //|  Zero Page,X   |   ROL Oper,X          |    36   |    2    |    6     |
                //|  Absolute      |   ROL Oper            |    2E   |    3    |    6     |
                //|  Absolute,X    |   ROL Oper,X          |    3E   |    3    |    7     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x2A:
                    Accumulator = (Accumulator * 2) + ProcessorStatus[0];
                    ProcessorStatus[0] = ((Accumulator >> 8) & 1);
                    Accumulator &= 0xFF;
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    break;
                case 0x26:
                case 0x36:
                case 0x2E:
                case 0x3E:
                    TempValue = (ReadCPUMemory(EffectiveAddress & 0xFFFF) * 2) + ProcessorStatus[0];
                    ProcessorStatus[0] = ((TempValue >> 8) & 1);
                    TempValue &= 0xFF;
                    CPUMemory[EffectiveAddress & 0xFFFF] = TempValue;
                    ProcessorStatus[7] = (TempValue >> 7) & 1;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    break;
                //ROR
                //+----------------+-----------------------+---------+---------+----------+
                //| Addressing Mode| Assembly Language Form| OP CODE |No. Bytes|No. Cycles|
                //+----------------+-----------------------+---------+---------+----------+
                //|  Accumulator   |   ROR A               |    6A   |    1    |    2     |
                //|  Zero Page     |   ROR Oper            |    66   |    2    |    5     |
                //|  Zero Page,X   |   ROR Oper,X          |    76   |    2    |    6     |
                //|  Absolute      |   ROR Oper            |    6E   |    3    |    6     |
                //|  Absolute,X    |   ROR Oper,X          |    7E   |    3    |    7     |
                //+----------------+-----------------------+---------+---------+----------+
                case 0x6A:
                    ProcessorStatus[7] = ProcessorStatus[0];
                    TempValue = Accumulator + (ProcessorStatus[0] * 0x100);
                    ProcessorStatus[0] = TempValue & 1;
                    TempValue &= ~1;
                    Accumulator = (TempValue / 2);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    break;
                case 0x66:
                case 0x76:
                case 0x6E:
                case 0x7E:
                    ProcessorStatus[7] = ProcessorStatus[0];
                    TempValue = (CPUMemory[EffectiveAddress & 0xFFFF] + (ProcessorStatus[0] * 0x100));
                    ProcessorStatus[0] = TempValue & 1;
                    TempValue &= ~1;
                    TempValue = TempValue / 2;
                    CPUMemory[EffectiveAddress & 0xFFFF] = TempValue;
                    ProcessorStatus[1] = ~((TempValue | (~TempValue + 1)) >> 31) & 1;
                    break;
            }
        }
       

        public static int prev = 0;
        public static void RunCPU()
        {
            if (CycleArray[CurrentOpCode] == null) { CurrentOpCode = 0; }
            
            switch (CycleArray[CurrentOpCode][CycleNumber])
            {
                //fetch opcode, increment PC
                case 1:                 
                    prev = CurrentOpCode;
                    CurrentOpCode = CPUMemory[ProgramCounter & 0xFFFF];
                    CycleNumber++;
                    ProgramCounter++;
                    break;
                //fetch value, increment PC
                case 2:
                    TempValue = CPUMemory[ProgramCounter];
                    EffectiveAddress = ProgramCounter;
                    Execute();
                    CycleNumber = 0;
                    ProgramCounter++;
                    break;
                //fetch low address byte, increment PC
                case 3:
                    AddressLow = CPUMemory[ProgramCounter];
                    ProgramCounter++;
                    CycleNumber++;
                    break;
                //fetch high address byte, increment PC
                case 4:
                    AddressHigh = CPUMemory[ProgramCounter];
                    EffectiveAddress = AddressHigh << 8 | AddressLow;
                    ProgramCounter++;
                    CycleNumber++;
                    break;
                //fetch operand, increment PC BCS
                case 41:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[0] != 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //fetch operand, increment PC BEQ
                case 42:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[1] == 1) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //fetch operand, increment PC BMI
                case 43:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[7] != 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //fetch operand, increment PC BNE
                case 44:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[1] == 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //fetch operand, increment PC BPL
                case 45:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[7] == 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //fetch operand, increment PC BVC
                case 46:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[6] == 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //fetch operand, increment PC BVS
                case 47:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[6] != 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;
                //Read Value, Opeartion
                case 75:
                    Execute();
                    CycleNumber = 0;
                    break;
                //If branch is taken, add operand to PCL. Otherwise increment PC.
                case 39:
                    EffectiveAddress = ((ProgramCounter) + (Operand - ((Operand >> 7 & 1) * 0x100))) & 0xFFFF;
                    ProgramCounter++;
                    if (((ProgramCounter - 1) & 0xFF00) != (EffectiveAddress & 0xFF00))
                    {
                        CycleNumber++;
                    }
                    else
                    {
                        ProgramCounter = EffectiveAddress;
                        CycleNumber = 0;
                    }
                    break;
                //fetch high address byte, increment PC
                case 79:
                    AddressHigh = CPUMemory[ProgramCounter];
                    ProgramCounter = AddressHigh << 8 | AddressLow;
                    CycleNumber = 0;
                    break;
                //read from effective address
                case 5:
                    //val = ReadCPUMemory(tempadd);
                    CycleNumber++;
                    break;
                //write the value back to effective address,and do the operation on it
                case 6:
                    CycleNumber++;
                    break;
                //write the new value to effective address
                case 7:
                    Execute();
                    CycleNumber = 0;
                    break;
                //write register to effective address Y
                case 8:
                    WriteToCPUMemory(EffectiveAddress, IndexRegisterY);
                    break;
                //fetch address, increment PC
                case 9:
                    EffectiveAddress = CPUMemory[ProgramCounter];
                    CycleNumber++;
                    ProgramCounter++;
                    break;
                //read from address, add index registerX to it
                case 10:
                    EffectiveAddress = (EffectiveAddress + IndexRegisterX) & 0xFF;
                    CycleNumber++;
                    break;
                //read from address, add index registerY to it
                case 11:
                    EffectiveAddress = (EffectiveAddress + IndexRegisterY) & 0xFF;
                    CycleNumber++;
                    break;
                //fetch high byte of address, add index registerX to low address byte,increment PC
                case 12:
                    AddressHigh = CPUMemory[ProgramCounter];
                    EffectiveAddress = (AddressHigh << 8 | ((AddressLow + IndexRegisterX) & 0xFF)) & 0xFFFF;
                    ProgramCounter++;
                    CycleNumber++;
                    break;
                //fetch high byte of address, add index registerY to low address byte,increment PC
                case 13:
                    AddressHigh = CPUMemory[ProgramCounter];
                    EffectiveAddress = (AddressHigh << 8 | ((AddressLow + IndexRegisterY) & 0xFF)) & 0xFFFF;
                    ProgramCounter++;
                    CycleNumber++;
                    break;
                //read from effective address, fix the high byte of effective address IndexRegisterX
                case 14:
                    if ((AddressLow + IndexRegisterX) > 0xFF)
                    {
                        TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                        EffectiveAddress += 0x100;
                        CycleNumber++;
                    }
                    else
                    {
                        Execute();
                        CycleNumber = 0;
                    }
                    break;

                //read from effective address, fix the high byte of effective address IndexRegisterY
                case 15:
                    if ((AddressLow + IndexRegisterY) > 0xFF)
                    {
                        TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                        EffectiveAddress += 0x100;
                        CycleNumber++;
                    }
                    else
                    {
                        Execute();
                        CycleNumber = 0;
                    }
                    break;
                //fetch pointer address, increment PC
                case 16:
                    Pointer = CPUMemory[ProgramCounter];
                    ProgramCounter++;
                    CycleNumber++;
                    break;
                //pointer    R  read from the address, add X to it
                case 17:
                    Pointer = (Pointer + IndexRegisterX) & 0xFF;
                    CycleNumber++;
                    break;
                //pointer+X   R  fetch effective address low
                case 18:
                    AddressLow = CPUMemory[Pointer & 0xFF];
                    CycleNumber++;
                    break;
                //pointer+X+1  R  fetch effective address high
                case 19:
                    AddressHigh = CPUMemory[(Pointer + 1) & 0xFF];
                    EffectiveAddress = AddressHigh << 8 | AddressLow;
                    CycleNumber++;
                    break;
                //read next instruction byte (and throw it away), increment PC
                case 20:
                    CycleNumber++;
                    ProgramCounter++;
                    break;
                //push PCH on stack (with B flag set), decrement S
                case 21:
                    CPUMemory[StackPointer + 0x100] = (ProgramCounter) >> 8;
                    StackPointer--;
                    StackPointer &= 0xFF;
                    CycleNumber++;
                    break;
                //push PCL on stack, decrement S
                case 22:
                    CPUMemory[StackPointer + 0x100] = (ProgramCounter) & 0xFF;
                    StackPointer--;
                    StackPointer &= 0xFF;
                    CycleNumber++;
                    break;
                //push P on stack, decrement S
                case 23:
                    ProcessorStatus[5] = 1;
                    ProcessorStatus[4] = 0;
                    if (ProcessorStatus[7] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 7; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 7); }
                    if (ProcessorStatus[6] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 6; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 6); }
                    if (ProcessorStatus[5] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 5; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 5); }
                   // if (ProcessorStatus[4] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 4; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 4); }
                    if (ProcessorStatus[3] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 3; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 3); }
                    if (ProcessorStatus[2] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 2; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 2); }
                    if (ProcessorStatus[1] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 1; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 1); }
                    if (ProcessorStatus[0] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 0; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 0); }
                    CPUMemory[StackPointer + 0x100] |= 1 << 4;
                    StackPointer--;
                    StackPointer &= 0xFF;
                    ProcessorStatus[2] = 1;
                    CycleNumber++;
                    break;
                //fetch PCL
                case 24:
                    PCL = CPUMemory[65534];
                    CycleNumber++;
                    break;
                //fetch PCH
                case 25:
                    PCH = CPUMemory[65535];
                    ProgramCounter = PCH << 8 | PCL;
                    CycleNumber = 0;
                    break;
                //read next instruction byte (and throw it away)
                case 26:
                    CycleNumber++;
                    break;
                //increment S
                case 27:
                    StackPointer++;
                    StackPointer &= 0xFF;
                    CycleNumber++;
                    break;
                //pull P from stack, increment S
                case 28:
                    ProcessorStatus[7] = (CPUMemory[StackPointer + 0x100] >> 7) & 1;
                    ProcessorStatus[6] = (CPUMemory[StackPointer + 0x100] >> 6) & 1;
                    ProcessorStatus[5] = (CPUMemory[StackPointer + 0x100] >> 5) & 1;
                    ProcessorStatus[4] = 0;
                    ProcessorStatus[3] = (CPUMemory[StackPointer + 0x100] >> 3) & 1;
                    ProcessorStatus[2] = (CPUMemory[StackPointer + 0x100] >> 2) & 1;
                    ProcessorStatus[1] = (CPUMemory[StackPointer + 0x100] >> 1) & 1;
                    ProcessorStatus[0] = CPUMemory[StackPointer + 0x100] & 1;
                    StackPointer++;
                    StackPointer &= 0xFF;
                    CycleNumber++;
                    break;
                //pull PCL from stack, increment S
                case 29:
                    PCL = CPUMemory[StackPointer + 0x100];
                    StackPointer++;
                    StackPointer &= 0xFF;
                    CycleNumber++;
                    break;
                //pull PCH from stack
                case 30:
                    PCH = CPUMemory[StackPointer + 0x100];
                    ProgramCounter = (PCH << 8 | PCL);
                    CycleNumber = 0;
                    break;
                //increment PC
                case 31:
                    ProgramCounter++;
                    CycleNumber = 0;
                    break;
                //push register on stack, decrement S A
                case 32:
                    CPUMemory[StackPointer + 0x100] = Accumulator;
                    StackPointer--;
                    StackPointer &= 0xFF;
                    CycleNumber = 0;
                    break;
                //push register on stack, decrement S P
                case 33:
                    
                    if (ProcessorStatus[7] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 7; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 7); }
                    if (ProcessorStatus[6] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 6; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 6); }
                    CPUMemory[StackPointer + 0x100] |= 1 << 5;
                    CPUMemory[StackPointer + 0x100] |= 1 << 4;
                    if (ProcessorStatus[3] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 3; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 3); }
                    if (ProcessorStatus[2] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 2; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 2); }
                    if (ProcessorStatus[1] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 1; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 1); }
                    if (ProcessorStatus[0] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 0; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 0); }
                    ProcessorStatus[4] = 0;
                    StackPointer--;
                    StackPointer &= 0xFF;
                    CycleNumber = 0;
                    break;
                //pull register from stack A
                case 34:
                    Accumulator = CPUMemory[StackPointer + 0x100];
                    ProcessorStatus[7] = (Accumulator >> 7) & 1;
                    if (Accumulator == 0) { ProcessorStatus[1] = 1; } else { ProcessorStatus[1] = 0; }
                    CycleNumber = 0;
                    break;
                //pull register from stack P
                case 35:
                    ProcessorStatus[7] = (CPUMemory[StackPointer + 0x100] >> 7) & 1;
                    ProcessorStatus[6] = (CPUMemory[StackPointer + 0x100] >> 6) & 1;
                    ProcessorStatus[5] = 1;
                    ProcessorStatus[4] = 0;
                    ProcessorStatus[3] = (CPUMemory[StackPointer + 0x100] >> 3) & 1;
                    ProcessorStatus[2] = (CPUMemory[StackPointer + 0x100] >> 2) & 1;
                    ProcessorStatus[1] = (CPUMemory[StackPointer + 0x100] >> 1) & 1;
                    ProcessorStatus[0] = (CPUMemory[StackPointer + 0x100] >> 0) & 1;
                    CycleNumber = 0;
                    break;
                // internal operation (predecrement S?)
                case 36:
                    CycleNumber++;
                    break;
                //push PCH on stack, decrement S /n
                case 37:
                    CPUMemory[StackPointer + 0x100] = (ProgramCounter) >> 8;
                    StackPointer--;
                    StackPointer &= 0xFF;
                    CycleNumber++;
                    break;
                //fetch operand, increment PC BCC
                case 38:
                    Operand = CPUMemory[ProgramCounter];
                    if (ProcessorStatus[0] == 0) { CycleNumber++; } else { CycleNumber = 0; }
                    ProgramCounter++;
                    break;

                //ix PCH. If it did not change, increment PC.
                case 40:
                    ProgramCounter = EffectiveAddress;
                    CycleNumber = 0;
                    break;

                //write register to effective address X
                case 48:
                    WriteToCPUMemory(EffectiveAddress, IndexRegisterX);
                    CycleNumber = 0;
                    break;
                //write register to effective address A
                case 49:
                    WriteToCPUMemory(EffectiveAddress, Accumulator);
                    CycleNumber = 0;
                    break;
                //fetch high address byte, increment PC
                case 50:
                    AddressHigh = CPUMemory[ProgramCounter];
                    EffectiveAddress = AddressHigh << 8 | AddressLow;
                    CycleNumber = 0;
                    break;
                //fetch effective address low
                case 51:
                    AddressLow = CPUMemory[ProgramCounter];
                    CycleNumber++;
                    break;
                //fetch effective address high, add Y to low byte of effective address
                case 52:
                    AddressHigh = CPUMemory[ProgramCounter];
                    EffectiveAddress = AddressHigh << 8 | ((AddressLow + IndexRegisterY) & 0xFF);
                    CycleNumber++;
                    break;
                    //Get OAM Data
                case 53:
                    PPU.OAMMEM[((TempValue2 - (TempValue * 256)) + PPU.OAMADDR2003) & 0xFF] = CPU.CPUMemory[TempValue2];
                    PPU.PPUSTATUS2002 = (PPU.PPUSTATUS2002 & 224) | (CPU.CPUMemory[TempValue2] & 31);
                    CycleNumber++;
                    break;
                //Imcrement OAM Pointer
                case 54:
                    TempValue2++;
                    CycleNumber++;
                    break;
                ////fetch low address to latch
                //case 55:
                //    break;
                ////fetch PCH, copy latch to PCL
                //case 56:
                //    break;
                //Acummaltor
                case 57:
                    ProcessorStatus[0] = Accumulator % 2;
                    Accumulator = Accumulator >> 1;
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //CLC
                case 58:
                    ProcessorStatus[0] = 0;
                    CycleNumber = 0;
                    break;
                //CLD
                case 59:
                    ProcessorStatus[3] = 0;
                    CycleNumber = 0;
                    break;
                //CLI
                case 60:
                    ProcessorStatus[2] = 0;
                    CycleNumber = 0;
                    break;
                //CLV
                case 61:
                    ProcessorStatus[6] = 0;
                    CycleNumber = 0;
                    break;
                //DEX
                case 62:
                    IndexRegisterX = (IndexRegisterX - 1) & 0xFF;
                    ProcessorStatus[7] = ((IndexRegisterX >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterX | (~IndexRegisterX + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //DEY
                case 63:
                    IndexRegisterY = (IndexRegisterY - 1) & 0xFF;
                    ProcessorStatus[7] = ((IndexRegisterY >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterY | (~IndexRegisterY + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //INX
                case 64:
                    IndexRegisterX = (IndexRegisterX + 1) & 0xFF;
                    ProcessorStatus[7] = ((IndexRegisterX >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterX | (~IndexRegisterX + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //INY
                case 65:
                    IndexRegisterY = (IndexRegisterY + 1) & 0xFF;
                    ProcessorStatus[7] = ((IndexRegisterY >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterY | (~IndexRegisterY + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //SEC
                case 66:
                    ProcessorStatus[0] = 1;
                    CycleNumber = 0;
                    break;
                //SED
                case 67:
                    ProcessorStatus[3] = 1;
                    CycleNumber = 0;
                    break;
                //SEI
                case 68:
                    ProcessorStatus[2] = 1;
                    CycleNumber = 0;
                    break;
                //TAX
                case 69:
                    IndexRegisterX = Accumulator;
                    ProcessorStatus[7] = ((IndexRegisterX >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterX | (~IndexRegisterX + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //TAY
                case 70:
                    IndexRegisterY = Accumulator;
                    ProcessorStatus[7] = ((IndexRegisterY >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterY | (~IndexRegisterY + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //TSX
                case 71:
                    IndexRegisterX = StackPointer;
                    ProcessorStatus[7] = ((IndexRegisterX >> 7) & 1);
                    ProcessorStatus[1] = ~((IndexRegisterX | (~IndexRegisterX + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //TXA
                case 72:
                    Accumulator = IndexRegisterX;
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //TYA
                case 73:
                    Accumulator = IndexRegisterY;
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //TXS
                case 74:
                    StackPointer = IndexRegisterX;
                    CycleNumber = 0;
                    break;

                //pull PCH from stack
                case 76:
                    PCH = CPUMemory[StackPointer + 0x100];
                    ProgramCounter = (PCH << 8 | PCL);
                    CycleNumber++;
                    break;
                //fetch PCL NMI
                case 77:
                    PCL = CPUMemory[0xFFFA];
                    CycleNumber++;
                    break;
                //fetch PCH NMI
                case 78:
                    PCH = CPUMemory[0xFFFB];
                    ProgramCounter = PCH << 8 | PCL;
                    PPU.NMIOCCURED = false;
                    CycleNumber = 0;
                    break;

                //Acumalator
                case 80:
                    TempValue = CPUMemory[ProgramCounter];
                    EffectiveAddress = ProgramCounter;
                    Execute();
                    CycleNumber = 0;
                    break;
                //write the new value to effective address STA
                case 81:
                    WriteToCPUMemory(EffectiveAddress & 0xFFFF, Accumulator);
                    CycleNumber = 0;
                    break;
                //pull PCH from stack RTS
                case 82:
                    PCH = CPUMemory[StackPointer + 0x100];
                    ProgramCounter = (PCH << 8 | PCL);
                    CycleNumber++;
                    break;
                //read from effective address STA
                case 83:
                    TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    if ((AddressLow + IndexRegisterX) > 0xFF) {EffectiveAddress += 0x100; }
                    CycleNumber++;
                    break;
                //fetch value, increment PC
                case 84:
                    CycleNumber = 0;
                    break;
                //write the new value to effective address STA
                case 85:
                    WriteToCPUMemory(EffectiveAddress & 0xFFFF, IndexRegisterX);
                    CycleNumber = 0;
                    break;
                //write the new value to effective address STA
                case 86:
                    WriteToCPUMemory(EffectiveAddress & 0xFFFF, IndexRegisterY);
                    CycleNumber = 0;
                    break;
                //read from effective address STA
                case 87:
                    TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    if ((AddressLow + IndexRegisterY) > 0xFF) { EffectiveAddress += 0x100; }
                    CycleNumber++;
                    break;
                //fetch low address byte, increment PC
                case 88:
                    AddressLow = CPUMemory[EffectiveAddress & 0xFFFF];
                    CycleNumber++;
                    break;
                //fetch high address byte, increment PC
                case 89:
                    if (((EffectiveAddress & 0xFF) + 1) == 0x100) { EffectiveAddress -= 0x100; }
                    AddressHigh = CPUMemory[EffectiveAddress + 1];
                    ProgramCounter = AddressHigh << 8 | AddressLow;
                    CycleNumber = 0;
                    break;
                //Acummaltor asl
                case 90:
                    Accumulator = Accumulator * 2;
                    if (Accumulator > 255) { Accumulator = Accumulator - 0x100; ProcessorStatus[0] = 1; } else { ProcessorStatus[0] = 0; }
                    ProcessorStatus[7] = ((Accumulator >> 7) & 1);
                    ProcessorStatus[1] = ~((Accumulator | (~Accumulator + 1)) >> 31) & 1;
                    CycleNumber = 0;
                    break;
                //fetch low address byte, increment PC
                case 91:
                    AddressLow = CPUMemory[Pointer];
                    CycleNumber++;
                    break;
                //fetch high address byte, increment PC
                case 92:
                    AddressHigh = CPUMemory[(Pointer + 1) % 0x100];
                    EffectiveAddress = (AddressHigh << 8 | AddressLow) + IndexRegisterY;
                    CycleNumber++;
                    break;
                //read from effective address, fix the high byte of effective address IndexRegisterY
                case 93:
                    if ((AddressLow + IndexRegisterY) > 0xFF)
                    {
                        TempValue = ReadCPUMemory((EffectiveAddress - 0x100) & 0xFFFF);
                        CycleNumber++;
                    }
                    else
                    {
                        Execute();
                        CycleNumber = 0;
                    }
                    break;
                //read from effective address, fix the high byte of effective address IndexRegisterY
                case 94:
                    TempValue = ReadCPUMemory(EffectiveAddress & 0xFFFF);
                    CycleNumber++;
                    if ((AddressLow + IndexRegisterY) > 0xFF)
                    {
                        TempValue = ReadCPUMemory((EffectiveAddress - 0x100) & 0xFFFF);
                    }
                    break;
                //fetch PCL IRQ
                case 95:
                    PCL = CPUMemory[0xFFFE];
                    CycleNumber++;
                    break;
                //fetch PCH IRQ
                case 96:
                    PCH = CPUMemory[0xFFFF];
                    ProgramCounter = PCH << 8 | PCL;
                    PPU.NMIOCCURED = false;
                    CycleNumber = 0;
                    break;
                //write the value back to effective address,and do the operation on it
                case 97:
                    TempValue = CPUMemory[EffectiveAddress & 0xFFFF];
                    WriteToCPUMemory(EffectiveAddress, TempValue);
                    CycleNumber++;
                    break;
                case 98: //nmi irq
                    ProcessorStatus[5] = 1;
                    ProcessorStatus[4] = 0;
                    if (ProcessorStatus[7] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 7; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 7); }
                    if (ProcessorStatus[6] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 6; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 6); }
                    if (ProcessorStatus[5] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 5; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 5); }
                    if (ProcessorStatus[4] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 4; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 4); }
                    if (ProcessorStatus[3] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 3; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 3); }
                    if (ProcessorStatus[2] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 2; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 2); }
                    if (ProcessorStatus[1] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 1; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 1); }
                    if (ProcessorStatus[0] == 1) { CPUMemory[StackPointer + 0x100] |= 1 << 0; } else { CPUMemory[StackPointer + 0x100] &= ~(1 << 0); }
                    StackPointer--;
                    StackPointer &= 0xFF;
                    ProcessorStatus[2] = 1;
                    CycleNumber++;
                    break;
                //fetch PCL Reset
                case 99:
                    PCL = CPUMemory[0xFFFC];
                    CycleNumber++;
                    break;
                //fetch PCH Reset
                case 100:
                    PCH = CPUMemory[0xFFFD];
                    ProgramCounter = PCH << 8 | PCL;
                    CycleNumber = 0;
                    break;
            }
            IRQHandler();


            PPU.TOTALCYCLES++;
            TotalCPUCycles++;
            if (PPU.TOTALCYCLES >= 29781)
            {
                PPU.TOTALCYCLES = PPU.TOTALCYCLES - 29781;
            }
            if ((PPU.TOTALCYCLES & 0x1) == 0) { PPU.OddCycle = false; } else { PPU.OddCycle = true; }
        }

        public static void IRQHandler()
        {
            if ((PPU.NMIOCCURED || (((APU.APU4015 >> 6) & 1) == 1) || INES4.MMC3IRQ == true) && CycleNumber == 0)
            {
                if (PPU.NMIOCCURED)
                {
                    CurrentOpCode = 0x101;
                    CycleNumber = 1;
                }
                else if (CPU.ProcessorStatus[2] == 1 && ((((APU.APU4015 >> 6) & 1) == 1) || INES4.MMC3IRQ == true))
                {
                    IRQDelay = 1;
                    if ((CurrentOpCode == 0x28 && prev == 0x58) || (CurrentOpCode == 0x78 && prev == 0x58) || (CurrentOpCode == 0x78 && prev == 0x28) || (CurrentOpCode == 0x28 && prev == 0x28))
                    {
                        CurrentOpCode = 0x102;
                        CycleNumber = 1;
                        IRQDelay = 0;
                    }
                }
                else if (CPU.ProcessorStatus[2] == 0 && ((((APU.APU4015 >> 6) & 1) == 1) || INES4.MMC3IRQ == true) && IRQDelay == 0)
                {
                    CurrentOpCode = 0x102;
                    CycleNumber = 1;
                    if (CurrentOpCode == 0x28 && prev == 0x58)
                    {
                        INES4.MMC3IRQ = false;
                    }
                }
                else if (CPU.ProcessorStatus[2] == 0 && ((((APU.APU4015 >> 6) & 1) == 1) || INES4.MMC3IRQ == true) && IRQDelay > 0)
                {
                    IRQDelay--;
                    if (CurrentOpCode == 0x40)
                    {
                        CurrentOpCode = 0x102;
                        CycleNumber = 1;
                    }
                }
            }
        }
        public static void AddCycles(int cycles)
        {
            PPU.TOTALCYCLES = PPU.TOTALCYCLES + cycles;
            TotalCPUCycles = TotalCPUCycles + cycles;
            if (PPU.TOTALCYCLES >= 29781)
            {
                PPU.TOTALCYCLES = PPU.TOTALCYCLES - 29781;
            }
            if ((PPU.TOTALCYCLES & 0x1) == 0) { PPU.OddCycle = false; } else { PPU.OddCycle = true; }
        }
       

        public static void Reset()
        {
            CPU.CPUMemory = new int[64 * 1024];
            for (int i = 0; i < CPUMemory.Length; i++)
            {
                CPUMemory[i] = 0;
            }
            CPU.ProgramCounter = 0;
            CycleNumber = 0;
            CurrentOpCode = 0;
            TotalCPUCycles = 0;
            TempValue = 0;
            EffectiveAddress = 0;
            AddressLow = 0;
            AddressHigh = 0;
            Pointer = 0;
            Operand = 0;
            PCL = 0;
            PCH = 0;
            CPU.ProcessorStatus = new int[8];
            CPU.ProcessorStatus[5] = 1;
            CPU.ProcessorStatus[2] = 1;
            CPU.StackPointer = 253;
            CPU.Accumulator = 0;
            CPU.IndexRegisterX = 0;
            CPU.IndexRegisterY = 0;
        }

        public static int ReadCPUMemory(int CPUMemoryAddress)
        {
            if (CPUMemoryAddress >= 0x2000 && CPUMemoryAddress < 0x4000)
            {
                CPUMemoryAddress = 0x2000 + (CPUMemoryAddress % 8);
                return PPU.ReadPPURegister(CPUMemoryAddress);
            }
            if (GAME.TheMapper is INES5 && CPUMemoryAddress >= 0x5000 && CPUMemoryAddress < 0x6000)
            {
                return GAME.TheMapper.ReadFromAddress(CPUMemoryAddress);
            }
            if (GAME.TheMapper is INES4 && CPUMemoryAddress >= 0x8000)
            {
                ((INES4)GAME.TheMapper).ReadAddress(CPUMemoryAddress);
            }
            if (CPUMemoryAddress == 0x4015)
            {
                return APU.readapu4015register();
            }
            else if (CPUMemoryAddress == 0x4016)
            {
                return Inputs.ReadInput4016();
            }
            else if (CPUMemoryAddress == 0x4017)
            {
                return Inputs.ReadInput4017();
            }
            else if (CPUMemoryAddress <= 0x2000)
            {
                return CPUMemory[(CPUMemoryAddress & 0x7FF)];
            }
            else
            {
                return CPUMemory[CPUMemoryAddress];
            }
        }
      
        public static void WriteToCPUMemory(int CPUMemoryAddress, int Value)
        {
            GAME.TheMapper.WriteToAddress(CPUMemoryAddress, Value);            
            if (CPUMemoryAddress >= 0x2000 && CPUMemoryAddress < 0x4000)
            {
                CPUMemoryAddress = 0x2000 + (CPUMemoryAddress % 8);
            }
            if (CPUMemoryAddress >= 0x2000 && CPUMemoryAddress <= 0x2007)
            {
                PPU.WriteToPPURegister(CPUMemoryAddress, Value);
                CPUMemory[CPUMemoryAddress & 0xFFFF] = Value;
            }
            else if (CPUMemoryAddress == 0x4014)
            {
                PPU.WriteToOAMRegister(Value);
            }
            else if (CPUMemoryAddress == 0x4016)
            {
                Inputs.WriteInput4016(Value);
            }
            else if (((CPUMemoryAddress >= 0x4000) && CPUMemoryAddress <= 0x4013) || CPUMemoryAddress == 0x4015 || CPUMemoryAddress == 0x4017)
            {
                APU.WriteToRegister(CPUMemoryAddress, Value);
            }
            else if (CPUMemoryAddress <= 0x2000)
            {
                CPUMemory[(CPUMemoryAddress & 0x7ff)] = Value;
            }
            else if (CPUMemoryAddress >= 0x6000 && CPUMemoryAddress < 0x8000)
            {
                CPUMemory[(CPUMemoryAddress & 0xFFFF)] = Value;
                
            }
            else if (CPUMemoryAddress < 0x8000)
            {
                CPUMemory[CPUMemoryAddress & 0xFFFF] = Value;
            }
        }      
    }
}
