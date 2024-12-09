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
        private readonly Button _mainButton;
        private MainWindow _mainWindow;
        public InspectionDetailModal(Button button, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainButton = button;
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
                _mainButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D43E3E"));

                _mainWindow.ChangeButtonImage("Resources/Icons/stop.png");

                _mainWindow.ChangeText("Stop Inspection");

                this.Close();

            }

        }
    }
}
