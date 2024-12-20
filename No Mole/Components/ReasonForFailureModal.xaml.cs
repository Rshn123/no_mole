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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for ReasonForFailureModal.xaml
    /// </summary>
    public partial class ReasonForFailureModal
    {
        private readonly ModalWindow _modalWindow;
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
