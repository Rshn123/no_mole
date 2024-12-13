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
    public partial class InspectionDetailModal : Window
    {
        private FailedInspection _mainWindow;
        public InspectionDetailModal(FailedInspection mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SerialNumberTextBox.Text))
            {
                MessageBox.Show("Serial Number* Field cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                this.Close();
            }

        }
    }
}
