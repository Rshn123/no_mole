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
    public partial class FailedInspection : Window
    {
        private bool _failOrPass = false;
        private bool _errorMessageFlag = true;
        private readonly Button? _mainButton;
        private readonly MainWindow? _mainWindow;
        public FailedInspection(Button button, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainButton = button;
            _mainWindow = mainWindow;
            Error_Message.Visibility = Visibility.Hidden;
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void ApplyBlurEffect()
        {
            this.Effect = new BlurEffect { Radius = 10 };
        }

        private void RemoveBlurEffect()
        {
            this.Effect = null;
        }

        private void ShowModal(Window modal, double width, double height)
        {
            ApplyBlurEffect();

            modal.Owner = this;
            modal.Width = width;
            modal.Height = height;

            modal.Left = this.Left + (this.Width - modal.Width) / 2;
            modal.Top = this.Top + (this.Height - modal.Height) / 2;

            modal.ShowDialog();

            RemoveBlurEffect();
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
                var modal = new UdiOrNonUdiModal(this);
                ShowModal(modal, 615, 415);
                Close();
            }
            else
            {
                _mainButton!.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AD42AD"));
                _mainWindow!.ChangeText("Start Inspection");
                _mainWindow.ChangeButtonImage("Resources/Icons/play.png");
                _mainWindow.ChangeRecordingVisibility(false);
                _mainWindow.StopTimer();
                Close();
            }


        }
    }
}
