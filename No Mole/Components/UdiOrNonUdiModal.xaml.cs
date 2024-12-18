using System;
using System.Windows;
using System.Windows.Media.Effects;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for UdiOrNonUdiModal.xaml
    /// </summary>
    public partial class UdiOrNonUdiModal : Window
    {
        public UdiOrNonUdiModal(FailedInspection failedInspection)
        {
            InitializeComponent();
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            Close();
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
            // Apply blur effect to the current window
            ApplyBlurEffect();

            // Set modal properties
            modal.Owner = this;
            modal.Width = width;
            modal.Height = height;

            // Center modal over the parent window
            modal.Left = this.Left + (this.Width - modal.Width) / 2;
            modal.Top = this.Top + (this.Height - modal.Height) / 2;

            // Show modal as a dialog
            modal.ShowDialog();

            // Remove blur effect
            RemoveBlurEffect();

            Close();
        }

        private void UDI_Button_Clicked(object sender, RoutedEventArgs e)
        {
            // Create and show the UDI modal
            ShowModal(new UDIEnterModal(this), 615, 415);
        }

        private void Non_UDI_Button_Clicked(object sender, RoutedEventArgs e)
        {
            // Create and show the Non-UDI modal
            ShowModal(new InspectionDetailModal(this), 615, 415);
        }
    }
}
