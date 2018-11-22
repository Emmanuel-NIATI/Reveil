using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.Devices.Gpio;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
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
using System.Threading.Tasks;
using Windows.UI;
using ReveilDriver;
using Windows.UI.Xaml.Media.Imaging;
using SpiDisplayDriver;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reveil
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // Variables liées aux librairies
        private static readonly UtilReveilDriver _UtilReveilDriver = new UtilReveilDriver();
        private static readonly ILI9341 _SpiDisplayDriver = new ILI9341();

        // Variables liées à l'affichage
        private String _Date = "lun 30/04/2018";

        private String _H_AFF = "08";
        private String _M_AFF = "30";
        private String _S_AFF = "00";
        private String _L_AFF = "00";

        private String _H_REG = "06";
        private String _M_REG = "30";

        private String _H_SON = "06";
        private String _M_SON = "30";

        private String _I_LOGO = "maker";

        private String _TYP_C = "Sonnerie";
        private String _SON_C = "Dring !";
        private String _RAD_C = "France Inter";
        private String _MP3_C = "Indochine - Des fleurs pour Salinger.mp3";

        // Variables liées au mode de fonctionnemant
        private String _MODE = "HORLOGE";
        private String _TYPE = "SONNERIE";

        // Variables liées au Reveil
        private String reveil = "0";

        private MediaPlayer mediaPlayerReveil;

        private MediaSource mediaSourceMP3;
        private MediaSource mediaSourceRadio;
        private MediaSource mediaSourceSonnerie;

        private MediaElement mediaElementReveil;

        // Variables liées aux Boutons
        private String BTN_GRIS = "Mode";
        private String BTN_BLANC = "";
        private String BTN_VERT = "";
        private String BTN_BLEU = "";
        private String BTN_JAUNE = "";

        // Variables liées au GPIO
        private GpioController _gpc;

        private GpioPin _pin27;
        private GpioPin _pin05;
        private GpioPin _pin13;
        private GpioPin _pin19;
        private GpioPin _pin26;

        // Variables liées à l'affichage LCD
        public const uint LCD_W = ILI9341.LCD_W;
        public const uint LCD_H = ILI9341.LCD_H;

        ushort[] _lcd = new ushort[LCD_W * LCD_H];

        ushort[] _justine01 = new ushort[LCD_W * LCD_H];
        ushort[] _justine02 = new ushort[60 * 60];

        ushort[] _maker02 = new ushort[LCD_W * LCD_H];

        ushort[] _point = new ushort[1 * 1];
        ushort[] _square = new ushort[40 * 40];
        ushort[] _rect = new ushort[60 * 40];

        ushort[] _4 = new ushort[60 * 60];

        public MainPage()
        {

            this.InitializeComponent();

            // Initialisation des variables
            this.initVariable();

            // Initialisation du GPIO
            this.initGpio();

            // Initialisation de l'affichage
            this.initSpiDisplay();
            
            // Initialisation du Timer
            this.travauxTimer();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            // Bouton Gris sur GPIO27
            _pin27.ValueChanged += _pin27_ValueChanged;

            // Bouton Blanc sur GPIO05
            _pin05.ValueChanged += _pin05_ValueChanged;

            // Bouton Vert sur GPIO13
            _pin13.ValueChanged += _pin13_ValueChanged;

            // Bouton Bleu sur GPIO19
            _pin19.ValueChanged += _pin19_ValueChanged;

            // Bouton Jaune sur GPIO26
            _pin26.ValueChanged += _pin26_ValueChanged;

        }

        private void initVariable()
        {

            // Chargement des images
            //_SpiDisplayDriver.LoadFile(_justine01, 240, 320, "ms-appx:///assets/justine01.jpg");
            //_SpiDisplayDriver.LoadFile(_justine02, 60, 60, "ms-appx:///assets/justine02.jpg");
            _SpiDisplayDriver.LoadFile(_maker02, 240, 320, "ms-appx:///assets/maker02.png");
            
            // Mise à la date et heure
            DATE.Text = _Date;
            H_AFF.Text = _H_AFF;
            M_AFF.Text = _M_AFF;
            S_AFF.Text = _S_AFF;

            // Connexion RJ45
            // Connexion Wifi
            // Connexion Bluetooth

            // Initialisation de l'image
            if (_I_LOGO.Equals("maker"))
            {

                I_LOGO.Source = new BitmapImage(new Uri("ms-appx:///Assets/maker.png"));
            }

            // Initialisation du Mode
            if (_MODE.Equals("HORLOGE"))
            {

                MOD_T.Text = "Horloge";
            }
            else if (_MODE.Equals("REVEIL_SET"))
            {

                MOD_T.Text = "Réveil (réglage)";
            }
            else if (_MODE.Equals("REVEIL_ON"))
            {

                MOD_T.Text = "Réveil activé !";
            }
            else if (_MODE.Equals("REVEIL_TYPE"))
            {

                MOD_T.Text = "Réveil (type)";
            }
            else if (_MODE.Equals("MP3"))
            {

                MOD_T.Text = "MP3 (choix)";
            }
            else if (_MODE.Equals("RADIO"))
            {

                MOD_T.Text = "Radio (choix)";
            }
            else if (_MODE.Equals("SONNERIE"))
            {

                MOD_T.Text = "Sonnerie (choix)";
            }

            // Initialisation du reveil
            H_REG.Text = _H_REG;
            M_REG.Text = _M_REG;

            H_SON.Text = _H_SON;
            M_SON.Text = _M_SON;

            // Initialisation du type de réveil
            if (_TYPE.Equals("SONNERIE"))
            {

                TYP_C.Text = "Sonnerie";
            }
            else if (_TYPE.Equals("RADIO"))
            {

                TYP_C.Text = "Radio";
            }
            else if (_TYPE.Equals("MP3"))
            {

                TYP_C.Text = "MP3";
            }

            // Initialisation des paramètres du reveil
            TYP_C.Text = _TYP_C;
            SON_C.Text = _SON_C;
            RAD_C.Text = _RAD_C;
            MP3_C.Text = _MP3_C;

            // Initialisation des bouton
            Btn_Gris.Text = BTN_GRIS;
            Btn_Blanc.Text = BTN_BLANC;
            Btn_Vert.Text = BTN_VERT;
            Btn_Bleu.Text = BTN_BLEU;
            Btn_Jaune.Text = BTN_JAUNE;

            // Préparation du réveil
            mediaPlayerReveil = new MediaPlayer();

            // Préparation du MP3
            mediaSourceMP3 = MediaSource.CreateFromUri( new Uri( "ms-appx:///MP3/Indochine - Des fleurs pour Salinger.mp3" ) );

            // Préparation de la Radio
            mediaSourceRadio = MediaSource.CreateFromUri( new Uri( "http://direct.franceinter.fr/live/franceinter-lofi.mp3" ) );

            // Préparation de la Sonnerie
            mediaSourceSonnerie = MediaSource.CreateFromUri( new Uri( "ms-appx:///Sonneries/dring.mp3" ) );

        }

        private void initGpio()
        {

            // Configuration du contrôleur du GPIO par défaut
            _gpc = GpioController.GetDefault();

            // Bouton Gris sur GPIO27 en entrée
            _pin27 = _gpc.OpenPin(27);
            _pin27.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin27.DebounceTimeout = new TimeSpan(10000);

            // Bouton Blanc sur GPIO05 en entrée
            _pin05 = _gpc.OpenPin(5);
            _pin05.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin05.DebounceTimeout = new TimeSpan(10000);

            // Bouton Vert sur GPIO13 en entrée
            _pin13 = _gpc.OpenPin(13);
            _pin13.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin13.DebounceTimeout = new TimeSpan(10000);

            // Bouton Bleu sur GPIO19 en entrée
            _pin19 = _gpc.OpenPin(19);
            _pin19.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin19.DebounceTimeout = new TimeSpan(10000);

            // Bouton Jaune sur GPIO26 en entrée
            _pin26 = _gpc.OpenPin(26);
            _pin26.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin26.DebounceTimeout = new TimeSpan(10000);

        }

        private async void initSpiDisplay()
        {

            // Initialisation de l'affichage
            await _SpiDisplayDriver.PowerOnSequence();
            await _SpiDisplayDriver.Wakeup();

            // Colorer l'affichage
            _SpiDisplayDriver.clearPicture(_lcd);
            _SpiDisplayDriver.colorPicture(_lcd, 255, 255, 255);

            // Editer l'affichage
            _SpiDisplayDriver.makeSquare(_square, 40, 40, 180, 120, 0);
            _SpiDisplayDriver.makeSquare(_point, 1, 1, 0, 0, 0);
            _SpiDisplayDriver.editPicture(_lcd, _square, 40, 40, 60, 240);
            _SpiDisplayDriver.editPicture(_lcd, _square, 40, 40, 120, 240);
            _SpiDisplayDriver.editPicture(_lcd, _square, 40, 40, 180, 240);
            _SpiDisplayDriver.editPicture(_lcd, _point, 1, 1, 10, 10);
            _SpiDisplayDriver.editPicture(_lcd, _point, 1, 1, 11, 11);
            _SpiDisplayDriver.editPicture(_lcd, _point, 1, 1, 250, 251);

            _SpiDisplayDriver.DrawPicture(_lcd);
            //_SpiDisplayDriver.DrawPicture(_justine01);
            _SpiDisplayDriver.DrawPicture(_maker02);
        }

        private void travauxTimer()
        {

            // Configuration du Timer
            DispatcherTimer dispatcherTimerClock = new DispatcherTimer();
            dispatcherTimerClock.Tick += dispatcherTimerClock_Tick;
            dispatcherTimerClock.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimerClock.Start();

            DispatcherTimer dispatcherTimerLCD = new DispatcherTimer();
            dispatcherTimerLCD.Tick += dispatcherTimerLCD_Tick;
            dispatcherTimerLCD.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimerLCD.Start();

        }

        private void dispatcherTimerClock_Tick(object sender, object e)
        {
            
            // Affichage de la date
            DATE.Text = _UtilReveilDriver.date();

            // Connexion RJ45

            // Connexion Wifi

            // Connexion Bluetooth

            // Affichage de l'heure
            _H_AFF = _UtilReveilDriver.heure();
            _M_AFF = _UtilReveilDriver.minute();
            _S_AFF = _UtilReveilDriver.seconde();
            _L_AFF = _UtilReveilDriver.milliSeconde();

            H_AFF.Text = _H_AFF;
            M_AFF.Text = _M_AFF;
            S_AFF.Text = _S_AFF;
 
            // Affichage de de l'image
            /*
            if (_I_KITTY.Equals("kitty_horloge"))
            {

                I_KITTY.Source = new BitmapImage(new Uri("ms-appx:///Assets/kitty_horloge.png"));
            }
            else if (_I_KITTY.Equals("kitty_reveil"))
            {

                I_KITTY.Source = new BitmapImage(new Uri("ms-appx:///Assets/kitty_reveil.png"));
            }
            else if (_I_KITTY.Equals("kitty_sonnerie"))
            {

                I_KITTY.Source = new BitmapImage(new Uri("ms-appx:///Assets/kitty_sonnerie.gif"));
            }
            */
            // Affichage du mode
            if (_MODE.Equals("HORLOGE"))
            {

                MOD_T.Text = "Horloge";
            }
            else if (_MODE.Equals("REVEIL_SET"))
            {

                MOD_T.Text = "Réveil (réglage)";
            }
            else if (_MODE.Equals("REVEIL_ON"))
            {

                MOD_T.Text = "Réveil activé !";
            }
            else if (_MODE.Equals("REVEIL_TYPE"))
            {

                MOD_T.Text = "Réveil (type)";
            }
            else if (_MODE.Equals("MP3"))
            {

                MOD_T.Text = "MP3 (choix)";
            }
            else if (_MODE.Equals("RADIO"))
            {

                MOD_T.Text = "Radio (choix)";
            }
            else if (_MODE.Equals("SONNERIE"))
            {

                MOD_T.Text = "Sonnerie (choix)";
            }

            // Affichage du réveil
            H_REG.Text = _H_REG;
            M_REG.Text = _M_REG;

            H_SON.Text = _H_SON;
            M_SON.Text = _M_SON;

            // Affichage du type de réveil
            if (_TYPE.Equals("SONNERIE"))
            {

                TYP_C.Text = "Sonnerie";
            }
            else if (_TYPE.Equals("RADIO"))
            {

                TYP_C.Text = "Radio";
            }
            else if (_TYPE.Equals("MP3"))
            {

                TYP_C.Text = "MP3";
            }

            // Affichage des boutons
            Btn_Gris.Text = BTN_GRIS;
            Btn_Blanc.Text = BTN_BLANC;
            Btn_Vert.Text = BTN_VERT;
            Btn_Bleu.Text = BTN_BLEU;
            Btn_Jaune.Text = BTN_JAUNE;

            // Contrôle de l'heure du réveil
            if (_MODE.Equals("REVEIL_ON"))
            {

                if( _H_SON.Equals(_H_AFF) )
                {

                    if (_M_SON.Equals(_M_AFF))
                    {

                        if( _TYPE.Equals("SONNERIE") )
                        {

                            mediaPlayerReveil.Source = mediaSourceSonnerie;

                        }
                        else if ( _TYPE.Equals("RADIO") )
                        {

                            mediaPlayerReveil.Source = mediaSourceRadio;

                        }
                        else if (_TYPE.Equals("MP3"))
                        {

                            mediaPlayerReveil.Source = mediaSourceMP3;

                        }

                        mediaPlayerReveil.Play();
                        
                        reveil = "1";

                    }

                }

            }

        }

        private void dispatcherTimerLCD_Tick(object sender, object e)
        {

            // Affichage de l'image

        }

        // Bouton Gris
        private void _pin27_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if ( args.Edge == GpioPinEdge.RisingEdge )
            {

                if (_MODE.Equals("HORLOGE"))
                {

                    // Passage au mode "REVEIL_SET"
                    _MODE = "REVEIL_SET";

                    // Les heures et minutes de sonnerie reprennent les valeurs du réglage
                    _H_SON = _H_REG;
                    _M_SON = _M_REG;

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "H+";
                    BTN_VERT = "H-";
                    BTN_BLEU = "M+";
                    BTN_JAUNE = "M-";

                }
                else if (_MODE.Equals("REVEIL_SET"))
                {

                    // Passage au mode "REVEIL_ON"
                    _MODE = "REVEIL_ON";

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "";
                    BTN_VERT = "";
                    BTN_BLEU = "";
                    BTN_JAUNE = "chut!";

                }
                else if (_MODE.Equals("REVEIL_ON"))
                {

                    // Passage au mode "REVEIL_TYPE"
                    _MODE = "REVEIL_TYPE";

                    // Les heures et minutes de sonnerie reprennent les valeurs du réglage
                    _H_SON = _H_REG;
                    _M_SON = _M_REG;

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "T+";
                    BTN_VERT = "T-";
                    BTN_BLEU = "";
                    BTN_JAUNE = "";

                }
                else if (_MODE.Equals("REVEIL_TYPE"))
                {

                    // Passage au mode "MP3"
                    _MODE = "MP3";

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "MP3+";
                    BTN_VERT = "MP3-";
                    BTN_BLEU = "Pause";
                    BTN_JAUNE = "";

                }
                else if (_MODE.Equals("MP3"))
                {

                    // Passage au mode "RADIO"
                    _MODE = "RADIO";

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "R+";
                    BTN_VERT = "R-";
                    BTN_BLEU = "Pause";
                    BTN_JAUNE = "";

                }
                else if (_MODE.Equals("RADIO"))
                {

                    // Passage au mode "SONNERIE"
                    _MODE = "SONNERIE";

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "S+";
                    BTN_VERT = "S-";
                    BTN_BLEU = "Pause";
                    BTN_JAUNE = "";

                }
                else if (_MODE.Equals("SONNERIE"))
                {

                    // Passage au mode "HORLOGE"
                    _MODE = "HORLOGE";

                    // Affectation des fonctions aux boutons
                    BTN_GRIS = "Mode";
                    BTN_BLANC = "";
                    BTN_VERT = "";
                    BTN_BLEU = "";
                    BTN_JAUNE = "";

                }

            }

        }

        // Bouton Blanc
        private void _pin05_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                if (_MODE.Equals("HORLOGE"))
                {


                }
                else if (_MODE.Equals("REVEIL_SET"))
                {

                    // Ajout d'une heure à l'horaire de réveil
                    _H_REG = _UtilReveilDriver.addHour(_H_REG);
                    _H_SON = _H_REG;

                }
                else if (_MODE.Equals("REVEIL_ON"))
                {


                }
                else if (_MODE.Equals("REVEIL_TYPE"))
                {

                    if (_TYPE.Equals("SONNERIE"))
                    {

                        _TYPE = "RADIO";
                    }
                    else if (_TYPE.Equals("RADIO"))
                    {

                        _TYPE = "MP3";
                    }
                    else if (_TYPE.Equals("MP3"))
                    {

                        _TYPE = "SONNERIE";
                    }

                }
                else if (_MODE.Equals("MP3"))
                {


                }
                else if (_MODE.Equals("RADIO"))
                {


                }
                else if (_MODE.Equals("SONNERIE"))
                {


                }

            }

        }

        // Bouton Vert
        private void _pin13_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                if (_MODE.Equals("HORLOGE"))
                {


                }
                else if (_MODE.Equals("REVEIL_SET"))
                {

                    // Retrait d'une heure à l'horaire de réveil
                    _H_REG = _UtilReveilDriver.removeHour(_H_REG);
                    _H_SON = _H_REG;

                }
                else if (_MODE.Equals("REVEIL_ON"))
                {


                }
                else if (_MODE.Equals("REVEIL_TYPE"))
                {

                    if (_TYPE.Equals("SONNERIE"))
                    {

                        _TYPE = "MP3";
                    }
                    else if (_TYPE.Equals("MP3"))
                    {

                        _TYPE = "RADIO";
                    }
                    else if (_TYPE.Equals("RADIO"))
                    {

                        _TYPE = "SONNERIE";
                    }

                }
                else if (_MODE.Equals("MP3"))
                {


                }
                else if (_MODE.Equals("RADIO"))
                {


                }
                else if (_MODE.Equals("SONNERIE"))
                {


                }

            }

        }

        // Bouton Bleu
        private void _pin19_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                if (_MODE.Equals("HORLOGE"))
                {


                }
                else if (_MODE.Equals("REVEIL_SET"))
                {

                    // Ajout d'une minute à l'horaire de réveil
                    _M_REG = _UtilReveilDriver.addMinute(_M_REG);
                    _M_SON = _M_REG;

                }
                else if (_MODE.Equals("REVEIL_ON"))
                {


                }
                else if (_MODE.Equals("REVEIL_TYPE"))
                {


                }
                else if (_MODE.Equals("MP3"))
                {


                }
                else if (_MODE.Equals("RADIO"))
                {


                }
                else if (_MODE.Equals("SONNERIE"))
                {


                }

            }

        }

        // Bouton Jaune
        private void _pin26_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                if (_MODE.Equals("HORLOGE"))
                {


                }
                else if (_MODE.Equals("REVEIL_SET"))
                {

                    // Retrait d'une minute à l'horaire de réveil
                    _M_REG = _UtilReveilDriver.removeMinute(_M_REG);
                    _M_SON = _M_REG;

                }
                else if (_MODE.Equals("REVEIL_ON"))
                {

                    if (reveil.Equals("1"))
                    {

                        mediaPlayerReveil.Pause();

                        _M_SON = _UtilReveilDriver.addMinute( _M_AFF );
                        _M_SON = _UtilReveilDriver.addMinute( _M_SON );

                        reveil = "0";

                    }

                }
                else if (_MODE.Equals("REVEIL_TYPE"))
                {


                }
                else if (_MODE.Equals("MP3"))
                {


                }
                else if (_MODE.Equals("RADIO"))
                {


                }
                else if (_MODE.Equals("SONNERIE"))
                {


                }

            }

        }

    }

}
