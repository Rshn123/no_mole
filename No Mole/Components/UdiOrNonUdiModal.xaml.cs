using System;
using System.Windows;
using System.Windows.Media.Effects;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for UdiOrNonUdiModal.xaml
    /// </summary>
    public partial class UdiOrNonUdiModal
    {
        ModalWindow _modalWindow;
        public UdiOrNonUdiModal(ModalWindow modalWindow)
        {
            InitializeComponent();
            _modalWindow = modalWindow;
        }

        private void UDI_Button_Clicked(object sender, RoutedEventArgs e)
        {
            // Create and show the UDI modal
            _modalWindow.MainFrame.Navigate(new UDIEnterModal(_modalWindow));
        }

        private void Non_UDI_Button_Clicked(object sender, RoutedEventArgs e)
        {
            // Create and show the Non-UDI modal
            _modalWindow.MainFrame.Navigate(new InspectionDetailModal(_modalWindow));
        }


    }
}
