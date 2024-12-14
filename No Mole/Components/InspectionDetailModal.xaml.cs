using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace No_Mole.Components
{
    public partial class InspectionDetailModal : Window
    {
        private readonly Button _mainButton;
        private readonly MainWindow _mainWindow;

        private static readonly SolidColorBrush PlaceholderColor = new SolidColorBrush(Colors.Gray);
        private static readonly SolidColorBrush ActiveTextColor = new SolidColorBrush(Colors.White);

        private readonly Dictionary<string, string> _placeholders = new Dictionary<string, string>
        {
            { "DeviceNameTextBox", "Enter device name" },
            { "ManufacturerTextBox", "Enter manufacturer name" },
            { "BatchTextBox", "Enter batch ID" },
            { "SerialNumberTextBox", "Enter serial number" }
        };

        public InspectionDetailModal(Button button, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainButton = button ?? throw new ArgumentNullException(nameof(button));
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));

            InitializePlaceholders();
        }

        private void InitializePlaceholders()
        {
            foreach (var placeholder in _placeholders)
            {
                var textBox = FindName(placeholder.Key) as TextBox;
                if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder.Value;
                    textBox.Foreground = PlaceholderColor;
                }
            }
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SerialNumberTextBox.Text) || SerialNumberTextBox.Text == _placeholders["SerialNumberTextBox"])
            {
                ValidationErrorText.Visibility = Visibility.Visible;
                return;
            }

            ValidationErrorText.Visibility = Visibility.Collapsed;

            _mainButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D43E3E"));
            _mainWindow.ChangeButtonImage("Resources/Icons/stop.png");
            _mainWindow.ChangeText("Stop Inspection");
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && _placeholders.TryGetValue(textBox.Name, out var placeholder))
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = ActiveTextColor;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && _placeholders.TryGetValue(textBox.Name, out var placeholder))
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.Foreground = PlaceholderColor;
                }
            }
        }
    }
}
