using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for SpecialIfAny.xaml
    /// </summary>
    public partial class SpecialIfAny
    {
        private readonly Window _modalWindow;
        private readonly MainWindow _mainWindow;
        public SpecialIfAny(ModalWindow modalWindow)
        {
            _modalWindow = modalWindow;
            InitializeComponent();
        }
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            _modalWindow.Close();
        }
    }
}
