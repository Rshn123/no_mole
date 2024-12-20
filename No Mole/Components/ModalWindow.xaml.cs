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
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        MainWindow _mainWindow;
        Button _button;

        public ModalWindow(MainWindow mainWindow, Button button)
        {
            _mainWindow = mainWindow;
            _button = button;
            InitializeComponent();
            MainFrame.Navigate(new FailedInspection(_button, _mainWindow, this));
        }

        public void Close_Popup_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ChangeRVIButtonState(string color, string image, string text)
        {
            _mainWindow.ChangeRVIButtonState(color, image, text);
        }
    }
}
