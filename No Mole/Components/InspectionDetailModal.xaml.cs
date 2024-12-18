using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace No_Mole.Components
{
    public partial class InspectionDetailModal : Window
    {
        // This dictionary will store the placeholder text for each textbox.
        private static readonly SolidColorBrush PlaceholderColor = new SolidColorBrush(Colors.Gray);
        private static readonly SolidColorBrush ActiveTextColor = new SolidColorBrush(Colors.White);

        private readonly Dictionary<string, string> _placeholders = new Dictionary<string, string>
        {
            { "DeviceNameTextBox", "Enter device name" },
            { "ManufacturerTextBox", "Enter manufacturer name" },
            { "BatchTextBox", "Enter batch ID" },
            { "SerialNumberTextBox", "Enter serial number" }
        };

        public InspectionDetailModal()
        {
            InitializeComponent();
            InitializePlaceholders();
        }

        private void InitializePlaceholders()
        {
            // Initialize placeholders for each textbox.
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
            // Validate Serial Number TextBox
            if (string.IsNullOrWhiteSpace(SerialNumberTextBox.Text) || SerialNumberTextBox.Text == _placeholders["SerialNumberTextBox"])
            {
                MessageBox.Show("Serial Number is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            MessageBox.Show("Details saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
