using System;
namespace NesPlayer
{
    class PPU
    {
        public static int VERTICAL;
        public static int[] PPUMemory = new int[64 * 1024];
        public static int PPUCTRL2000;
        public static int PPUMASK2001;
        public static int PPUSTATUS2002;
        public static int OAMADDR2003;        
        public static int FineX = 0;
        public static int OAMDATA2004;
        public static int PPUSCROLL2005;
        public static int PPUADDR2006;
        public static int PPUDATA2007;
        public static int PPUCycleCount;
        public static int Scanlines;
        public static int OAMIndex;
        public static bool VBlankOccured;
        public static int AddOnePPU;
        public static int SpriteOverflowCount;       
        public static bool PPURegistersActive = false;
        public static byte[,] NESPallette = new byte[64, 3];
        public static int LoopyT;       
        public static int TOTALCYCLES;        
        public static int OAMReadBuffer;        
        public static int[] OAMMEM = new int[(512)];
        public static int[] SecondaryOAM = new int[(32)];
        public static int[,] SpriteOutputs = new int[8, 4];
        public static bool SpriteInRange;
        public static byte[] Pixels = new byte[245760];        
        public static int m2 = 0;
        public static int n2 = 0;
        public static int SpriteCount = 0;
        public static bool SpriteZeroOccuredThisFrame = false;
        public static int PPUScrollLow;
        public static int PPUScrollHigh;       
        public static int InternalReadBuffer2007;
        public static bool DISABLEWRITESTOSECONDOAM;       
        public static bool NMIOCCURED;        
        public static bool PPUScrollToggle;
        public static bool OddCycle;
        public static int VRAMLow;
        public static int VRAMHigh;

        public static void RunPPU(int Cycles)
        {
            int StartPPUCycleCount = PPUCycleCount;
            PPUCycleCount = PPUCycleCount + (Cycles * 3);
            if (PPUCycleCount > 340)
            {
                PPUCycleCount = PPUCycleCount - 341;
                Scanlines++;
                StartPPUCycleCount = 0;
                OAMIndex = 0;
                if (Scanlines == 261)
                {
                    Scanlines = -1;
                }
            }
            //Turn on ppu after 25000 cpu cycles
            if (TOTALCYCLES > 25000)
            {
                PPURegistersActive = true;
            }
            
            // add extra ppu cycle
            if ((Scanlines == 0 && VBlankOccured == true && PPUCycleCount >= 0) || (Scanlines == -1 && VBlankOccured == true && PPUCycleCount == 340))
            {
                if (AddOnePPU % 2 == 0 && ((((PPUMASK2001 >> 3) & 1) == 1) || (((PPUMASK2001 >> 4) & 1) == 1)))
                {
                    PPUCycleCount = PPUCycleCount + 1;
                    if (PPUCycleCount > 340)
                    {
                        PPUCycleCount = PPUCycleCount - 341;
                        Scanlines++;
                        StartPPUCycleCount = 0;
                        OAMIndex = 0;
                        if (Scanlines == 261)
                        {
                            Scanlines = -1;
                        }
                    }
                }
                NES.DrawFrame = true;
                VBlankOccured = false;
            }
            //clear 2002 on dot 0
            if (((Scanlines == -1 && PPUCycleCount >= (0)) && PPUCycleCount <= (4)) || (Scanlines == 261 && PPUCycleCount>339))
            {
                PPUSTATUS2002 &= ~(1 << 7);
                PPUSTATUS2002 &= ~(1 << 6);
                PPUSTATUS2002 &= ~(1 << 5);
                SpriteZeroOccuredThisFrame = false;
            }
            //check if rendering is enabled
            if ((Scanlines >= 0 && Scanlines < 240) && (((PPUMASK2001 >> 4) & 1) == 1 || ((PPUMASK2001 >> 3) & 1) == 1))
            {
                for (int Dot = StartPPUCycleCount; Dot < PPUCycleCount; Dot++)
                {
                    if (Dot < 0x100 && Dot >= 0)
                    {
                        if (Dot == 0 && Scanlines == 0)
                        {
                            SpriteOverflowCount = 0;
                            OAMIndex = 0;
                            PPUADDR2006 = LoopyT;
                            PPUADDR2006 &= ~0x7000;
                            PPUADDR2006 += (0x1000 * (PPUScrollLow % 8));
                        }
                        if (Dot == 0)
                        {
                            //Reset ppuaddr and finex for scrolling
                            if (VERTICAL == 1)
                            {
                                PPUADDR2006 = (PPUADDR2006 & ~0x0C00) | ((LoopyT >> 10 & 3) << 10);
                            }
                            FineX = PPUScrollHigh % 8;
                            PPUADDR2006 &= ~0x001F;
                            PPUADDR2006 += (LoopyT & 0x1F);
                        }
                        //clear secondary oam, reset spriteoverflow
                        if ((Dot > 0x1F) && Dot < 64)
                        {
                            SecondaryOAM[Dot - 32] = 0xFF;
                            SpriteOverflowCount = 0;
                        }
                        //get current sprite size
                        int spritesize = (8 + (8 * ((PPUCTRL2000 >> 5 & 1))));
                        //Find sprites to draw on this scanline
                        if ((Dot > 63) && OAMIndex < 257)
                        {
                            int yavlue = OAMMEM[OAMIndex];
                            if ((((Scanlines) >= yavlue) && ((Scanlines) < (yavlue + spritesize))) && yavlue >= 0 && SpriteOverflowCount < 32 && yavlue <= 0xef)
                            {
                                SecondaryOAM[SpriteOverflowCount] = OAMMEM[OAMIndex];
                                OAMIndex++;
                                SpriteOverflowCount++;
                                SecondaryOAM[SpriteOverflowCount] = OAMMEM[OAMIndex];
                                OAMIndex++;
                                SpriteOverflowCount++;
                                SecondaryOAM[SpriteOverflowCount] = OAMMEM[OAMIndex];
                                OAMIndex++;
                                SpriteOverflowCount++;
                                SecondaryOAM[SpriteOverflowCount] = OAMMEM[OAMIndex];
                                OAMIndex++;
                                SpriteOverflowCount++;
                            }
                            else
                            {
                                OAMIndex += 4;
                            }
                        }
                        int SpriteColorIndex = -1;
                        // Get background color
                        int BackGroundColor = (((PPUMemory[((PPUMemory[(0x2000 | (PPUADDR2006 & 0x0FFF))] * 16) + (((PPUCTRL2000 >> 4) & 1) * 0x1000)) + ((PPUADDR2006 >> 12 & 7))] >> (7 - FineX) & 1) | (PPUMemory[((PPUMemory[(0x2000 | (PPUADDR2006 & 0x0FFF))] * 16) + (((PPUCTRL2000 >> 4) & 1) * 0x1000)) + 8 + ((PPUADDR2006 >> 12 & 7))] >> (7 - FineX) & 1) << 1) + ((((PPUMemory[(0x23C0 | (PPUADDR2006 & 0x0C00) | ((PPUADDR2006 >> 4) & 0x38) | ((PPUADDR2006 >> 2) & 0x07))]) >> 2 * ((((((PPUADDR2006 >> 5)) & 0x1F)) & 0x02) | ((((PPUADDR2006 & 0x1F)) & 0x02) >> 1))) & 3) * 4));
                        if (BackGroundColor % 4 == 0) { BackGroundColor = 0; }
                        
                        int BackGroundColorIndex = PPUMemory[BackGroundColor + 0x3F00];
                        int PixelIndex = (((Scanlines * 0x100) + Dot) * 4);
                        // if background is enabled ,draw pixel to pixel array
                        if ((((PPUMASK2001 >> 3) & 1) == 1))
                        {
                            Pixels[PixelIndex] = NESPallette[BackGroundColorIndex, 2];
                            Pixels[PixelIndex + 1] = NESPallette[BackGroundColorIndex, 1];
                            Pixels[PixelIndex + 2] = NESPallette[BackGroundColorIndex, 0];
                            Pixels[PixelIndex + 3] = 0xFF;
                        }
                        // check if sprites are enabled
                        if ((((PPUMASK2001 >> 4) & 1) == 1))
                        {
                            bool SpritesInRange = false;
                            int InRangeSpriteIndex = -1;
                            //find sprites in range
                            for (int i = 0; i < 8; i++)
                            {
                                if (SpriteOutputs[i, 0] == -1 || SpriteOutputs[i, 0] == 0x100) { i = 7; }
                                else
                                {
                                    if ((((((Scanlines - 1) - (SpriteOutputs[i, 0]))) < spritesize) && ((((Scanlines - 1) - (SpriteOutputs[i, 0]))) >= 0)) && ((Dot - SpriteOutputs[i, 3]) < 8) && ((Dot - SpriteOutputs[i, 3]) >= 0) && !SpritesInRange)
                                    {
                                        InRangeSpriteIndex = i;
                                        SpritesInRange = true;
                                        i = 7;
                                    }
                                }
                            }
                            if (SpritesInRange)
                            {
                                //Get sprite color for current pixel
                                int SpriteColor = GetSpriteColor(InRangeSpriteIndex, Dot);
                                //if sprite color is not opaque
                                if (SpriteColor % 4 != 0)
                                {
                                    //check if rendering is enabled
                                    if (((((PPUMASK2001 >> 2) & 1) == 1) && (((PPUMASK2001 >> 1) & 1) == 1)) || Dot > 7)
                                    {
                                        //Check for sprite overflow
                                        if (!SpriteZeroOccuredThisFrame && Dot != 0 && Dot != 0xFF && ((((PPUMASK2001 >> 3) & 1) == 1) && (((PPUMASK2001 >> 4) & 1) == 1)) && BackGroundColor % 4 != 0 && (((PPUSTATUS2002 >> 6) & 1) != 1) && SpriteOutputs[InRangeSpriteIndex, 0] == OAMMEM[0] && SpriteOutputs[InRangeSpriteIndex, 1] == OAMMEM[1] && SpriteOutputs[InRangeSpriteIndex, 2] == OAMMEM[2] && SpriteOutputs[InRangeSpriteIndex, 3] == OAMMEM[3])
                                        {
                                            PPUSTATUS2002 |= 1 << 6;
                                            SpriteZeroOccuredThisFrame = true;
                                        }
                                    }
                                    //get sprite color
                                    SpriteColorIndex = PPUMemory[(SpriteColor) + 0x3F00 + 16 + ((SpriteOutputs[InRangeSpriteIndex, 2] & 3) * 4)];
                                    //check to see if sprite is hidden behind background
                                    if ((BackGroundColorIndex != SpriteColorIndex) && (((SpriteOutputs[InRangeSpriteIndex, 2] >> 5) & 1) == 0 || (((SpriteOutputs[InRangeSpriteIndex, 2] >> 5) & 1) == 1 && BackGroundColor % 4 == 0)) && !(Scanlines < 16 && Dot < 8))
                                    {
                                        Pixels[PixelIndex] = NESPallette[SpriteColorIndex, 2];
                                        Pixels[PixelIndex + 1] = NESPallette[SpriteColorIndex, 1];
                                        Pixels[PixelIndex + 2] = NESPallette[SpriteColorIndex, 0];
                                        Pixels[PixelIndex + 3] = 0xFF;
                                    }
                                }
                                else
                                {
                                    //Check for additional sprites for current pixel
                                    SpritesInRange = false;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        if ((((((Scanlines - 1) - (SpriteOutputs[i, 0]))) < spritesize) && ((((Scanlines - 1) - (SpriteOutputs[i, 0]))) >= 0) && (SpriteOutputs[i, 3] + 8) < 257) && ((Dot - SpriteOutputs[i, 3]) < 8) && ((Dot - SpriteOutputs[i, 3]) >= 0) && !SpritesInRange && InRangeSpriteIndex != i && SpriteOutputs[i, 1] != 0)
                                        {
                                            InRangeSpriteIndex = i;
                                            SpritesInRange = true;
                                            i = 7;
                                        }
                                    }
                                    if (SpritesInRange)
                                    {
                                        SpriteColor = GetSpriteColor(InRangeSpriteIndex, Dot);
                                        if (SpriteColor % 4 != 0)
                                        {
                                            if (((((PPUMASK2001 >> 2) & 1) == 1) && (((PPUMASK2001 >> 1) & 1) == 1)) || Dot > 7)
                                            {
                                               if (!SpriteZeroOccuredThisFrame && Dot != 0 && Dot != 0xFF && ((((PPUMASK2001 >> 3) & 1) == 1) && (((PPUMASK2001 >> 4) & 1) == 1)) && BackGroundColor % 4 != 0 && (((PPUSTATUS2002 >> 6) & 1) != 1) && SpriteOutputs[InRangeSpriteIndex, 0] == OAMMEM[0] && SpriteOutputs[InRangeSpriteIndex, 1] == OAMMEM[1] && SpriteOutputs[InRangeSpriteIndex, 2] == OAMMEM[2] && SpriteOutputs[InRangeSpriteIndex, 3] == OAMMEM[3])
                                                {
                                                    PPUSTATUS2002 |= 1 << 6;
                                                    SpriteZeroOccuredThisFrame = true;
                                                }
                                            }
                                            SpriteColorIndex = PPUMemory[(SpriteColor) + 0x3F00 + 16 + ((SpriteOutputs[InRangeSpriteIndex, 2] & 3) * 4)];
                                            if ((BackGroundColorIndex != SpriteColorIndex) && (((SpriteOutputs[InRangeSpriteIndex, 2] >> 5) & 1) == 0 || (((SpriteOutputs[InRangeSpriteIndex, 2] >> 5) & 1) == 1 && BackGroundColor % 4 == 0)) && !(Scanlines<16 && Dot<8))
                                            {
                                                Pixels[PixelIndex] = NESPallette[SpriteColorIndex, 2];
                                                Pixels[PixelIndex + 1] = NESPallette[SpriteColorIndex, 1];
                                                Pixels[PixelIndex + 2] = NESPallette[SpriteColorIndex, 0];
                                                Pixels[PixelIndex + 3] = 0xFF;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (FineX == 7 && Dot < 340)
                    {
                        // Coarse X increment
                        //if ((v & 0x001F) == 31) // if coarse X == 31
                        //    v &= ~0x001F          // coarse X = 0
                        //    v ^= 0x0400           // switch horizontal nametable
                        //    else
                        //    v += 1                // increment coarse X
                        FineX = 0;
                        if ((PPUADDR2006 & 0x001F) == 0x1F)
                        {
                            PPUADDR2006 &= ~0x001F;
                            PPUADDR2006 ^= 0x0400;
                        }
                        else { PPUADDR2006 += 1; }
                    }
                    else { FineX++; }

                    if (Dot == 0x100)
                    {
                        // Y increment
                        // if ((v & 0x7000) != 0x7000)        // if fine Y < 7
                        // v += 0x1000                      // increment fine Y
                        // else
                        // v &= ~0x7000                     // fine Y = 0
                        // int y = (v & 0x03E0) >> 5        // let y = coarse Y
                        // if (y == 29)
                        //y = 0                          // coarse Y = 0
                        //v ^= 0x0800                    // switch vertical nametable
                        // else if (y == 31)
                        // y = 0                          // coarse Y = 0, nametable not switched
                        // else
                        // y += 1                         // increment coarse Y
                        // v = (v & ~0x03E0) | (y << 5)     // put coarse Y back into v
                        if ((PPUADDR2006 & 0x7000) != 0x7000) { PPUADDR2006 += 0x1000; }
                        else
                        {
                            PPUADDR2006 &= ~0x7000;
                            int CoarseY = (((PPUADDR2006 >> 5)) & 0x1F);
                            if (CoarseY == 29)
                            {
                                CoarseY = 0;
                                PPUADDR2006 ^= 0x0800;
                            }
                            else if (CoarseY == 0x1F)
                            {
                                CoarseY = 0;
                            }
                            else
                            {
                                CoarseY += 1;
                            }
                            PPUADDR2006 = (PPUADDR2006 & ~0x03E0) | (CoarseY << 5);
                        }
                    }
                    if (Dot > 0 && Dot < 65)
                    {
                        //Disable Writes To SecondaryOAM
                        DISABLEWRITESTOSECONDOAM = false;
                        SpriteInRange = false;
                        //clear sprite indexes
                        n2 = 0; m2 = 0;
                    }
                    else if (Dot > 64 && Dot < 257 && Scanlines < 240)
                    {
                        if (Dot % 2 == 1)
                        {
                            //Write to read buffer on oddcyles
                            if (n2 < 64)
                            {
                                OAMReadBuffer = OAMMEM[(n2 * 4) + m2];
                            }
                        }
                        else if (SpriteInRange == false)
                        {
                            //Search for sprites in range
                            if (DISABLEWRITESTOSECONDOAM == false)
                            {
                                if ((((Scanlines + 1) >= OAMReadBuffer) && ((Scanlines + 1) < (OAMReadBuffer + (8 + (8 * ((PPUCTRL2000 >> 5 & 1))))))) && OAMReadBuffer > 0 && OAMReadBuffer <= 0xef)
                                {
                                    SpriteInRange = true;
                                    m2++;
                                }
                                else
                                { SpriteInRange = false; n2++; }
                            }
                            else
                            {
                                //check for sprite overflow
                                if ((((Scanlines + 1) >= OAMReadBuffer) && ((Scanlines + 1) < (OAMReadBuffer + (8 + (8 * ((PPUCTRL2000 >> 5 & 1))))))) && OAMReadBuffer > 0 && OAMReadBuffer <= 0xef)
                                {
                                    PPUSTATUS2002 |= 1 << 5;
                                    m2++;
                                    if (m2 == 4) { n2++; m2 = 0; }
                                }
                                else {n2++; m2++; if (m2 == 4) { m2 = 0; } }
                            }
                        }
                        else if (SpriteInRange == true)
                        {
                            m2++;
                            if (m2 == 4)
                            {
                                m2 = 0; n2++; SpriteInRange = false; SpriteCount++;
                                //If 8 sprites found disable writes to secondaryOAM
                                if (SpriteCount == 8)
                                {
                                    DISABLEWRITESTOSECONDOAM = true;
                                }
                            }
                        }
                    }
                    if (Dot >= 256 && Dot < 320)
                    {
                        //Load sprites for next scanline from secondaryoam to sprite units
                        if (Dot % 8 == 0)
                        {
                            int i = ((Dot - 256) / 8);
                            SpriteOutputs[i, 0] = SecondaryOAM[(i * 4)];
                            SpriteOutputs[i, 1] = SecondaryOAM[(i * 4) + 1];
                            SpriteOutputs[i, 2] = SecondaryOAM[(i * 4) + 2];
                            SpriteOutputs[i, 3] = SecondaryOAM[(i * 4) + 3];
                        }
                        n2 = 0; SpriteCount = 0;
                    }
                    GAME.TheMapper.SetTileLatch(((PPUMemory[(0x2000 | (PPUADDR2006 & 0x0FFF))] * 16) + (((PPUCTRL2000 >> 4) & 1) * 0x1000)));
                    GAME.TheMapper.SetA12Latch(Dot, false);
                }
            }
            if ((((Scanlines == 241 && PPUCycleCount >= (0))) && VBlankOccured == false) || (((Scanlines == 240 && PPUCycleCount >= (341))) && VBlankOccured == false))
            {
                AddOnePPU++;
                //clear vb flag
                if (PPURegistersActive)
                {
                    PPUSTATUS2002 |= 1 << 7;
                }
                //check for nmi irq
                if (PPUCTRL2000 > 127)
                {
                    NMIOCCURED = true;
                }
                VBlankOccured = true;
            }
        }
      
        // Get sprite color for current pixel
        public static int GetSpriteColor(int SecondOAMIndex,int dot)
        {
            
            int TileValue = SpriteOutputs[SecondOAMIndex, 1];
            if ((PPUCTRL2000 >> 5 & 1) == 1) { TileValue &= ~(1 << 0); }
            int YOffset = (Scanlines - 1) - (SpriteOutputs[SecondOAMIndex, 0]);
            // sprite size 8
            if (((Scanlines - 1) - SpriteOutputs[SecondOAMIndex, 0]) > 7)
            {
                TileValue = SpriteOutputs[SecondOAMIndex, 1];
                TileValue &= ~(1 << 0);
                TileValue += 1;
                YOffset = YOffset - 8;
            }
            // sprite size 16
            if ((8 + (8 * ((PPUCTRL2000 >> 5 & 1)))) == 16 && ((SpriteOutputs[SecondOAMIndex, 2] >> 7) & 1) == 1)
            {
                TileValue = SpriteOutputs[SecondOAMIndex, 1];
                TileValue &= ~(1 << 0);
                TileValue += 1;
                if (((Scanlines - 1) - SpriteOutputs[SecondOAMIndex, 0]) > 7)
                {
                    TileValue = SpriteOutputs[SecondOAMIndex, 1];
                    if ((PPUCTRL2000 >> 5 & 1) == 1) { TileValue &= ~(1 << 0); }
                }
            }
            if ((PPUCTRL2000 >> 5 & 1) == 0) { TileValue = TileValue + (((PPUCTRL2000 >> 3) & 1) * 0x100); } else { TileValue = TileValue + ((SpriteOutputs[SecondOAMIndex, 1] & 1) * 0x100); }
            if ((((SpriteOutputs[SecondOAMIndex, 2] >> 6) & 1) == 0 && ((SpriteOutputs[SecondOAMIndex, 2] >> 7) & 1) == 0))
            {
                return (((PPUMemory[(TileValue * 16) + (YOffset)] >> Math.Abs((dot - SpriteOutputs[SecondOAMIndex, 3]) - 7)) & 1) | (((PPUMemory[(TileValue * 16) + (YOffset) + 8] >> Math.Abs((dot - SpriteOutputs[SecondOAMIndex, 3]) - 7)) & 1) << 1));
            }
            else if ((((SpriteOutputs[SecondOAMIndex, 2] >> 6) & 1) == 0 && ((SpriteOutputs[SecondOAMIndex, 2] >> 7) & 1) == 1))
            {
                return (((PPUMemory[(TileValue * 16) + ((7) - YOffset)] >> Math.Abs((dot - SpriteOutputs[SecondOAMIndex, 3]) - 7)) & 1) | (((PPUMemory[(TileValue * 16) + ((7) - YOffset) + 8] >> Math.Abs((dot - SpriteOutputs[SecondOAMIndex, 3]) - 7)) & 1) << 1));
            }
            else if ((((SpriteOutputs[SecondOAMIndex, 2] >> 6) & 1) == 1 && ((SpriteOutputs[SecondOAMIndex, 2] >> 7) & 1) == 0))
            {
                return (((PPUMemory[(TileValue * 16) + (YOffset)] >> (Math.Abs(((7) - (dot - SpriteOutputs[SecondOAMIndex, 3])) - 7))) & 1) | (((PPUMemory[(TileValue * 16) + (YOffset) + 8] >> (Math.Abs(((7) - (dot - SpriteOutputs[SecondOAMIndex, 3])) - 7))) & 1) << 1));
            }
            else if ((((SpriteOutputs[SecondOAMIndex, 2] >> 6) & 1) == 1 && ((SpriteOutputs[SecondOAMIndex, 2] >> 7) & 1) == 1))
            {
                return (((PPUMemory[(TileValue * 16) + ((7) - YOffset)] >> Math.Abs(((7) - (dot - SpriteOutputs[SecondOAMIndex, 3])) - 7)) & 1) | (((PPUMemory[(TileValue * 16) + ((7) - YOffset) + 8] >> Math.Abs(((7) - (dot - SpriteOutputs[SecondOAMIndex, 3])) - 7)) & 1) << 1));
            }            
            return 0;
        }

        public static void WriteToPPURegister(int MemoryAddress, int Val)
        {
            if (MemoryAddress == 0x2000)
            {
                PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F);
                PPUCTRL2000 = Val;
                if (((PPUCTRL2000 >> 0) & 1) != ((LoopyT >> 10) & 1)) { LoopyT ^= 1 << 10; }
                if (((PPUCTRL2000 >> 1) & 1) != ((LoopyT >> 11) & 1)) { LoopyT ^= 1 << 11; }
            }
            else if (MemoryAddress == 0x2001)
            {
                PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F);
                PPUMASK2001 = Val;
            }
            else if (MemoryAddress == 0x2003)
            {
                PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F);
                OAMADDR2003 = Val;
            }
            else if (MemoryAddress == 0x2004)
            {
                PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F);
                OAMMEM[OAMADDR2003] = Val;
                OAMADDR2003 = OAMADDR2003 + 1;
            }
            else if (MemoryAddress == 0x2005)
            {
                if (PPUScrollToggle == false)
                {
                    PPUScrollHigh = Val;
                    PPUScrollToggle = true;
                    PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F); 
                    if (((PPUScrollHigh >> 0) & 1) != ((FineX >> 0) & 1)) { FineX ^= 1 << 0; }
                    if (((PPUScrollHigh >> 1) & 1) != ((FineX >> 1) & 1)) { FineX ^= 1 << 1; }
                    if (((PPUScrollHigh >> 2) & 1) != ((FineX >> 2) & 1)) { FineX ^= 1 << 2; }
                    if (((PPUScrollHigh >> 3) & 1) != ((LoopyT >> 0) & 1)) { LoopyT ^= 1 << 0; }
                    if (((PPUScrollHigh >> 4) & 1) != ((LoopyT >> 1) & 1)) { LoopyT ^= 1 << 1; }
                    if (((PPUScrollHigh >> 5) & 1) != ((LoopyT >> 2) & 1)) { LoopyT ^= 1 << 2; }
                    if (((PPUScrollHigh >> 6) & 1) != ((LoopyT >> 3) & 1)) { LoopyT ^= 1 << 3; }
                    if (((PPUScrollHigh >> 7) & 1) != ((LoopyT >> 4) & 1)) { LoopyT ^= 1 << 4; }
                }
                else
                {
                    PPUScrollLow = Val;
                    PPUScrollToggle = false;
                    PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F);
                    if (((PPUScrollLow >> 0) & 1) != ((LoopyT >> 12) & 1)) { LoopyT ^= 1 << 12; }
                    if (((PPUScrollLow >> 1) & 1) != ((LoopyT >> 13) & 1)) { LoopyT ^= 1 << 13; }
                    if (((PPUScrollLow >> 2) & 1) != ((LoopyT >> 14) & 1)) { LoopyT ^= 1 << 14; }
                    if (((PPUScrollLow >> 3) & 1) != ((LoopyT >> 5) & 1)) { LoopyT ^= 1 << 5; }
                    if (((PPUScrollLow >> 4) & 1) != ((LoopyT >> 6) & 1)) { LoopyT ^= 1 << 6; }
                    if (((PPUScrollLow >> 5) & 1) != ((LoopyT >> 7) & 1)) { LoopyT ^= 1 << 7; }
                    if (((PPUScrollLow >> 6) & 1) != ((LoopyT >> 8) & 1)) { LoopyT ^= 1 << 8; }
                    if (((PPUScrollLow >> 7) & 1) != ((LoopyT >> 9) & 1)) { LoopyT ^= 1 << 9; }
                }
            }
            else if (MemoryAddress == 0x2006)
            {
                if (PPUScrollToggle == false)
                {
                    VRAMHigh = Val;
                    PPUScrollToggle = true;
                    PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (VRAMHigh & 0x1F);
                    if (((VRAMHigh >> 0) & 1) != ((LoopyT >> 8) & 1)) { LoopyT ^= 1 << 8; }
                    if (((VRAMHigh >> 1) & 1) != ((LoopyT >> 9) & 1)) { LoopyT ^= 1 << 9; }
                    if (((VRAMHigh >> 2) & 1) != ((LoopyT >> 10) & 1)) { LoopyT ^= 1 << 10; }
                    if (((VRAMHigh >> 3) & 1) != ((LoopyT >> 11) & 1)) { LoopyT ^= 1 << 11; }
                    if (((VRAMHigh >> 4) & 1) != ((LoopyT >> 12) & 1)) { LoopyT ^= 1 << 12; }
                    if (((VRAMHigh >> 5) & 1) != ((LoopyT >> 13) & 1)) { LoopyT ^= 1 << 13; }
                    LoopyT &= ~(1 << 14);
                }
                else
                {
                    VRAMLow = Val;
                    PPUADDR2006 = (VRAMHigh) << 8 | (VRAMLow);
                    PPUScrollToggle = false;
                    PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (VRAMLow & 0x1F);
                    if (((VRAMLow >> 0) & 1) != ((LoopyT >> 0) & 1)) { LoopyT ^= 1 << 0; }
                    if (((VRAMLow >> 1) & 1) != ((LoopyT >> 1) & 1)) { LoopyT ^= 1 << 1; }
                    if (((VRAMLow >> 2) & 1) != ((LoopyT >> 2) & 1)) { LoopyT ^= 1 << 2; }
                    if (((VRAMLow >> 3) & 1) != ((LoopyT >> 3) & 1)) { LoopyT ^= 1 << 3; }
                    if (((VRAMLow >> 4) & 1) != ((LoopyT >> 4) & 1)) { LoopyT ^= 1 << 4; }
                    if (((VRAMLow >> 5) & 1) != ((LoopyT >> 5) & 1)) { LoopyT ^= 1 << 5; }
                    if (((VRAMLow >> 6) & 1) != ((LoopyT >> 6) & 1)) { LoopyT ^= 1 << 6; }
                    if (((VRAMLow >> 7) & 1) != ((LoopyT >> 7) & 1)) { LoopyT ^= 1 << 7; }
                    PPUADDR2006 = LoopyT;
                }
            }
            else if (MemoryAddress == 0x2007)
            {
                PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (Val & 0x1F);
                PPUDATA2007 = Val;
                if (PPUADDR2006 < 0x2000)
                {
                    WriteToPPUMemory(PPUADDR2006, PPUDATA2007);
                }
                else
                {
                    WriteToPPUMemory(PPUADDR2006, Val);
                }
                if (PPUADDR2006 >= 0x2000 && PPUADDR2006 < 0x3000)
                {
                    if (VERTICAL == 0)
                    {
                        if (PPUADDR2006 >= 0x2000 && PPUADDR2006 < 0x2400)
                        {
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2400 && PPUADDR2006 < 0x2800)
                        {
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2800 && PPUADDR2006 < 0x2c00)
                        {
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2c00 && PPUADDR2006 < 0x3000)
                        {
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                        }
                    }
                    else if (VERTICAL == 1)
                    {
                        if (PPUADDR2006 >= 0x2000 && PPUADDR2006 < 0x2400)
                        {
                            PPUMemory[PPUADDR2006 + 0x800] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2400 && PPUADDR2006 < 0x2800)
                        {
                            PPUMemory[PPUADDR2006 + 0x800] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2800 && PPUADDR2006 < 0x2c00)
                        {
                            PPUMemory[PPUADDR2006 - 0x800] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2c00 && PPUADDR2006 < 0x3000)
                        {
                            PPUMemory[PPUADDR2006 - 0x800] = Val;
                        }
                    }
                    else if (VERTICAL == 2)
                    {
                        if (PPUADDR2006 >= 0x2000 && PPUADDR2006 < 0x2400)
                        {
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                            PPUMemory[PPUADDR2006 + 0x800] = Val;
                            PPUMemory[PPUADDR2006 + 0xC00] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2400 && PPUADDR2006 < 0x2800)
                        {
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                            PPUMemory[PPUADDR2006 + 0x800] = Val;
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2800 && PPUADDR2006 < 0x2c00)
                        {
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                            PPUMemory[PPUADDR2006 - 0x800] = Val;
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2c00 && PPUADDR2006 < 0x3000)
                        {
                            PPUMemory[PPUADDR2006 - 0xC00] = Val;
                            PPUMemory[PPUADDR2006 - 0x800] = Val;
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                        }
                    }
                    else if (VERTICAL == 3)
                    {
                        if (PPUADDR2006 >= 0x2000 && PPUADDR2006 < 0x2400)
                        {
                            PPUMemory[PPUADDR2006 + 0xC00] = Val;
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                            PPUMemory[PPUADDR2006 + 0x800] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2400 && PPUADDR2006 < 0x2800)
                        {
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                            PPUMemory[PPUADDR2006 + 0x800] = Val;
                        }
                        else if (PPUADDR2006 >= 0x2800 && PPUADDR2006 < 0x2C00)
                        {
                            PPUMemory[PPUADDR2006 - 0x400] = Val;
                            PPUMemory[PPUADDR2006 + 0x400] = Val;
                            PPUMemory[PPUADDR2006 - 0x800] = Val;
                        }
                    }
                }
                PPUADDR2006 = (((PPUCTRL2000 >> 2 & 1) * 0x1F) + 1) + PPUADDR2006;
            }
        }

        public static void WriteToPPUMemory(int MemoryAddress, int Val)
        {
            MemoryAddress &= 0x3FFF;
            // Write to Character RAM
            if(MemoryAddress < 0x2000)
            {
                GAME.TheMapper.WriteCHRRAM(MemoryAddress, Val);
                if (GAME.MAPPERNUMBER == 5 && INES5.UseMMC5Banks)
                {
                    GAME.MMC5Sprite16[MemoryAddress] = Val;
                }                
            }
            // Write to Pallete
            if (MemoryAddress >= 0x3f00)
            {
                if (MemoryAddress == 0x3f00 || MemoryAddress == 0x3f10 || MemoryAddress == 0x3f20 || MemoryAddress == 0x3f30 || MemoryAddress == 0x3f40 || MemoryAddress == 0x3f50)
                {
                    PPUMemory[0x3F00] = Val & 0x3F;
                    PPUMemory[0x3F10] = Val & 0x3F;
                }
                MemoryAddress = ((MemoryAddress - 0x3f00) % 32) + 0x3F00;
                PPUMemory[MemoryAddress] = Val & 0x3F;
            }
            else
            {
                PPUMemory[MemoryAddress] = Val;
            }
        }
        public static int ReadPPURegister(int MemoryAddress)
        {
            int val = 0;
            switch (MemoryAddress)
            {
                case 0x2000:
                    break;
                case 0x2001:
                    break;
                case 0x2002:
                    // Check if vblank will occur on this cycle
                    if ((((Scanlines == 240 && PPUCycleCount >= (338))) && VBlankOccured == false) || (((Scanlines == 241 && PPUCycleCount <= (1))) && VBlankOccured == false))
                    {
                        AddOnePPU++;
                        if (PPURegistersActive)
                        {
                            PPUSTATUS2002 |= 1 << 7;
                        }
                        VBlankOccured = true;
                    }
                    // return ppustatus before clearing flag
                    int StatusOutput = PPUSTATUS2002;
                    if (((CPU.CycleArray[CPU.CurrentOpCode].Length - CPU.CycleNumber) == 1 && (PPUCycleCount == 338) && Scanlines == 240))
                    {
                        StatusOutput &= ~(1 << 7);
                        NMIOCCURED = false;
                    }
                    //Clear VB Flag
                    PPUSTATUS2002 &= ~(1 << 7);
                    //Reset PPUScrollToggle
                    PPUScrollToggle = false;
                    return StatusOutput;
                case 0x2003:
                    break;
                case 0x2004:
                    return OAMMEM[OAMADDR2003];
                case 0x2005:
                    break;
                case 0x2006:
                    break;
                case 0x2007:
                    int DataOutput = 0;
                    int offset = 0;
                    if (PPUADDR2006 < 0x3F00)
                    {
                        //ppu read gets buffered
                        DataOutput = InternalReadBuffer2007;
                        if (PPUADDR2006 >= 0x3000)
                        {
                            offset = 0x1000;
                        }
                        PPUDATA2007 = PPUMemory[PPUADDR2006-offset];
                    }
                    else
                    {
                        // Read from pallete no buffering
                        PPUDATA2007 = PPUMemory[PPUADDR2006];
                        DataOutput = PPUDATA2007;
                    }
                    InternalReadBuffer2007 = PPUMemory[PPUADDR2006 - offset];
                    PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (PPUDATA2007 & 0x1F);
                    //Increment PPU Addr
                    if ((((PPUCTRL2000 & (1 << 2)) * 8) / 32) == 1)
                    {
                        PPUADDR2006 = PPUADDR2006 + 32;
                    }
                    else
                    {
                        PPUADDR2006 = PPUADDR2006 + 1;
                    }
                    return DataOutput;
            }
            return val;
        }
      
        public static void Reset()
        {
            InternalReadBuffer2007 = 0;
            OddCycle = false;
            PPURegistersActive = false;
            VERTICAL = 0;
            PPUMemory = new int[64 * 1024];
            PPUCTRL2000 = 0;
            PPUSCROLL2005 = 0;
            PPUSTATUS2002 = 0;
            AddOnePPU = 0;
            OAMADDR2003 = 0;
            OAMDATA2004 = 0;
            OAMIndex = 0;
            PPUScrollLow = 0;
            SpriteOverflowCount = 0;
            SpriteInRange = false;
            PPUADDR2006 = 0;
            PPUDATA2007 = 0;
            PPUMASK2001 = 0;
            PPUScrollHigh = 0;
            TOTALCYCLES = 0;
            PPUCycleCount = 0;
            LoopyT = 0;
            SpriteOutputs = new int[8, 4];
            OAMMEM = new int[(512)];
            SecondaryOAM = new int[(512)];
            FineX = 0;
            VBlankOccured = false;
            Scanlines = 241;
            OAMReadBuffer = 0;
            Pixels = new byte[245760];
            m2 = 0;
            n2 = 0;
            SpriteCount = 0;
            DISABLEWRITESTOSECONDOAM = false;
            NMIOCCURED = false;
            VRAMLow = 0;
            VRAMHigh = 0;
        
            NESPallette[0, 0] = 124; NESPallette[0, 1] = 124; NESPallette[0, 2] = 124;
            NESPallette[1, 0] = 0; NESPallette[1, 1] = 0; NESPallette[1, 2] = 252;
            NESPallette[2, 0] = 0; NESPallette[2, 1] = 0; NESPallette[2, 2] = 188;
            NESPallette[3, 0] = 68; NESPallette[3, 1] = 40; NESPallette[3, 2] = 188;
            NESPallette[4, 0] = 148; NESPallette[4, 1] = 0; NESPallette[4, 2] = 132;
            NESPallette[5, 0] = 168; NESPallette[5, 1] = 0; NESPallette[5, 2] = 32;
            NESPallette[6, 0] = 168; NESPallette[6, 1] = 16; NESPallette[6, 2] = 0;
            NESPallette[7, 0] = 136; NESPallette[7, 1] = 20; NESPallette[7, 2] = 0;
            NESPallette[8, 0] = 80; NESPallette[8, 1] = 48; NESPallette[8, 2] = 0;
            NESPallette[9, 0] = 0; NESPallette[9, 1] = 120; NESPallette[9, 2] = 0;
            NESPallette[10, 0] = 0; NESPallette[10, 1] = 104; NESPallette[10, 2] = 0;
            NESPallette[11, 0] = 0; NESPallette[11, 1] = 88; NESPallette[11, 2] = 0;
            NESPallette[12, 0] = 0; NESPallette[12, 1] = 64; NESPallette[12, 2] = 88;
            NESPallette[13, 0] = 0; NESPallette[13, 1] = 0; NESPallette[13, 2] = 0;
            NESPallette[14, 0] = 0; NESPallette[14, 1] = 0; NESPallette[14, 2] = 0;
            NESPallette[15, 0] = 0; NESPallette[15, 1] = 0; NESPallette[15, 2] = 0;
            NESPallette[16, 0] = 188; NESPallette[16, 1] = 188; NESPallette[16, 2] = 188;
            NESPallette[17, 0] = 0; NESPallette[17, 1] = 120; NESPallette[17, 2] = 248;
            NESPallette[18, 0] = 0; NESPallette[18, 1] = 88; NESPallette[18, 2] = 248;
            NESPallette[19, 0] = 104; NESPallette[19, 1] = 68; NESPallette[19, 2] = 252;
            NESPallette[20, 0] = 216; NESPallette[20, 1] = 0; NESPallette[20, 2] = 204;
            NESPallette[21, 0] = 228; NESPallette[21, 1] = 0; NESPallette[21, 2] = 88;
            NESPallette[22, 0] = 248; NESPallette[22, 1] = 56; NESPallette[22, 2] = 0;
            NESPallette[23, 0] = 228; NESPallette[23, 1] = 92; NESPallette[23, 2] = 16;
            NESPallette[24, 0] = 172; NESPallette[24, 1] = 124; NESPallette[24, 2] = 0;
            NESPallette[25, 0] = 0; NESPallette[25, 1] = 184; NESPallette[25, 2] = 0;
            NESPallette[26, 0] = 0; NESPallette[26, 1] = 168; NESPallette[26, 2] = 0;
            NESPallette[27, 0] = 0; NESPallette[27, 1] = 168; NESPallette[27, 2] = 68;
            NESPallette[28, 0] = 0; NESPallette[28, 1] = 136; NESPallette[28, 2] = 136;
            NESPallette[29, 0] = 0; NESPallette[29, 1] = 0; NESPallette[29, 2] = 0;
            NESPallette[30, 0] = 0; NESPallette[30, 1] = 0; NESPallette[30, 2] = 0;
            NESPallette[0x1F, 0] = 0; NESPallette[0x1F, 1] = 0; NESPallette[0x1F, 2] = 0;
            NESPallette[32, 0] = 248; NESPallette[32, 1] = 248; NESPallette[32, 2] = 248;
            NESPallette[33, 0] = 60; NESPallette[33, 1] = 188; NESPallette[33, 2] = 252;
            NESPallette[34, 0] = 104; NESPallette[34, 1] = 136; NESPallette[34, 2] = 252;
            NESPallette[35, 0] = 152; NESPallette[35, 1] = 120; NESPallette[35, 2] = 248;
            NESPallette[36, 0] = 248; NESPallette[36, 1] = 120; NESPallette[36, 2] = 248;
            NESPallette[37, 0] = 248; NESPallette[37, 1] = 88; NESPallette[37, 2] = 152;
            NESPallette[38, 0] = 248; NESPallette[38, 1] = 120; NESPallette[38, 2] = 88;
            NESPallette[39, 0] = 252; NESPallette[39, 1] = 160; NESPallette[39, 2] = 68;
            NESPallette[40, 0] = 248; NESPallette[40, 1] = 184; NESPallette[40, 2] = 0;
            NESPallette[41, 0] = 184; NESPallette[41, 1] = 248; NESPallette[41, 2] = 24;
            NESPallette[42, 0] = 88; NESPallette[42, 1] = 216; NESPallette[42, 2] = 84;
            NESPallette[43, 0] = 88; NESPallette[43, 1] = 248; NESPallette[43, 2] = 152;
            NESPallette[44, 0] = 0; NESPallette[44, 1] = 232; NESPallette[44, 2] = 216;
            NESPallette[45, 0] = 120; NESPallette[45, 1] = 120; NESPallette[45, 2] = 120;
            NESPallette[46, 0] = 0; NESPallette[46, 1] = 0; NESPallette[46, 2] = 0;
            NESPallette[47, 0] = 0; NESPallette[47, 1] = 0; NESPallette[47, 2] = 0;
            NESPallette[48, 0] = 252; NESPallette[48, 1] = 252; NESPallette[48, 2] = 252;
            NESPallette[49, 0] = 164; NESPallette[49, 1] = 228; NESPallette[49, 2] = 252;
            NESPallette[50, 0] = 184; NESPallette[50, 1] = 184; NESPallette[50, 2] = 248;
            NESPallette[51, 0] = 216; NESPallette[51, 1] = 184; NESPallette[51, 2] = 248;
            NESPallette[52, 0] = 248; NESPallette[52, 1] = 184; NESPallette[52, 2] = 248;
            NESPallette[53, 0] = 248; NESPallette[53, 1] = 164; NESPallette[53, 2] = 192;
            NESPallette[54, 0] = 240; NESPallette[54, 1] = 208; NESPallette[54, 2] = 176;
            NESPallette[55, 0] = 252; NESPallette[55, 1] = 0xE0; NESPallette[55, 2] = 168;
            NESPallette[56, 0] = 248; NESPallette[56, 1] = 216; NESPallette[56, 2] = 120;
            NESPallette[57, 0] = 216; NESPallette[57, 1] = 248; NESPallette[57, 2] = 120;
            NESPallette[58, 0] = 184; NESPallette[58, 1] = 248; NESPallette[58, 2] = 184;
            NESPallette[59, 0] = 184; NESPallette[59, 1] = 248; NESPallette[59, 2] = 216;
            NESPallette[60, 0] = 0; NESPallette[60, 1] = 252; NESPallette[60, 2] = 252;
            NESPallette[61, 0] = 248; NESPallette[61, 1] = 216; NESPallette[61, 2] = 248;
            NESPallette[62, 0] = 0; NESPallette[62, 1] = 0; NESPallette[62, 2] = 0;
            NESPallette[63, 0] = 0; NESPallette[63, 1] = 0; NESPallette[63, 2] = 0;
        }
        public static void WriteToOAMRegister(int Val)
        {
            if (((TOTALCYCLES - 1) & 1) != 0) { OddCycle = true; } 
            if (OddCycle)
            {
                CPU.AddCycles(1);
                RunPPU(1);
                APU.RunAPU(1);
            }
            else
            {
                CPU.AddCycles(2);
                RunPPU(2);
                APU.RunAPU(2);
            }
            for (int k = (Val * 0x100); k < ((Val * 0x100) + 0x100); k++)
            {
                OAMMEM[((k - (Val * 0x100))+OAMADDR2003)&0xFF] = CPU.CPUMemory[k];
                PPUSTATUS2002 = (PPUSTATUS2002 & 0xE0) | (CPU.CPUMemory[k] & 0x1F);
                CPU.AddCycles(2);
                RunPPU(2);
                APU.RunAPU(2);
            }
         }
    }   
}
