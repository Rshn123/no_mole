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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Accord;
using Flattinger.UI.ToastMessage;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for SpecialIfAny.xaml
    /// </summary>
    public partial class SpecialIfAny
    {
        private readonly ModalWindow _modalWindow;
        public SpecialIfAny(ModalWindow modalWindow)
        {
            _modalWindow = modalWindow;
            InitializeComponent();
        }
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            _modalWindow.ChangeRVIButtonState("#AD42AD", "Resources/Icons/play.png", "Start RVI");
            TriggerToast();
            _modalWindow.Close();
        }

        private void TriggerToast()
        {
            // Access MainWindow and its NotificationContainer
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            if (mainWindow != null)
            {
                // Trigger a toast notification
                var toastProvider = new ToastProvider(mainWindow.NotificationContainer);
                toastProvider.NotificationService.AddNotification(
                    Flattinger.Core.Enums.ToastType.INFO,
                    "Saved in cloud.",
                    "Video and Images are saved to cloud.",
                    5
                );
            }
            else
            {
                MessageBox.Show("MainWindow is not accessible.");
            }
        }
    }
}
