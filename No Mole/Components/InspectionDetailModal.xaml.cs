using System.Windows;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for InspectionDetailModal.xaml
    /// </summary>
    public partial class InspectionDetailModal
    {
        private readonly ModalWindow _modalWindow;
        private bool _errorMessageVisibility = false;
        public InspectionDetailModal(ModalWindow modalWindow)
        {
            InitializeComponent();
            _modalWindow = modalWindow;
            InspectionModalErrorMsg.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SerialNumberTextBox.Text))
            {
                InspectionModalErrorMsg.Visibility = Visibility.Visible;
                _errorMessageVisibility = true;
            }
            else
            {
                InspectionModalErrorMsg.Visibility = Visibility.Hidden;
                _errorMessageVisibility = false;
                _modalWindow.MainFrame.Navigate(new ReasonForFailureModal(_modalWindow));
            }

        }
    }
}
