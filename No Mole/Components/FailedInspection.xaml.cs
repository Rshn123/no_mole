using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for FailedInspection.xaml
    /// </summary>
    public partial class FailedInspection
    {
        private bool _failOrPass = false;
        private bool _errorMessageFlag = true;
        private readonly Button? _mainButton;
        private readonly MainWindow? _mainWindow;
        private readonly ModalWindow? _modalWindow;
        public FailedInspection(Button button, MainWindow mainWindow, ModalWindow modalWindow)
        {
            InitializeComponent();
            _mainButton = button;
            _mainWindow = mainWindow;
            _modalWindow = modalWindow;
            Error_Message.Visibility = Visibility.Hidden;
        }

        private void SetButtonStates(Button activeButton, Button inactiveButton, bool failOrPassValue)
        {
            _errorMessageFlag = false;
            Error_Message.Visibility = Visibility.Hidden;
            activeButton.Background = Brushes.Purple;
            inactiveButton.Background = new BrushConverter().ConvertFromString("#292929") as Brush;
            _failOrPass = failOrPassValue;
        }

        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonStates(PassButton, FailButton, false);
        }

        private void FailButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonStates(FailButton, PassButton, true);
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_errorMessageFlag)
            {
                Error_Message.Visibility = Visibility.Visible;
                return;
            }

            if (_failOrPass)
            {
                _modalWindow!.MainFrame.Navigate( new UdiOrNonUdiModal(_modalWindow));
            }
            else
            {
                _mainButton!.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AD42AD"));
                _mainWindow!.ChangeText("Start Inspection");
                _mainWindow.ChangeButtonImage("Resources/Icons/play.png");
                _mainWindow.ChangeRecordingVisibility(false);
                _mainWindow.StopTimer();
                _modalWindow!.Close();
            }


        }
    }
}
