using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace No_Mole.Components
{
    public partial class Login : Window
    {
        private bool isPasswordVisible = false; 

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Close_BTN(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EyeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                
                PasswordBoxHidden.Password = PasswordBoxVisible.Text;
                PasswordBoxHidden.Visibility = Visibility.Visible;
                PasswordBoxVisible.Visibility = Visibility.Collapsed;

                EyeIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Components/eyeclose.png"));
            }
            else
            {
                PasswordBoxVisible.Text = PasswordBoxHidden.Password;
                PasswordBoxVisible.Visibility = Visibility.Visible;
                PasswordBoxHidden.Visibility = Visibility.Collapsed;

                EyeIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Components/eyeopen.png"));
            }

            isPasswordVisible = !isPasswordVisible; 
        }
    }
}
