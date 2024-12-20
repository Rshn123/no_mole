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
    /// Interaction logic for WarningModal.xaml
    /// </summary>
    public partial class WarningModal : Window
    {
        SavedFileShow _savedFileShow;
        public WarningModal(SavedFileShow savedFileShow)
        {
            InitializeComponent();
            _savedFileShow = savedFileShow;
        }
        
        private void YesButton(object sender, RoutedEventArgs e)
        {
            Close();
            _savedFileShow.Close();
        }

        private void NoButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
