using System;
using System.IO;
namespace NesPlayer
{
    class APU
    {
        public static bool DisableAllSoundFromMenu = false;
        public static bool DisablePulse1FromMenu = false;
        public static bool DisablePulse2FromMenu = false;
        public static bool DisableNoiseFromMenu = false;
        public static bool DisableTriangleFromMenu = false;
        public static bool DisablePulse1 = false;
        public static bool DisablePulse2 = false;
        public static bool DisableNoise = false;
        public static bool DisableTriangle = false;
        public static bool DisableDMC = false;
        public static int APUFrameCounter = 0;        
        public static int Jitter = 0;
        public static int APU4015 = 0;
        public static int APU4017 = 0;
        //     |  0   1   2   3   4   5   6   7    8   9   A   B   C   D   E   F
        //-----+----------------------------------------------------------------
        //00-0F  10,254, 20,  2, 40,  4, 80,  6, 160,  8, 60, 10, 14, 12, 26, 14,
        //10-1F  12, 16, 24, 18, 48, 20, 96, 22, 192, 24, 72, 26, 16, 28, 32, 30
        public static int[] LengthCntrLookup = { 10, 254, 20, 2, 40, 4, 80, 6, 160, 8, 60, 10, 14, 12, 26, 14, 12, 16, 24, 18, 48, 20, 96, 22, 192, 24, 72, 26, 16, 28, 32, 30 };
        public static float[] APUPulse1Buffer = new float[441000];
        public static int APUBufferCounter = 0;
        public static int[] TriangleLookup = { 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        public static int[] NoiseTimerPeriodLookup = { 4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068 };
        public static int[] NoiseTimerFrequency = { 4811, 2405, 1202, 601, 300, 200, 150, 120, 95, 75, 50, 38, 25, 19, 10, 5 };
        public static int[] DMCPeriodLookup = { 428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106, 84, 72, 54 };
        public static int[,] PulseLookup = new int[4, 8] {
                     {0, 1, 0, 0, 0, 0, 0, 0},
                     {0, 1, 1, 0, 0, 0, 0, 0},
                     {0, 1, 1, 1, 1, 0, 0, 0},
                     {1, 0, 0, 1, 1, 1, 1, 1}
                     };
        public static int[] PulseOutLookup = { 0, 760, 1503, 2228, 2936, 3627, 4303, 4963, 5609, 6240, 6858 , 7462 , 8053 , 8631,
            9198,9752,10296,10828,11349,11860,12361,12852,13334,13807,14270,14725,15171,15609,16039,16461,16876
        };    
        public static float[] APUPulse2Buffer = new float[441000];
        public static float[] APUTriangleBuffer = new float[441000];
        public static float[] APUNoiseBuffer = new float[441000];
        public static bool CPUOdd = false;
        public static int SampleRate = 44100;
        public APU()
        {
            
        }
        public static void Reset()
        {
           
            APU.APU4017 = 0;
            APU.APU4015 = 0;
            NOISE.SHIFTREGISTER = 1;
            TRIANGLE.Reset();
            NOISE.Reset();
            DMC.Reset();
            DisablePulse1FromMenu = false;
            DisablePulse2FromMenu = false;
            DisableNoiseFromMenu = false;
            DisableTriangleFromMenu = false;
            DisablePulse1 = true;
            DisablePulse2 = true;
            DisableNoise = true;
            DisableTriangle = true;
            DisableDMC = false;
            DisableAllSoundFromMenu = false;
            APUFrameCounter = 0;
            APUBufferCounter = 0;
            Jitter = 0;
            APUPulse2Buffer = new float[441000];
            APUTriangleBuffer = new float[441000];
            CPUOdd = false;
            PULSE1.CONSTANTVOLUME = 0;
            PULSE1.DECAYLEVELCOUNTER = 0;
            PULSE1.DUTY = 0;
            PULSE1.DUTYBIT = 0;
            PULSE1.ENVELOPE = 0;
            PULSE1.ENVELOPELOOPLENGTHHALT = 0;
            PULSE1.LENGTH = 0;
            PULSE1.LENGTHCOUNTERLOAD = 0;
            PULSE1.NEGATE = 0;
            PULSE1.PERIOD = 0;
            PULSE1.SHIFT = 0;
            PULSE1.SWEEPDIVDERCOUNTER = 0;
            PULSE1.SWEEPENABLE = 0;
            PULSE1.SWEEPRELOAD = false;
            PULSE1.TIMER = 0;
            PULSE1.TIMERCOUNTER = 0;
            PULSE1.VOLUME = 0;
            PULSE2.CONSTANTVOLUME = 0;
            PULSE2.DECAYLEVELCOUNTER = 0;
            PULSE2.DUTY = 0;
            PULSE2.DUTYBIT = 0;
            PULSE2.ENVELOPE = 0;
            PULSE2.ENVELOPELOOPLENGTHHALT = 0;
            PULSE2.LENGTH = 0;
            PULSE2.LENGTHCOUNTERLOAD = 0;
            PULSE2.NEGATE = 0;
            PULSE2.PERIOD = 0;
            PULSE2.SHIFT = 0;
            PULSE2.SWEEPDIVDERCOUNTER = 0;
            PULSE2.SWEEPENABLE = 0;
            PULSE2.SWEEPRELOAD = false;
            PULSE2.TIMER = 0;
            PULSE2.TIMERCOUNTER = 0;
            PULSE2.VOLUME = 0;
        }
        public static void WriteToRegister(int cpumemoryaddress, int valuetobewritten)
        {
            switch (cpumemoryaddress)
            {
                case 0x4000:
                    PULSE1.ENVELOPE = valuetobewritten & 15;
                    PULSE1.CONSTANTVOLUME = valuetobewritten >> 4 & 1;
                    PULSE1.ENVELOPELOOPLENGTHHALT = valuetobewritten >> 5 & 1;
                    PULSE1.DUTY = valuetobewritten >> 6 & 3;
                    PULSE1.VOLUME = 15;
                    break;
                case 0x4001:
                    PULSE1.SHIFT = valuetobewritten & 7;
                    PULSE1.NEGATE = valuetobewritten >> 3 & 1;
                    PULSE1.PERIOD = valuetobewritten >> 4 & 7;
                    PULSE1.SWEEPENABLE = valuetobewritten >> 7 & 1;
                    PULSE1.SWEEPRELOAD = true;
                    break;
                case 0x4002:
                    PULSE1.TIMER = (PULSE1.TIMER & 0x700) | valuetobewritten;
                    break;
                case 0x4003:
                    PULSE1.TIMER = (PULSE1.TIMER & 0xFF) | (valuetobewritten & 7) << 8;
                    PULSE1.LENGTHCOUNTERLOAD = valuetobewritten >> 3 & 31;
                    PULSE1.DUTYBIT = 0;
                    if (!DisablePulse1)
                    {
                        PULSE1.LENGTH = LengthCntrLookup[PULSE1.LENGTHCOUNTERLOAD];
                    }
                    break;
                case 0x4004:
                    PULSE2.ENVELOPE = valuetobewritten & 15;
                    PULSE2.CONSTANTVOLUME = valuetobewritten >> 4 & 1;
                    PULSE2.ENVELOPELOOPLENGTHHALT = valuetobewritten >> 5 & 1;
                    PULSE2.DUTY = valuetobewritten >> 6 & 3;
                    PULSE2.VOLUME = 15;
                    break;
                case 0x4005:
                    PULSE2.SHIFT = valuetobewritten & 7;
                    PULSE1.NEGATE = valuetobewritten >> 3 & 1;
                    PULSE2.PERIOD = valuetobewritten >> 4 & 7;
                    PULSE2.SWEEPENABLE = valuetobewritten >> 7 & 1;
                    PULSE2.SWEEPRELOAD = true;
                    break;
                case 0x4006:
                    PULSE2.TIMER = (PULSE2.TIMER & 0x700) | valuetobewritten;
                    break;
                case 0x4007:
                    PULSE2.TIMER = (PULSE2.TIMER & 0xFF) | (valuetobewritten & 7) << 8;
                    PULSE2.LENGTHCOUNTERLOAD = valuetobewritten >> 3 & 31;
                    PULSE2.DUTYBIT = 0;
                    if (!DisablePulse2)
                    {
                        PULSE2.LENGTH = LengthCntrLookup[PULSE2.LENGTHCOUNTERLOAD];
                    }
                    break;
                case 0x4008:
                    TRIANGLE.LINEARCOUNTERLOADVALUE = valuetobewritten & 127;
                    TRIANGLE.LINEARCONTROLLENGTHHALT = valuetobewritten >> 7 & 1;
                    break;
                case 0x4009:
                    break;
                case 0x400a:
                    TRIANGLE.TIMER = (TRIANGLE.TIMER & 0x700) | valuetobewritten;
                    break;
                case 0x400b:
                    TRIANGLE.TIMER = (TRIANGLE.TIMER & 0xFF) | (valuetobewritten & 7) << 8;
                    TRIANGLE.LENGTHCOUNTERLOAD = valuetobewritten >> 3 & 31;
                    TRIANGLE.LINEARCOUNTERLOAD = true;
                    if (!DisableTriangle)
                    {
                        TRIANGLE.LENGTH = LengthCntrLookup[TRIANGLE.LENGTHCOUNTERLOAD];
                    }
                    break;
                case 0x400c:
                    NOISE.ENVELOPE = valuetobewritten & 15;
                    NOISE.CONSTANTVOLUME = valuetobewritten >> 4 & 1;
                    NOISE.ENVELOPELOOPLENGTHHALT = valuetobewritten >> 5 & 1;
                    break;
                case 0x400d:
                    break;
                case 0x400e:
                    NOISE.NOISEPERIOD = valuetobewritten & 15;
                    NOISE.LOOPNOISE = valuetobewritten >> 7 & 1;
                    break;
                case 0x400f:
                    NOISE.LENGTHCOUNTERLOAD = valuetobewritten >> 3 & 31;
                    if (!DisableNoise)
                    {
                        NOISE.LENGTH = LengthCntrLookup[NOISE.LENGTHCOUNTERLOAD];
                    }
                    break;
                case 0x4010:
                    DMC.LOOP = valuetobewritten >> 6 & 1;
                    DMC.IRQENABLE = valuetobewritten >> 7 & 1;
                    DMC.FREQUENCY = DMCPeriodLookup[valuetobewritten & 15];
                    break;
                case 0x4011:
                    DMC.DMCOUTPUT = valuetobewritten & 127;
                    break;
                case 0x4012:
                    DMC.SAMPLEADDRESS = 0xC000 | (valuetobewritten << 6);
                    break;
                case 0x4013:
                    DMC.SAMPLELENGTH = (valuetobewritten << 4) | 1;
                    break;
                case 0x4015:
                    if ((((valuetobewritten >> 0) & 1) == 0)) { APU4015 &= ~(1 << 0); PULSE1.LENGTH = 0; DisablePulse1 = true; } else { APU4015 |= 1 << 0; DisablePulse1 = false; }
                    if ((((valuetobewritten >> 1) & 1) == 0)) { APU4015 &= ~(1 << 1); PULSE2.LENGTH = 0; DisablePulse2 = true; } else { APU4015 |= 1 << 1; DisablePulse2 = false; }
                    if ((((valuetobewritten >> 2) & 1) == 0)) { APU4015 &= ~(1 << 2); TRIANGLE.LENGTH = 0; DisableTriangle = true; } else { APU4015 |= 1 << 2; DisableTriangle = false; }
                    if ((((valuetobewritten >> 3) & 1) == 0)) { APU4015 &= ~(1 << 3); NOISE.LENGTH = 0; DisableNoise = true; } else { APU4015 |= 1 << 3; DisableNoise = false; }
                    if ((((valuetobewritten >> 4) & 1) == 0)) { APU4015 &= ~(1 << 4);
                        DMC.BYTESREMAINING = 0;
                    }
                    else
                    {
                        APU4015 |= 1 << 4;
                        if (DMC.BYTESREMAINING == 0)
                        {
                            DMC.CURRENTADDRESS = DMC.SAMPLEADDRESS;
                            DMC.BYTESREMAINING = DMC.SAMPLELENGTH;
                        }                        
                    }
                    DMC.IRQENABLE = 0;
                    APU4015 &= ~(1 << 7);
                    break;
                case 0x4017:
                    APU4017 = valuetobewritten;
                    if ((((APU4017 >> 6) & 1) == 1))
                    {
                        APU4015 &= ~(1 << 6);
                    }
                   
                    if ((((APU4017 >> 7) & 1) == 1))
                    {
                        APUFrameCounter = 0;
                        clockenvelopesandtrianglelinearcounter();
                        clocklengthcountersandsweepunits();
                        
                   }
                    else
                    {
                       if (((PPU.TOTALCYCLES&1)==1)&&!CPUOdd) { Jitter = 1; } else { Jitter = 0; }
                        APUFrameCounter = 0;
                    }
                    break;
                default:
                    break;
            }
        }
        
        public static int readapu4015register()
        {
            if (PULSE1.LENGTH == 0) { APU4015 &= ~(1 << 0); } else { APU4015 |= 1 << 0; }
            if (PULSE2.LENGTH == 0) { APU4015 &= ~(1 << 1); } else { APU4015 |= 1 << 1; }
            if (TRIANGLE.LENGTH == 0) { APU4015 &= ~(1 << 2); } else { APU4015 |= 1 << 2; }
            if (NOISE.LENGTH == 0) { APU4015 &= ~(1 << 3); } else { APU4015 |= 1 << 3; }
            if (DMC.BYTESREMAINING == 0) { APU4015 &= ~(1 << 4); } else { APU4015 |= 1 << 4; }
            if ((Jitter == 1) && (APUFrameCounter) == 29831)
            {
                APU4015 |= 1 << 6;
            }
            if (!CPUOdd&&(APUFrameCounter) == 29831)
            {
                APU4015 &= ~(1 << 6);
            }
            int attemp = APU4015;
            APU4015 &= ~(1 << 6);
            return attemp;
        }
        public static void runframe()
        {
            for (int i = 0; i < 735; i++)
            {
                int vol1 = PULSE1.VOLUME;
                if (PULSE1.CONSTANTVOLUME == 0) { vol1 = PULSE1.ENVELOPE; }
                int vol2 = PULSE2.VOLUME;
                if (PULSE2.CONSTANTVOLUME == 0) { vol2 = PULSE2.ENVELOPE; }
                int vol3 = 6;
                int vol4 = NOISE.VOLUME;
                if (NOISE.CONSTANTVOLUME == 0) { vol4 = NOISE.ENVELOPE; }
                int ampstep = 200;
                int ampstep2 = 200;
                APUPulse1Buffer[APUBufferCounter] = (float)(Math.Sin((PULSE1.OutputFrequency() * (2 * Math.PI) / (double)44100) * (double)APUBufferCounter));
                if (APUPulse1Buffer[APUBufferCounter] > 0) { APUPulse1Buffer[APUBufferCounter] = vol1 * ampstep; } else if (APUPulse1Buffer[APUBufferCounter] == 0) { APUPulse1Buffer[APUBufferCounter] = 0; } else { APUPulse1Buffer[APUBufferCounter] = -(vol1 * ampstep); }
                APUPulse2Buffer[APUBufferCounter] = (float)(Math.Sin((PULSE2.OutputFrequency() * (2 * Math.PI) / (double)44100) * (double)APUBufferCounter));
                if (APUPulse2Buffer[APUBufferCounter] > 0) { APUPulse2Buffer[APUBufferCounter] = vol2 * ampstep; } else if (APUPulse2Buffer[APUBufferCounter] == 0) { APUPulse2Buffer[APUBufferCounter] = 0; } else { APUPulse2Buffer[APUBufferCounter] = -(vol2 * ampstep); }
                APUTriangleBuffer[APUBufferCounter] = (float)(Math.Sin((TRIANGLE.OutputFrequency() * (2 * Math.PI) / (double)44100) * (double)APUBufferCounter));
                if (APUTriangleBuffer[APUBufferCounter] > 0) { APUTriangleBuffer[APUBufferCounter] = vol3 * ampstep2; } else if (APUTriangleBuffer[APUBufferCounter] == 0) { APUTriangleBuffer[APUBufferCounter] = 0; } else { APUTriangleBuffer[APUBufferCounter] = -(vol3 * ampstep2); }
                APUNoiseBuffer[APUBufferCounter] = (float)(Math.Sin((NOISE.OutputFrequency() * (2 * Math.PI) / (double)44100) * (double)APUBufferCounter));
                if (APUNoiseBuffer[APUBufferCounter] > 0) { APUNoiseBuffer[APUBufferCounter] = vol4 * ampstep; } else if (APUNoiseBuffer[APUBufferCounter] == 0) { APUNoiseBuffer[APUBufferCounter] = 0; } else { APUPulse2Buffer[APUBufferCounter] = -(vol4 * ampstep); }
                if (DisablePulse1FromMenu || DisableAllSoundFromMenu) { APUPulse1Buffer[APUBufferCounter] = 0; }
                if (DisablePulse2FromMenu || DisableAllSoundFromMenu) { APUPulse2Buffer[APUBufferCounter] = 0; }
                if (DisableTriangleFromMenu || DisableAllSoundFromMenu) { APUTriangleBuffer[APUBufferCounter] = 0; }
                if (DisableNoiseFromMenu || DisableAllSoundFromMenu) { APUNoiseBuffer[APUBufferCounter] = 0; }
                float hdgd = ((APUPulse1Buffer[APUBufferCounter] + APUPulse2Buffer[APUBufferCounter] + APUTriangleBuffer[APUBufferCounter] + APUNoiseBuffer[APUBufferCounter]));
                APUPulse1Buffer[APUBufferCounter] = hdgd;
                APUBufferCounter++;
                if (APUBufferCounter == 44100)
                {
                    Play();
                }
              
            }
        }
    

        public static float mixeroutput()
        {
            byte pulse1freq = PULSE1.Output();
            byte pulse2freq = PULSE2.Output();
            byte triangleout = TRIANGLE.Output();
            byte noiseout = NOISE.Output();
            int dmcout = DMC.DMCOUTPUT;
            float pulse_out = 0;
            if ((pulse1freq + pulse2freq) > 0)
            {
                pulse_out = 95.88f / ((8128 / (pulse1freq + pulse2freq)) + 100f);
            }
            float tnd_out = 158.79f / ((1 / ((triangleout / 8227f) + (noiseout / 12241f) + (dmcout / 22638f))) + 100);
            double output = (((pulse_out + tnd_out)));
            short freq2=(short)(PulseOutLookup[pulse1freq + pulse2freq] +tnd_out*65220);
            double freq = output * 65220;
            float n = Convert.ToInt16(freq2);
            return n;
        }
        public static void Play()
        {
            if (APUBufferCounter == 44100)
            {
                var mStrm = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mStrm);
                int formatChunkSize = 16;
                int headerSize = 8;
                short formatType = 1;
                short tracks = 1;
                int samplesPerSecond = SampleRate;
                short bitsPerSample = 16;
                short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                int bytesPerSecond = samplesPerSecond * frameSize;
                int waveSize = 4;
                int samples = APUBufferCounter;
                int dataChunkSize = samples * frameSize;
                int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
                writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
                writer.Write(fileSize);
                writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
                writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
                writer.Write(formatChunkSize);
                writer.Write(formatType);
                writer.Write(tracks);
                writer.Write(samplesPerSecond);
                writer.Write(bytesPerSecond);
                writer.Write(frameSize);
                writer.Write(bitsPerSample);
                writer.Write(0x61746164); // = encoding.GetBytes("data")               
                writer.Write(dataChunkSize);
                {                    
                    for (int step = 0; step < (samples); step++)
                    {
                       short s1 = (short)((APUPulse1Buffer[step]));
                        writer.Write(s1);
                    }
                }
                mStrm.Seek(0, SeekOrigin.Begin);
                new System.Media.SoundPlayer(mStrm).Play();
                writer.Close();
                mStrm.Close();
                APUBufferCounter = 0;
            }
           
        }
      
       
        public static void clockenvelopesandtrianglelinearcounter()
        {
            TRIANGLE.ClockLinearCounter();
            PULSE1.ClockEnvelope();            
            PULSE2.ClockEnvelope();           
            NOISE.ClockEnvelope();            
        }
        public static void clocklengthcountersandsweepunits()
        {
            PULSE1.ClockLength();
            PULSE2.ClockLength();
            NOISE.ClockLength();
            TRIANGLE.ClockLength();
            PULSE1.ClockSweep();            
            PULSE2.ClockSweep();           
        }
        public static void clocktimersAPU()
        {
            PULSE1.ClockTimer();
            PULSE2.ClockTimer();
            NOISE.ClockTimer();            
            DMC.ClockTimer();           
        }

        public static void clocktimersCPU()
        {
            TRIANGLE.ClockTimer();
           // DMC.ClockTimer();
        }
      
        public static void RunAPU(int cpucyc)
        {
            int fincpucyc = cpucyc;
            if (((APU4017 >> 7) & 1) == 0)
            {               
                for (int i = 0; i < fincpucyc; i++)
                {
                    if ((APUFrameCounter) == (7459 + Jitter))
                    {
                        clockenvelopesandtrianglelinearcounter();
                        runframe();
                    }
                    else if ((APUFrameCounter) == (14915 + Jitter))
                    {
                        clockenvelopesandtrianglelinearcounter();
                        clocklengthcountersandsweepunits();
                        runframe();
                    }
                    else if ((APUFrameCounter) == (22373 + Jitter))
                    {
                        clockenvelopesandtrianglelinearcounter();
                        runframe();
                    }
                    else if ((APUFrameCounter) == (29830 + Jitter))
                    {
                        if (((APU4017 >> 6) & 1) == 0)
                        {
                            APU4015 |= 1 << 6;
                            if (CPU.ProcessorStatus[2] == 0) { CPU.IRQDelay = 0; } else { CPU.IRQDelay = 1; }
                         }
                    }
                    else if ((APUFrameCounter) == (29831 + Jitter))
                    {
                        clockenvelopesandtrianglelinearcounter();
                        clocklengthcountersandsweepunits();
                        runframe();
                        if (((APU4017 >> 6) & 1) == 0)
                        {
                            APU4015 |= 1 << 6;
                            if (CPU.ProcessorStatus[2] == 0) { CPU.IRQDelay = 0; } else { CPU.IRQDelay = 1; }
                            Jitter = 0;
                        }
                    }
                    else if ((APUFrameCounter) == (29832 + Jitter))
                    {
                        if (((APU4017 >> 6) & 1) == 0)
                        {
                            APU4015 |= 1 << 6;
                            if (CPU.ProcessorStatus[2] == 0) { CPU.IRQDelay = 0; } else { CPU.IRQDelay = 1; }
                        }
                    }
                    else if ((APUFrameCounter) == (37289 + Jitter))
                    {
                        clockenvelopesandtrianglelinearcounter();
                        runframe();
                        APUFrameCounter = 7460;
                    }
                        if (CPUOdd) { CPUOdd = false; } else { CPUOdd = true; }
                    if (CPUOdd)
                    {
                        clocktimersAPU();
                    }
                    clocktimersCPU();
                    APUFrameCounter++;
                }
            }
            else
            {
                for (int i = 0; i < fincpucyc; i++)
                {
                    if (APUFrameCounter == 7459)
                        {
                            clockenvelopesandtrianglelinearcounter();
                            runframe();
                        }
                        else if (APUFrameCounter == 14915)
                        {
                            clockenvelopesandtrianglelinearcounter();
                            clocklengthcountersandsweepunits();
                            runframe();
                        }
                        else if (APUFrameCounter == 22373)
                        {
                            clockenvelopesandtrianglelinearcounter();
                            runframe();
                        }
                        else if (APUFrameCounter == 29831)
                        {
                            //clockenvelopesandtrianglelinearcounter();
                            //clocklengthcountersandsweepunits();
                            runframe();
                        }
                        else if (APUFrameCounter == 37283)
                        {
                        clockenvelopesandtrianglelinearcounter();
                        clocklengthcountersandsweepunits();
                        APUFrameCounter = 1;
                        runframe();
                    }
                    if (CPUOdd) { CPUOdd = false; } else { CPUOdd = true; }
                    if (CPUOdd)
                    {
                        clocktimersAPU();
                    }
                    clocktimersCPU();
                    APUFrameCounter++;

                }
            }
        }

    }
    class PULSE1
    {
        private static int _LENGTH;
        public static int LENGTH
        {
            get { return _LENGTH; }
            set { _LENGTH = value; }
        }

        private static int _DUTY;
        public static int DUTY
        {
            get { return _DUTY; }
            set { _DUTY = value; }
        }

        private static int _ENVELOPELOOPLENGTHHALT;
        public static int ENVELOPELOOPLENGTHHALT
        {
            get { return _ENVELOPELOOPLENGTHHALT; }
            set { _ENVELOPELOOPLENGTHHALT = value; }
        }

        private static int _CONSTANTVOLUME;
        public static int CONSTANTVOLUME
        {
            get { return _CONSTANTVOLUME; }
            set { _CONSTANTVOLUME = value; }
        }

        private static int _ENVELOPE;
        public static int ENVELOPE
        {
            get { return _ENVELOPE; }
            set { _ENVELOPE = value; }
        }

        private static bool _SWEEPRELOAD;
        public static bool SWEEPRELOAD
        {
            get { return _SWEEPRELOAD; }
            set { _SWEEPRELOAD = value; }
        }

        private static int _SWEEPENABLE;
        public static int SWEEPENABLE
        {
            get { return _SWEEPENABLE; }
            set { _SWEEPENABLE = value; }
        }

        private static int _SWEEPDIVDERCOUNTER;
        public static int SWEEPDIVDERCOUNTER
        {
            get { return _SWEEPDIVDERCOUNTER; }
            set { _SWEEPDIVDERCOUNTER = value; }
        }

        private static int _PERIOD;
        public static int PERIOD
        {
            get { return _PERIOD; }
            set { _PERIOD = value; }
        }

        private static int _VOLUME;
        public static int VOLUME
        {
            get { return _VOLUME; }
            set { _VOLUME = value; }
        }

        private static int _DUTYBIT;
        public static int DUTYBIT
        {
            get { return _DUTYBIT; }
            set { _DUTYBIT = value; }
        }

        private static int _DECAYLEVELCOUNTER;
        public static int DECAYLEVELCOUNTER
        {
            get { return _DECAYLEVELCOUNTER; }
            set { _DECAYLEVELCOUNTER = value; }
        }

        private static int _NEGATE;
        public static int NEGATE
        {
            get { return _NEGATE; }
            set { _NEGATE = value; }
        }

        private static int _SHIFT;
        public static int SHIFT
        {
            get { return _SHIFT; }
            set { _SHIFT = value; }
        }

        private static int _TIMER;
        public static int TIMER
        {
            get { return _TIMER; }
            set { _TIMER = value; }
        }

        private static int _TIMERCOUNTER;
        public static int TIMERCOUNTER
        {
            get { return _TIMERCOUNTER; }
            set { _TIMERCOUNTER = value; }
        }
        private static int _LENGTHCOUNTERLOAD;
        public static int LENGTHCOUNTERLOAD
        {
            get { return _LENGTHCOUNTERLOAD; }
            set { _LENGTHCOUNTERLOAD = value; }
        }

        public static void ClockLength()
        {
            if (ENVELOPELOOPLENGTHHALT == 0)
            {
                if (LENGTH > 0)
                {
                    LENGTH--;
                }
            }
        }
        public static void ClockEnvelope()
        {
            if (DECAYLEVELCOUNTER > 0)
            {
                DECAYLEVELCOUNTER--;
            }
            else
            {
                if (ENVELOPELOOPLENGTHHALT == 1 && DECAYLEVELCOUNTER == 0)
                {
                    DECAYLEVELCOUNTER = 15;
                }
                else if (DECAYLEVELCOUNTER > 0)
                {
                    DECAYLEVELCOUNTER--;
                }
                if (CONSTANTVOLUME == 1)
                {
                    VOLUME = ENVELOPE;
                }
                else
                {
                    VOLUME = DECAYLEVELCOUNTER;
                }
            }
        }
        public static void ClockTimer()
        {
            if (TIMERCOUNTER > 0)
            {
                TIMERCOUNTER--;
                DUTYBIT++;
                if (DUTYBIT == 8) { DUTYBIT = 0; }
            }
            else
            {
                TIMERCOUNTER = TIMER;
            }
        }

        public static void ClockSweep()
        {
            if (SWEEPRELOAD)
            {
                if (SWEEPENABLE == 1 && SWEEPDIVDERCOUNTER == 0)
                {
                    if (NEGATE == 1)
                    {
                        TIMER -= (TIMER >> SHIFT);
                        if (TIMER > 0) { TIMER--; }
                    }
                    else
                    {
                        TIMER += (TIMER >> SHIFT);
                    }                    
                }
                SWEEPDIVDERCOUNTER = PERIOD;
                SWEEPRELOAD = false;
            }
            else if (SWEEPDIVDERCOUNTER > 0)
            {
                SWEEPDIVDERCOUNTER--;
            }
            else
            {
                if (SWEEPENABLE == 1)
                {
                    if (NEGATE == 1)
                    {
                        TIMER -= (TIMER >> SHIFT);
                        if (TIMER > 0) { TIMER--; }
                    }
                    else
                    {
                        TIMER += (TIMER >> SHIFT);
                    }
                }
                SWEEPDIVDERCOUNTER = PERIOD;
            }
        }
        public static byte Output()
        {
            byte samp = 0;
            if ((APU.APU4015 & 1) == 1 && LENGTH > 0 && TIMER>7 && APU.PulseLookup[DUTY, DUTYBIT]!=0)
            {
                if (CONSTANTVOLUME == 0)
                {
                    samp = (byte)VOLUME;
                }
                else
                {
                    samp = (byte)ENVELOPE;
                }              
            }
            return samp;
        }
        public static float OutputFrequency()
        {
            float samp = 0;
            if ((APU.APU4015 & 1) == 1 && LENGTH > 0 && TIMER > 7 )
            {
                double f = 1789773 / (16 * (TIMER + 1));
                samp = (float)f;
            }
            return samp;
        }
    }
    class PULSE2
    {
        private static int _LENGTH;
        public static int LENGTH
        {
            get { return _LENGTH; }
            set { _LENGTH = value; }
        }

        private static int _DUTY;
        public static int DUTY
        {
            get { return _DUTY; }
            set { _DUTY = value; }
        }

        private static int _DUTYBIT;
        public static int DUTYBIT
        {
            get { return _DUTYBIT; }
            set { _DUTYBIT = value; }
        }
        private static int _ENVELOPELOOPLENGTHHALT;
        public static int ENVELOPELOOPLENGTHHALT
        {
            get { return _ENVELOPELOOPLENGTHHALT; }
            set { _ENVELOPELOOPLENGTHHALT = value; }
        }

        private static int _CONSTANTVOLUME;
        public static int CONSTANTVOLUME
        {
            get { return _CONSTANTVOLUME; }
            set { _CONSTANTVOLUME = value; }
        }

        private static int _VOLUME;
        public static int VOLUME
        {
            get { return _VOLUME; }
            set { _VOLUME = value; }
        }

        private static bool _SWEEPRELOAD;
        public static bool SWEEPRELOAD
        {
            get { return _SWEEPRELOAD; }
            set { _SWEEPRELOAD = value; }
        }

        private static int _ENVELOPE;
        public static int ENVELOPE
        {
            get { return _ENVELOPE; }
            set { _ENVELOPE = value; }
        }
        private static int _SWEEPDIVDERCOUNTER;
        public static int SWEEPDIVDERCOUNTER
        {
            get { return _SWEEPDIVDERCOUNTER; }
            set { _SWEEPDIVDERCOUNTER = value; }
        }

        private static int _DECAYLEVELCOUNTER;
        public static int DECAYLEVELCOUNTER
        {
            get { return _DECAYLEVELCOUNTER; }
            set { _DECAYLEVELCOUNTER = value; }
        }

        private static int _SWEEPENABLE;
        public static int SWEEPENABLE
        {
            get { return _SWEEPENABLE; }
            set { _SWEEPENABLE = value; }
        }
        private static int _TIMERCOUNTER;
        public static int TIMERCOUNTER
        {
            get { return _TIMERCOUNTER; }
            set { _TIMERCOUNTER = value; }
        }
        private static int _PERIOD;
        public static int PERIOD
        {
            get { return _PERIOD; }
            set { _PERIOD = value; }
        }

        private static int _NEGATE;
        public static int NEGATE
        {
            get { return _NEGATE; }
            set { _NEGATE = value; }
        }

        private static int _SHIFT;
        public static int SHIFT
        {
            get { return _SHIFT; }
            set { _SHIFT = value; }
        }

        private static int _TIMER;
        public static int TIMER
        {
            get { return _TIMER; }
            set { _TIMER = value; }
        }

        private static int _LENGTHCOUNTERLOAD;
        public static int LENGTHCOUNTERLOAD
        {
            get { return _LENGTHCOUNTERLOAD; }
            set { _LENGTHCOUNTERLOAD = value; }
        }
        public static void ClockLength()
        {
            if (ENVELOPELOOPLENGTHHALT == 0)
            {
                if (LENGTH > 0)
                {
                    LENGTH--;
                }
            }
        }
        public static void ClockEnvelope()
        {
            if (DECAYLEVELCOUNTER > 0)
            {
                DECAYLEVELCOUNTER--;
            }
            else
            {
                if (ENVELOPELOOPLENGTHHALT == 1 && DECAYLEVELCOUNTER == 0)
                {
                    DECAYLEVELCOUNTER = 15;
                }
                else if (DECAYLEVELCOUNTER > 0)
                {
                    DECAYLEVELCOUNTER--;
                }
                if (CONSTANTVOLUME == 1)
                {
                    VOLUME = ENVELOPE;
                }
                else
                {
                    VOLUME = DECAYLEVELCOUNTER;
                }
            }
        }
            
        
        public static void ClockTimer()
        {
            if (TIMERCOUNTER > 0)
            {
                TIMERCOUNTER--;
                DUTYBIT++;
                if (DUTYBIT == 8) { DUTYBIT = 0; }
            }
            else
            {
                TIMERCOUNTER = TIMER;
            }
        }

        public static void ClockSweep()
        {
            if (SWEEPRELOAD)
            {
                if (SWEEPENABLE == 1 && SWEEPDIVDERCOUNTER == 0)
                {
                    int shiftedvalue = (TIMER >> SHIFT);
                    if (NEGATE == 1)
                    {
                        TIMER -= (TIMER >> SHIFT);
                    }
                    else
                    {
                        TIMER += (TIMER >> SHIFT);
                    }
                }
                SWEEPDIVDERCOUNTER = PERIOD;
                SWEEPRELOAD = false;
            }
            else if (SWEEPDIVDERCOUNTER > 0)
            {
                SWEEPDIVDERCOUNTER--;
            }
            else
            {
                if (SWEEPENABLE == 1)
                {
                    if (NEGATE == 1)
                    {
                        TIMER -= (TIMER >> SHIFT);
                    }
                    else
                    {
                        TIMER += (TIMER >> SHIFT);
                    }
                }
                SWEEPDIVDERCOUNTER = PERIOD;
            }
        }
        public static byte Output()
        {
            byte samp = 0;
            if (((APU.APU4015 >> 1) & 1)==1 && LENGTH > 0 && TIMER > 7 && APU.PulseLookup[DUTY, DUTYBIT] != 0)
            {
                if (CONSTANTVOLUME == 0)
                {
                    samp = (byte)VOLUME;
                }
                else
                {
                    samp = (byte)ENVELOPE;
                }
            }
            return samp;
        }
        public static float OutputFrequency()
        {
            float samp = 0f;
            if (((APU.APU4015 >> 1) & 1)==1 && LENGTH > 0 && TIMER > 7 )
            {
                double f = 1789773 / (16 * (TIMER + 1));
                samp = (float)f;
            }
            return samp;
        }
    }
    class TRIANGLE
    {
        private static int _LENGTH;
        public static int LENGTH
        {
            get { return _LENGTH; }
            set { _LENGTH = value; }
        }

        private static int _LINEARCONTROLLENGTHHALT;
        public static int LINEARCONTROLLENGTHHALT
        {
            get { return _LINEARCONTROLLENGTHHALT; }
            set { _LINEARCONTROLLENGTHHALT = value; }
        }
        private static int _VOLUME;
        public static int VOLUME
        {
            get { return _VOLUME; }
            set { _VOLUME = value; }
        }

        private static int _LINEARCOUNTER;
        public static int LINEARCOUNTER
        {
            get { return _LINEARCOUNTER; }
            set { _LINEARCOUNTER = value; }
        }

       
        private static int _DUTYBIT;
        public static int DUTYBIT
        {
            get { return _DUTYBIT; }
            set { _DUTYBIT = value; }
        }

        private static int _LINEARCOUNTERLOADVALUE;
        public static int LINEARCOUNTERLOADVALUE
        {
            get { return _LINEARCOUNTERLOADVALUE; }
            set { _LINEARCOUNTERLOADVALUE = value; }
        }

        private static bool _LINEARCOUNTERLOAD;
        public static bool LINEARCOUNTERLOAD
        {
            get { return _LINEARCOUNTERLOAD; }
            set { _LINEARCOUNTERLOAD = value; }
        } 
        private static int _TIMER;
        public static int TIMER
        {
            get { return _TIMER; }
            set { _TIMER = value; }
        }
        private static int _TIMERCOUNTER;
        public static int TIMERCOUNTER
        {
            get { return _TIMERCOUNTER; }
            set { _TIMERCOUNTER = value; }
        }
        private static int _LENGTHCOUNTERLOAD;
        public static int LENGTHCOUNTERLOAD
        {
            get { return _LENGTHCOUNTERLOAD; }
            set { _LENGTHCOUNTERLOAD = value; }
        }
        public static void Reset()
        {
            LENGTH = 0;
            LINEARCONTROLLENGTHHALT = 0;
            VOLUME = 0;
            LINEARCOUNTER = 0;
            DUTYBIT = 0;
            LINEARCOUNTERLOADVALUE = 0;
            LINEARCOUNTERLOAD = false;
            TIMER = 0;
            TIMERCOUNTER = 0;
            LENGTHCOUNTERLOAD = 0;

        }
        public static void ClockLength()
        {
            if (LINEARCONTROLLENGTHHALT == 0 && LENGTH>0)
            {
                LENGTH--;
            }
        }
        public static void ClockLinearCounter()
        {
            if (LINEARCOUNTERLOAD)
            { LINEARCOUNTER = LINEARCOUNTERLOADVALUE; }
            else if (LINEARCOUNTER > 0)
            {
                LINEARCOUNTER--;
            }
            if (LINEARCONTROLLENGTHHALT == 1)
            {
                LINEARCOUNTERLOAD = false;
            }
        }
        public static void ClockTimer()
        {
            if (TIMERCOUNTER > 0)
            {
                TIMERCOUNTER--;
                if(LENGTH>0 && LINEARCOUNTER > 0)
                {
                    DUTYBIT++;
                    if (DUTYBIT == 32) { DUTYBIT = 0; }
                }
            }
            else
            {
                TIMERCOUNTER = TIMER;
            }
        }
        public static byte Output()
        {
            byte output= 0;
            if(LENGTH>0&&LINEARCOUNTER>0&& ((APU.APU4015 >> 2) & 1) == 1)
            {
                output = (byte)APU.TriangleLookup[DUTYBIT];
            }          
            return output;
        }
        public static float OutputFrequency()
        {
            float output = 0.0f;
            if (LENGTH > 0 && LINEARCOUNTER > 0 && ((APU.APU4015 >> 2) & 1) == 1 && TIMER>1)
            {
                double f = 1789773 / (32 * (TIMER + 1));
                output = (float)f *3;
            }
            return output;
        }
    }
    class NOISE
    {
        private static int _LENGTH;
        public static int LENGTH
        {
            get { return _LENGTH; }
            set { _LENGTH = value; }
        }

        private static int _ENVELOPELOOPLENGTHHALT;
        public static int ENVELOPELOOPLENGTHHALT
        {
            get { return _ENVELOPELOOPLENGTHHALT; }
            set { _ENVELOPELOOPLENGTHHALT = value; }
        }

        private static int _CONSTANTVOLUME;
        public static int CONSTANTVOLUME
        {
            get { return _CONSTANTVOLUME; }
            set { _CONSTANTVOLUME = value; }
        }

        private static int _VOLUME;
        public static int VOLUME
        {
            get { return _VOLUME; }
            set { _VOLUME = value; }
        }

        private static int _ENVELOPE;
        public static int ENVELOPE
        {
            get { return _ENVELOPE; }
            set { _ENVELOPE = value; }
        }
        private static int _DECAYLEVELCOUNTER;
        public static int DECAYLEVELCOUNTER
        {
            get { return _DECAYLEVELCOUNTER; }
            set { _DECAYLEVELCOUNTER = value; }
        }

        private static int _LOOPNOISE;
        public static int LOOPNOISE
        {
            get { return _LOOPNOISE; }
            set { _LOOPNOISE = value; }
        }

        private static int _NOISEPERIOD;
        public static int NOISEPERIOD
        {
            get { return _NOISEPERIOD; }
            set { _NOISEPERIOD = value; }
        }

        private static int _SHIFTREGISTER;
        public static int SHIFTREGISTER
        {
            get { return _SHIFTREGISTER; }
            set { _SHIFTREGISTER = value; }
        }

        private static int _TIMERCOUNTER;
        public static int TIMERCOUNTER
        {
            get { return _TIMERCOUNTER; }
            set { _TIMERCOUNTER = value; }
        }
        private static int _LENGTHCOUNTERLOAD;
        public static int LENGTHCOUNTERLOAD
        {
            get { return _LENGTHCOUNTERLOAD; }
            set { _LENGTHCOUNTERLOAD = value; }
        }

        public static void Reset()
        {
            DECAYLEVELCOUNTER = 0;
            LOOPNOISE = 0;
            CONSTANTVOLUME = 0;
            ENVELOPELOOPLENGTHHALT = 0;
            ENVELOPE = 0;
            NOISEPERIOD = 0;
            SHIFTREGISTER = 0;
            LENGTH = 0;
            VOLUME = 0;
            TIMERCOUNTER = 0;
            LENGTHCOUNTERLOAD = 0;

        }
        public static void ClockLength()
        {
            if (ENVELOPELOOPLENGTHHALT == 0 && LENGTH>0)
            {
                LENGTH--;
            }
        }
        public static void ClockEnvelope()
        {
            if (DECAYLEVELCOUNTER > 0)
            {
                DECAYLEVELCOUNTER--;
            }
            else
            {
                if (ENVELOPELOOPLENGTHHALT == 1 && DECAYLEVELCOUNTER == 0)
                {
                    DECAYLEVELCOUNTER = 15;
                }
                else if (DECAYLEVELCOUNTER > 0)
                {
                    DECAYLEVELCOUNTER--;
                }
                if (CONSTANTVOLUME == 1)
                {
                    VOLUME = ENVELOPE;
                }
                else
                {
                    VOLUME = DECAYLEVELCOUNTER;
                }
            }
        }
        public static void ClockTimer()
        {
            if (TIMERCOUNTER > 0)
            {
                TIMERCOUNTER--;
                int bitshift = 1;
                if (LOOPNOISE == 1) { bitshift = 6; }
                int bit1 = SHIFTREGISTER & 1;
                int bit2 = (SHIFTREGISTER >> bitshift) & 1;
                SHIFTREGISTER >>= 1;
                SHIFTREGISTER |= (bit1 ^ bit2) << 14;
            }
            else
            {
                TIMERCOUNTER = APU.NoiseTimerPeriodLookup[NOISEPERIOD];               
            }
        }
        public static byte Output()
        {
            byte noisefinal = 0;
            if (((APU.APU4015 >> 3) & 1) == 1 && LENGTH>0 && (SHIFTREGISTER&1) !=1)
            {
                if (CONSTANTVOLUME == 0)
                {
                    noisefinal = (byte)ENVELOPE;
                }
                else
                {
                    noisefinal = (byte)VOLUME;
                }
            }
            return noisefinal;
        }
        public static float OutputFrequency()
        {
            float noisefinal = 0;
            if (((APU.APU4015 >> 3) & 1) == 1 && LENGTH > 0 )
            {
                noisefinal = APU.NoiseTimerFrequency[NOISEPERIOD];
            }
            return noisefinal;
        }
    }
    class DMC
    {
        private static int _IRQENABLE;
        public static int IRQENABLE
        {
            get { return _IRQENABLE; }
            set { _IRQENABLE = value; }
        }

        private static int _BYTESREMAINING;
        public static int BYTESREMAINING
        {
            get { return _BYTESREMAINING; }
            set { _BYTESREMAINING = value; }
        }

        private static int _BITSREMAINING;
        public static int BITSREMAINING
        {
            get { return _BITSREMAINING; }
            set { _BITSREMAINING = value; }
        }

        private static int _LOOP;
        public static int LOOP
        {
            get { return _LOOP; }
            set { _LOOP = value; }
        }

        private static int _FREQUENCY;
        public static int FREQUENCY
        {
            get { return _FREQUENCY; }
            set { _FREQUENCY = value; }
        }

        private static int _LOADCOUNTER;
        public static int LOADCOUNTER
        {
            get { return _LOADCOUNTER; }
            set { _LOADCOUNTER = value; }
        }

        private static int _DMCOUTPUT;
        public static int DMCOUTPUT
        {
            get { return _DMCOUTPUT; }
            set { _DMCOUTPUT = value; }
        }

        private static int _SAMPLEADDRESS;
        public static int SAMPLEADDRESS
        {
            get { return _SAMPLEADDRESS; }
            set { _SAMPLEADDRESS = value; }
        }

        private static int _CURRENTADDRESS;
        public static int CURRENTADDRESS
        {
            get { return _CURRENTADDRESS; }
            set { _CURRENTADDRESS = value; }
        }

        private static int _SAMPLELENGTH;
        public static int SAMPLELENGTH
        {
            get { return _SAMPLELENGTH; }
            set { _SAMPLELENGTH = value; }
        }

        private static int _SAMPLEBUFFER;
        public static int SAMPLEBUFFER
        {
            get { return _SAMPLEBUFFER; }
            set { _SAMPLEBUFFER = value; }
        }
        
        private static int _TIMERCOUNTER;
        public static int TIMERCOUNTER
        {
            get { return _TIMERCOUNTER; }
            set { _TIMERCOUNTER = value; }
        }

        public static void Reset()
        {
            IRQENABLE = 0;
            BYTESREMAINING = 0;
            BITSREMAINING = 0;
            LOOP = 0;
            FREQUENCY = 0;
            LOADCOUNTER = 0;
            DMCOUTPUT = 0;
            SAMPLEADDRESS = 0;
            CURRENTADDRESS = 0;
            SAMPLELENGTH = 0;
            SAMPLEBUFFER = 0;
            TIMERCOUNTER = 0;
        }
        public static void FetchSample()
        {
            if (((APU.APU4015 >> 4) & 1) == 1)
            {
                if (BYTESREMAINING != 0)
                {
                    if (BITSREMAINING == 0) { BITSREMAINING = 8; }
                    //PPU.computescanlines2(4);
                    //CPU.AddCycles(4);
                    SAMPLEBUFFER = CPU.CPUMemory[CURRENTADDRESS];
                    CURRENTADDRESS++;
                    if (CURRENTADDRESS > 0xFFFF) { CURRENTADDRESS = CURRENTADDRESS - 0X8000; }
                    BYTESREMAINING--;
                    if (BYTESREMAINING == 0)
                    {
                        if (LOOP == 1)
                        {
                            CURRENTADDRESS = SAMPLEADDRESS;
                            BYTESREMAINING = SAMPLELENGTH;
                        }
                        else
                        {
                            APU.APU4015 |= 1 << 7;
                        }
                    }
                }
            }

            //if (((APU.apu4015 >> 4) & 1) == 1)
            //{
            //    if (BYTESREMAINING > 0 && BITSREMAINING == 0)
            //    {
            //        //if (BYTESREMAINING != SAMPLELENGTH)
            //        //{
            //        //    PPU.computescanlines2(4);
            //        //    CPU.AddCycles(4);
            //        //}
            //        int hshshs = TraceLog.INSTRUCTIONCOUNT;
            //        int ppus = PPU.PPUCycleCount;
            //        int hgdd = PPU.TOTALCYCLES;
            //        SAMPLEBUFFER = CPU.CPUMemory[CURRENTADDRESS];
            //        BITSREMAINING = 8;
            //        CURRENTADDRESS++;
            //        if (CURRENTADDRESS > 0xFFFF) { CURRENTADDRESS = CURRENTADDRESS - 0X8000; }
            //        BYTESREMAINING--;
            //        if (BYTESREMAINING == 0 && LOOP == 1)
            //        {
            //            CURRENTADDRESS = SAMPLEADDRESS;
            //            BYTESREMAINING = SAMPLELENGTH;
            //        }
            //        else if (BYTESREMAINING == 0 && IRQENABLE == 1)
            //        {
            //            APU.apu4015 |= 1 << 7;
            //        }
            //    }
            //}
        }
   
        public static void DMCShifter()
        {
            if (BITSREMAINING > 0)
            {
                if ((SAMPLEBUFFER & 1) == 1)
                {
                    if (DMCOUTPUT <= 125) { DMCOUTPUT += 2; }
                }
                else
                {
                    if (DMCOUTPUT >= 2) { DMCOUTPUT -= 2; }
                }
                SAMPLEBUFFER >>= 1;
                BITSREMAINING--;
            }
        }
        public static void ClockTimer()
        {
            if (((APU.APU4015 >> 4) & 1) == 1)
            {
                if (TIMERCOUNTER == 0)
                {
                    FetchSample();
                    TIMERCOUNTER = FREQUENCY;
                    DMCShifter();
                    //FetchSample();
                }
                else
                {
                    TIMERCOUNTER--;
                    TIMERCOUNTER--;
                }
        }
    }

            public static int Output()
        {
            return DMCOUTPUT;
        }
    }
}

