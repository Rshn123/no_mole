using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            InspectionModalErrorMsg.Visibility = Visibility.Hidden;
            _modalWindow = modalWindow;
            InitializeComponent();
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
