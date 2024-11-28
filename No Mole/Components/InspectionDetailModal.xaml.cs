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
        public InspectionDetailModal(Button button)
        {
            InitializeComponent();
            _mainButton = button;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainButton!.Content = "Stop Inspection";

            _mainButton.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#D43E3E"));

            this.Close();
        }
    }
}
