using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.Devices.Gpio;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SpiDisplayDriver
{

    public class ILI9341
    {

        private const string SPI_CONTROLLER_NAME = "SPI0";              // For Raspberry Pi 2 & 3, use SPI0
        private const Int32 SPI_CHIP_SELECT_LINE = 0;                   // Line 0 maps to physical pin number 24 on the Raspberry Pi 2 & 3
        private const Int32 DATA_COMMAND_PIN = 22;                      // We use GPIO 22 since it's conveniently near the SPI
        private const Int32 RESET_PIN = 23;                             // We use GPIO 23 since it's conveniently near the SPI pins

        public const uint LCD_W = 240;                                  // SPI Display width
        public const uint LCD_H = 320;                                  // SPI Display height
        private const uint LINE_HEIGHT = 32;

        private static readonly byte[] CMD_SLEEP_OUT = { 0x11 };
        private static readonly byte[] CMD_DISPLAY_ON = { 0x29 };
        private static readonly byte[] CMD_MEMORY_WRITE_MODE = { 0x2C };
        private static readonly byte[] CMD_DISPLAY_OFF = { 0x28 };
        private static readonly byte[] CMD_ENTER_SLEEP = { 0x10 };
        private static readonly byte[] CMD_COLUMN_ADDRESS_SET = { 0x2a };
        private static readonly byte[] CMD_PAGE_ADDRESS_SET = { 0x2b };
        private static readonly byte[] CMD_POWER_CONTROL_A = { 0xcb };
        private static readonly byte[] CMD_POWER_CONTROL_B = { 0xcf };
        private static readonly byte[] CMD_DRIVER_TIMING_CONTROL_A = { 0xe8 };
        private static readonly byte[] CMD_DRIVER_TIMING_CONTROL_B = { 0xea };
        private static readonly byte[] CMD_POWER_ON_SEQUENCE_CONTROL = { 0xed };
        private static readonly byte[] CMD_PUMP_RATIO_CONTROL = { 0xf7 };
        private static readonly byte[] CMD_POWER_CONTROL_1 = { 0xc0 };
        private static readonly byte[] CMD_POWER_CONTROL_2 = { 0xc1 };
        private static readonly byte[] CMD_VCOM_CONTROL_1 = { 0xc5 };
        private static readonly byte[] CMD_VCOM_CONTROL_2 = { 0xc7 };
        private static readonly byte[] CMD_MEMORY_ACCESS_CONTROL = { 0x36 };
        private static readonly byte[] CMD_PIXEL_FORMAT = { 0x3a };
        private static readonly byte[] CMD_FRAME_RATE_CONTROL = { 0xb1 };
        private static readonly byte[] CMD_DISPLAY_FUNCTION_CONTROL = { 0xb6 };
        private static readonly byte[] CMD_ENABLE_3G = { 0xf2 };
        private static readonly byte[] CMD_GAMMA_SET = { 0x26 };
        private static readonly byte[] CMD_POSITIVE_GAMMA_CORRECTION = { 0xe0 };
        private static readonly byte[] CMD_NEGATIVE_GAMMA_CORRECTION = { 0xe1 };

        private SpiDevice SpiDisplay;
        private GpioController IoController;
        private GpioPin DataCommandPin;
        private GpioPin ResetPin;

        private async Task InitHardware()
        {

            IoController = GpioController.GetDefault();
            DataCommandPin = IoController.OpenPin(DATA_COMMAND_PIN);
            DataCommandPin.Write(GpioPinValue.High);
            DataCommandPin.SetDriveMode(GpioPinDriveMode.Output);

            ResetPin = IoController.OpenPin(RESET_PIN);
            ResetPin.Write(GpioPinValue.High);
            ResetPin.SetDriveMode(GpioPinDriveMode.Output);

            var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE);
            settings.ClockFrequency = 10000000;
            settings.Mode = SpiMode.Mode3;
            string spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);
            var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);
            SpiDisplay = await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);
        }

        public async Task PowerOnSequence()
        {

            await InitHardware();
            await Task.Delay(5);
            ResetPin.Write(GpioPinValue.Low);
            await Task.Delay(5);
            ResetPin.Write(GpioPinValue.High);
            await Task.Delay(20);
            await Wakeup();
        }

        public async Task Wakeup()
        {

            DisplaySendCommand(CMD_SLEEP_OUT);
            await Task.Delay(60);

            DisplaySendCommand(CMD_POWER_CONTROL_A);
            DisplaySendData(new byte[] { 0x39, 0x2C, 0x00, 0x34, 0x02 });
            DisplaySendCommand(CMD_POWER_CONTROL_B);
            DisplaySendData(new byte[] { 0x00, 0xC1, 0x30 });
            DisplaySendCommand(CMD_DRIVER_TIMING_CONTROL_A);
            DisplaySendData(new byte[] { 0x85, 0x00, 0x78 });
            DisplaySendCommand(CMD_DRIVER_TIMING_CONTROL_B);
            DisplaySendData(new byte[] { 0x00, 0x00 });
            DisplaySendCommand(CMD_POWER_ON_SEQUENCE_CONTROL);
            DisplaySendData(new byte[] { 0x64, 0x03, 0x12, 0x81 });
            DisplaySendCommand(CMD_PUMP_RATIO_CONTROL);
            DisplaySendData(new byte[] { 0x20 });
            DisplaySendCommand(CMD_POWER_CONTROL_1);
            DisplaySendData(new byte[] { 0x23 });
            DisplaySendCommand(CMD_POWER_CONTROL_2);
            DisplaySendData(new byte[] { 0x10 });
            DisplaySendCommand(CMD_VCOM_CONTROL_1);
            DisplaySendData(new byte[] { 0x3e, 0x28 });
            DisplaySendCommand(CMD_VCOM_CONTROL_2);
            DisplaySendData(new byte[] { 0x86 });
            DisplaySendCommand(CMD_MEMORY_ACCESS_CONTROL);
            DisplaySendData(new byte[] { 0x48 });
            DisplaySendCommand(CMD_PIXEL_FORMAT);
            DisplaySendData(new byte[] { 0x55 });
            DisplaySendCommand(CMD_FRAME_RATE_CONTROL);
            DisplaySendData(new byte[] { 0x00, 0x18 });
            DisplaySendCommand(CMD_DISPLAY_FUNCTION_CONTROL);
            DisplaySendData(new byte[] { 0x08, 0x82, 0x27 });
            DisplaySendCommand(CMD_ENABLE_3G);
            DisplaySendData(new byte[] { 0x00 });
            DisplaySendCommand(CMD_GAMMA_SET);
            DisplaySendData(new byte[] { 0x01 });
            DisplaySendCommand(CMD_POSITIVE_GAMMA_CORRECTION);
            DisplaySendData(new byte[] { 0x0F, 0x31, 0x2B, 0x0C, 0x0E, 0x08, 0x4E, 0xF1, 0x37, 0x07, 0x10, 0x03, 0x0E, 0x09, 0x00 });
            DisplaySendCommand(CMD_NEGATIVE_GAMMA_CORRECTION);
            DisplaySendData(new byte[] { 0x00, 0x0E, 0x14, 0x03, 0x11, 0x07, 0x31, 0xC1, 0x48, 0x08, 0x0F, 0x0C, 0x31, 0x36, 0x0F });
            DisplaySendCommand(CMD_SLEEP_OUT);

            await Task.Delay(120);

            DisplaySendCommand(CMD_DISPLAY_ON);
        }

        public void Sleep()
        {

            DisplaySendCommand(CMD_DISPLAY_OFF);
            DisplaySendCommand(CMD_ENTER_SLEEP);
        }

        public void CleanUp()
        {

            SpiDisplay.Dispose();
            ResetPin.Dispose();
            DataCommandPin.Dispose();
        }

        private void SetAddress(uint x0, uint y0, uint x1, uint y1)
        {

            DisplaySendCommand(CMD_COLUMN_ADDRESS_SET);
            DisplaySendData(new byte[] { (byte)(x0 >> 8), (byte)(x0), (byte)(x1 >> 8), (byte)(x1) });
            DisplaySendCommand(CMD_PAGE_ADDRESS_SET);
            DisplaySendData(new byte[] { (byte)(y0 >> 8), (byte)(y0), (byte)(y1 >> 8), (byte)(y1) });
            DisplaySendCommand(CMD_MEMORY_WRITE_MODE);
        }

        public static ushort RGB888ToRGB565(byte r8, byte g8, byte b8)
        {

            ushort r5 = (ushort)((r8 * 249 + 1014) >> 11);
            ushort g6 = (ushort)((g8 * 253 + 505) >> 10);
            ushort b5 = (ushort)((b8 * 249 + 1014) >> 11);
            return (ushort)(r5 << 11 | g6 << 5 | b5);
        }

        private void DisplaySendData(byte[] Data)
        {

            DataCommandPin.Write(GpioPinValue.High);
            SpiDisplay.Write(Data);
        }

        private void DisplaySendCommand(byte[] Command)
        {

            DataCommandPin.Write(GpioPinValue.Low);
            SpiDisplay.Write(Command);
        }

        // Zone Pixel

        public void LCDPixel(uint colour, uint x0, uint y0)
        {

            uint x1 = x0 + 1;
            uint y1 = y0 + 1;

            byte VH = (byte)(colour >> 8);
            byte VL = (byte)(colour);
            SetAddress(x0, y0, x1, y1);
            byte[] buffer = new byte[2];
            int index = 0;

            for (int i = 0; i < 24; i++)
            {

                for (int j = 0; j < 32; j++)
                {

                    buffer[index++] = VH;
                    buffer[index++] = VL;
                }

            }

            DisplaySendData(buffer);

        }

        public void LCDPixel4040(uint colour)
        {

            LCDPixel(colour, 40, 40);
        }

        public void LCDPixelTest1()
        {

            LCDSquare01(RGB888ToRGB565(140, 0, 26));
        }



        // Zone Square

        public void LCDSquare(uint colour, uint x0, uint y0, uint x1, uint y1)
        {

            byte VH = (byte)(colour >> 8);
            byte VL = (byte)(colour);
            SetAddress( x0, y0, x1, y1);
            byte[] buffer = new byte[24 * 32 * 2];
            int index = 0;

            for (int i = 0; i < 24; i++)
            {

                for (int j = 0; j < 32; j++)
                {

                    buffer[index++] = VH;
                    buffer[index++] = VL;
                }

            }

            DisplaySendData(buffer);

        }

        public void LCDSquare00(uint colour)
        {
            LCDSquare(colour, 0, 0, 23, 31);
        }

        public void LCDSquare01(uint colour)
        {
            LCDSquare(colour, 0, 24, 47, 31);
        }
        
        public void LCDSquareTest1()
        {
            LCDSquare00(RGB888ToRGB565(0, 0, 0));
            LCDSquare01(RGB888ToRGB565(140, 0, 26));
        }

        // Zone Line

        public void LCDLine(uint colour, uint line)
        {

            byte VH = (byte)(colour >> 8);
            byte VL = (byte)(colour);
            SetAddress(0, line * LINE_HEIGHT, LCD_W - 1, (line + 1) * LINE_HEIGHT - 1);
            byte[] buffer = new byte[LCD_W * LINE_HEIGHT * 2];
            int index = 0;

            for (int i = 0; i < LCD_W; i++)
            {

                for (int j = 0; j < LINE_HEIGHT; j++)
                {

                    buffer[index++] = VH;
                    buffer[index++] = VL;
                }

            }

            DisplaySendData(buffer);

        }

        public void LCDClear(uint colour)
        {

            for (uint line = 0; line < LCD_H / LINE_HEIGHT; line++)
            {

                LCDLine(colour, line);
            }
        }

        public void LCDtest1()
        {

            uint colour = 0;

            for (uint line = 0; line < LCD_H / LINE_HEIGHT; line++)
            {

                LCDLine(colour, line);
            }
        }

        public void LCDtest2()
        {

            LCDLine(RGB888ToRGB565(0, 0, 0), 0);
            LCDLine(RGB888ToRGB565(255, 255, 255), 1);
            LCDLine(RGB888ToRGB565(140, 0, 26), 2);
            LCDLine(RGB888ToRGB565(0, 120, 215), 3);

            LCDLine(RGB888ToRGB565(0, 0, 0), 4);
            LCDLine(RGB888ToRGB565(255, 255, 255), 5);
            LCDLine(RGB888ToRGB565(140, 0, 26), 6);
            LCDLine(RGB888ToRGB565(0, 120, 215), 7);

            LCDLine(RGB888ToRGB565(0, 0, 0), 8);
            LCDLine(RGB888ToRGB565(255, 255, 255), 9);

        }

        // Zone File

        public async void LoadFile(ushort[] photo, uint w, uint h, string name)
        {

            StorageFile srcfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(name));

            using (IRandomAccessStream fileStream = await srcfile.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                BitmapTransform transform = new BitmapTransform()
                {

                    ScaledWidth = Convert.ToUInt32(w),
                    ScaledHeight = Convert.ToUInt32(h)
                };

                PixelDataProvider pixelData = await decoder.GetPixelDataAsync( BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);

                byte[] sourcePixels = pixelData.DetachPixelData();

                if (sourcePixels.Length == w * h * 4)
                {

                    int pi = 0;
                    int i = 0;
                    byte red = 0, green = 0, blue = 0;

                    foreach (byte b in sourcePixels)
                    {

                        switch (i)
                        {

                            case 0:
                                blue = b;
                                break;
                            case 1:
                                green = b;
                                break;
                            case 2:
                                red = b;
                                break;
                            case 3:
                                photo[pi] = RGB888ToRGB565(red, green, blue);
                                pi++;
                                break;
                        }

                        i = (i + 1) % 4;
                    }

                }
                else
                {

                    return;
                }

            }
        }

        // Zone Picture

        public void clearPicture(ushort[] picture)
        {

            if (picture.Length == LCD_W * LCD_H)
            {

                for (int i = 0; i < picture.Length; i++)
                {

                    picture[i] = RGB888ToRGB565(0, 0, 0);
                }

            }
            else
            {

                return;
            }

        }

        public void colorPicture(ushort[] picture, byte r, byte g, byte b)
        {

            if (picture.Length == LCD_W * LCD_H)
            {

                for (int i = 0; i < picture.Length; i++)
                {

                    picture[i] = RGB888ToRGB565(r, g, b);
                }

            }
            else
            {

                return;
            }

        }

        public void makeSquare(ushort[] square, int w, int h, byte r, byte g, byte b)
        {

            if (square.Length == w * h)
            {

                for (int i = 0; i < square.Length; i++)
                {

                    square[i] = RGB888ToRGB565(r, g, b);
                }

            }
            else
            {

                return;
            }

        }

        public void editPicture(ushort[] picture, ushort[] square, int w, int h, int x, int y)
        {

            for(int j = 0; j < h; j++)
            {

                for (int i = 0; i < w; i++)
                {

                    // Numéro de pixel du square
                    int s = i + j;

                    // Numéro de pixel du picture

                    int p = (j * 240) + (y * 240 + x + i);

                    picture[p] = square[s];
                }


            }

        }

        public void DrawPicture(ushort[] picture)
        {

            // si la taille de l'image == 240 x 320 = 76 800
            if (picture.Length == LCD_W * LCD_H)
            {

                // taille du block = 240 x 32 = 7 680
                int block_size = (int)(LCD_W * LINE_HEIGHT);

                // nombre de block = 76 800 / 7 680 = 10
                int number_of_blocks = picture.Length / block_size;

                // buffer = tableau de byte de taille = 7 680 x 2 = 15 360
                byte[] buffer = new byte[block_size * 2];

                int i = 0;
                uint line = 0;

                foreach (ushort s in picture)
                {

                    buffer[i * 2] = (byte)((s >> 8) & 0xFF);
                    buffer[i * 2 + 1] = (byte)(s & 0xFF);
                    i++;

                    if (i >= block_size)
                    {

                        i = 0;
                        SetAddress(0, line * LINE_HEIGHT, LCD_W - 1, (line + 1) * LINE_HEIGHT - 1);
                        DisplaySendData(buffer);
                        line++;
                    }

                }

            }
            else
            {

                return;
            }

        }

    }

}