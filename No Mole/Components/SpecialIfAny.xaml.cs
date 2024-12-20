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
            _modalWindow.Close();
        }

        private void OpenModal(Window modal, int height, int width)
        {
            BlurEffect blur = new()
            {
                Radius = 10
            };

            this.Effect = blur;

            modal.Owner = _modalWindow;
            modal.Width = width;
            modal.Height = height;

            modal.Left = _modalWindow.Left + (this.Width - modal.Width) / 2;
            modal.Top = _modalWindow.Top + (this.Height - modal.Height) / 2;

            modal.ShowDialog();

            this.Effect = null;
        }
    }
}
