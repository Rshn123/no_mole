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
        public InspectionDetailModal()
        {
            InitializeComponent();
        }


        private void MyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HintTextBlock.Visibility = string.IsNullOrEmpty(MyTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HintTextBlock.Visibility = string.IsNullOrEmpty(MyTextBox.Text) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void MyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HintTextBlock.Visibility = string.IsNullOrEmpty(MyTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
