using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for ReasonForFailureModal.xaml
    /// </summary>
    public partial class ReasonForFailureModal
    {
        ModalWindow _modalWindow;
        public ReasonForFailureModal(ModalWindow modalWindow)
        {
            InitializeComponent();
            _modalWindow = modalWindow;
            Error_Message.Visibility = Visibility.Hidden;
        }
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NotesTextBox.Text.Length > 0)
            {
                _modalWindow.MainFrame.Navigate(new SpecialIfAny(_modalWindow));
            }
            else
            {
                Error_Message.Visibility = Visibility.Visible;
            }
        }


    }
}
