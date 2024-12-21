using System.Windows;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for UDIEnterModal.xaml
    /// </summary>
    public partial class UDIEnterModal
    {
        readonly ModalWindow _modalWindow;
        private bool _udiErrorMessageVisibility = false;
        public UDIEnterModal(ModalWindow modalWindow)
        {
            InitializeComponent();
            UDIErrorMsg.Visibility = Visibility.Hidden;
            _modalWindow = modalWindow;
        }

        private void Continue_Button(object sender, RoutedEventArgs e)
        {
            if (!(UDIInputField.Text.Length > 0))
            {
                UDIErrorMsg.Visibility = Visibility.Visible;
                _udiErrorMessageVisibility = true;
            }
            else
            {
                UDIErrorMsg.Visibility = Visibility.Hidden;
                _modalWindow.MainFrame.Navigate(new ReasonForFailureModal(_modalWindow));
                _udiErrorMessageVisibility = false;
            }
        }
    }
}
