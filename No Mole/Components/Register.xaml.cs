using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace No_Mole.Components
{
    public partial class Register : Window
    {
        private bool isPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;

        public Register()
        {
            InitializeComponent();
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                PasswordBoxHidden.Password = PasswordBoxVisible.Text;
                PasswordBoxHidden.Visibility = Visibility.Visible;
                PasswordBoxVisible.Visibility = Visibility.Collapsed;
                EyeIconPassword.Source = new BitmapImage(new Uri("/Components/eyeclos.png", UriKind.Relative));
            }
            else
            {
                PasswordBoxVisible.Text = PasswordBoxHidden.Password;
                PasswordBoxHidden.Visibility = Visibility.Collapsed;
                PasswordBoxVisible.Visibility = Visibility.Visible;
                EyeIconPassword.Source = new BitmapImage(new Uri("/Components/eyeopen.png", UriKind.Relative));
            }

            isPasswordVisible = !isPasswordVisible;
        }

        private void ToggleConfirmPasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (isConfirmPasswordVisible)
            {
                ConfirmPasswordBoxHidden.Password = ConfirmPasswordBoxVisible.Text;
                ConfirmPasswordBoxHidden.Visibility = Visibility.Visible;
                ConfirmPasswordBoxVisible.Visibility = Visibility.Collapsed;
                EyeIconConfirmPassword.Source = new BitmapImage(new Uri("/Components/eyeclos.png", UriKind.Relative));
            }
            else
            {
                ConfirmPasswordBoxVisible.Text = ConfirmPasswordBoxHidden.Password;
                ConfirmPasswordBoxHidden.Visibility = Visibility.Collapsed;
                ConfirmPasswordBoxVisible.Visibility = Visibility.Visible;
                EyeIconConfirmPassword.Source = new BitmapImage(new Uri("/Components/eyeopen.png", UriKind.Relative));
            }

            isConfirmPasswordVisible = !isConfirmPasswordVisible;
        }
        private void NavigateToLogin(object sender, RoutedEventArgs e)
        {
          
            Login loginWindow = new Login(); 
            loginWindow.Show();

            this.Close();
        }

        private void Register_Close_BTN(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
