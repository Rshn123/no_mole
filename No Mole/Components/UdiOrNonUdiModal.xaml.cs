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
    /// Interaction logic for UdiOrNonUdiModal.xaml
    /// </summary>
    public partial class UdiOrNonUdiModal : Window
    {
        public UdiOrNonUdiModal(FailedInspection failedInspection)
        {
            InitializeComponent();
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UDI_Button_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Non_UDI_Button_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
