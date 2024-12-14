using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for ContactUs.xaml
    /// </summary>
    public partial class ContactUs : Window
    {
        public ContactUs()
        {
            InitializeComponent();
        }

       
        private void Contact_Us_Close_BTN(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                
                if (textBox.Text == "Fullname" || textBox.Text == "youremail@gmail.com" || textBox.Text == "+012231321313" || textBox.Text == "Type here..")
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = new SolidColorBrush(Colors.White);
                }
            }
        }

       
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
            
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    switch (textBox.Name)
                    {
                        case "FullNameTextBox":
                            textBox.Text = "Fullname";
                            break;
                        case "EmailTextBox":
                            textBox.Text = "youremail@gmail.com";
                            break;
                        case "MobileNumberTextBox":
                            textBox.Text = "+012231321313";
                            break;
                        case "QueryTextBox":
                            textBox.Text = "Type here..";
                            break;
                    }
                    textBox.Foreground = new SolidColorBrush(Colors.Gray); 
                }
            }
        }
    }
}
