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
    /// Interaction logic for UDIEnterModal.xaml
    /// </summary>
    public partial class UDIEnterModal
    {
        readonly ModalWindow _modalWindow;
        public UDIEnterModal(ModalWindow modalWindow)
        {
            InitializeComponent();
            _modalWindow = modalWindow;
        }

        private void Continue_Button(object sender, RoutedEventArgs e)
        {
            _modalWindow.MainFrame.Navigate(new ReasonForFailureModal(_modalWindow));
        }
    }
}
