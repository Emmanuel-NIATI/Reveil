using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.Devices.Gpio;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SpiDisplayDriver;
using System.Threading.Tasks;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reveil
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private GpioController _gpc;

        private GpioPin _GpioPin17;
        private GpioPin _GpioPin27;
        private GpioPin _GpioPin05;
        private GpioPin _GpioPin06;
        private GpioPin _GpioPin13;
        private GpioPin _GpioPin19;
        private GpioPin _GpioPin26;

        private MediaPlayer mediaPlayerNoir;
        private MediaPlayer mediaPlayerGris;
        private MediaPlayer mediaPlayerBlanc;
        private MediaPlayer mediaPlayerRouge;
        private MediaPlayer mediaPlayerVert;
        private MediaPlayer mediaPlayerBleu;
        private MediaPlayer mediaPlayerJaune;

        ILI9341_TFT_LCD_1 spiDisplayDriver = new ILI9341_TFT_LCD_1();

        public const uint LCD_W = ILI9341_TFT_LCD_1.LCD_W;
        public const uint LCD_H = ILI9341_TFT_LCD_1.LCD_H;

        ushort[] _picture = new ushort[LCD_W * LCD_H];

        ushort[] _rp = new ushort[LCD_W * LCD_H];
        ushort[] _windows = new ushort[LCD_W * LCD_H];
        ushort[] _justine = new ushort[LCD_W * LCD_H];

        ushort[] _black = new ushort[LCD_W * LCD_H];
        ushort[] _grey = new ushort[LCD_W * LCD_H];
        ushort[] _white = new ushort[LCD_W * LCD_H];
        ushort[] _red = new ushort[LCD_W * LCD_H];
        ushort[] _green = new ushort[LCD_W * LCD_H];
        ushort[] _blue = new ushort[LCD_W * LCD_H];
        ushort[] _yellow = new ushort[LCD_W * LCD_H];

        public MainPage()
        {
            this.InitializeComponent();

            this.initSpiSisplay();
            this.initGpio();

            this.travauxTimer();
        }

        private async void initSpiSisplay()
        {

            await spiDisplayDriver.PowerOnSequence();
            await spiDisplayDriver.Wakeup();

            spiDisplayDriver.LoadBitmap(_rp, "ms-appx:///assets/rp.png");
            spiDisplayDriver.LoadBitmap(_windows, "ms-appx:///assets/windows.png");
            spiDisplayDriver.LoadBitmap(_justine, "ms-appx:///assets/justine.jpg");

            spiDisplayDriver.LoadBitmap(_black, "ms-appx:///assets/black.jpg");
            spiDisplayDriver.LoadBitmap(_grey, "ms-appx:///assets/grey.jpg");
            spiDisplayDriver.LoadBitmap(_white, "ms-appx:///assets/white.jpg");
            spiDisplayDriver.LoadBitmap(_red, "ms-appx:///assets/red.jpg");
            spiDisplayDriver.LoadBitmap(_green, "ms-appx:///assets/green.jpg");
            spiDisplayDriver.LoadBitmap(_blue, "ms-appx:///assets/blue.jpg");
            spiDisplayDriver.LoadBitmap(_yellow, "ms-appx:///assets/yellow.jpg");

            _picture = _black;
        }

        private async void initGpio()
        {

            _gpc = GpioController.GetDefault();

            // Bouton Noir sur GPIO17
            _GpioPin17 = _gpc.OpenPin(17);
            _GpioPin17.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerNoir = new MediaPlayer();

            // Bouton Gris sur GPIO27
            _GpioPin27 = _gpc.OpenPin(27);
            _GpioPin27.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerGris = new MediaPlayer();

            // Bouton Blanc sur GPIO05
            _GpioPin05 = _gpc.OpenPin(5);
            _GpioPin05.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerBlanc = new MediaPlayer();

            // Bouton Rouge sur GPIO06
            _GpioPin06 = _gpc.OpenPin(6);
            _GpioPin06.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerRouge = new MediaPlayer();

            // Bouton Vert sur GPIO13
            _GpioPin13 = _gpc.OpenPin(13);
            _GpioPin13.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerVert = new MediaPlayer();

            // Bouton Bleu sur GPIO19
            _GpioPin19 = _gpc.OpenPin(19);
            _GpioPin19.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerBleu = new MediaPlayer();

            // Bouton Jaune sur GPIO26
            _GpioPin26 = _gpc.OpenPin(26);
            _GpioPin26.SetDriveMode(GpioPinDriveMode.Input);

            mediaPlayerJaune = new MediaPlayer();

        }

        
        private void travauxTimer()
        {

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
        }
        
        private async void dispatcherTimer_Tick(object sender, object e)
        {

            // Afficher l'image
            spiDisplayDriver.DrawPicture(_picture);

            // Bouton Noir
            if (_GpioPin17.Read() == GpioPinValue.High)
            {

                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Black");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _black;
            }

            // Bouton Gris
            if (_GpioPin27.Read() == GpioPinValue.High)
            {

                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Grey");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _grey;
            }

            // Bouton Blanc
            if (_GpioPin05.Read() == GpioPinValue.High)
            {

                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("White");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _white;
            }

            // Bouton Rouge
            if (_GpioPin06.Read() == GpioPinValue.High)
            {

                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Red");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _red;
            }

            // Bouton Vert
            if (_GpioPin13.Read() == GpioPinValue.High)
            {

                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Green");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _green;
            }

            // Bouton Bleu
            if (_GpioPin19.Read() == GpioPinValue.High)
            {

                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Blue");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _blue;
            }

            // Bouton Jaune
            if (_GpioPin26.Read() == GpioPinValue.High)
            {
                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Yellow");
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();

                _picture = _yellow;
            }

        }

    }

}
