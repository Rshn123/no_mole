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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for ReasonForFailureModal.xaml
    /// </summary>
    public partial class ReasonForFailureModal : Window
    {
        public ReasonForFailureModal()
        {
            InitializeComponent();
            Error_Message.Visibility = Visibility.Hidden;
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NotesTextBox.Text.Length > 0)
            {
                ShowModal(new SpecialIfAny(), 615, 415);
            }
            else
            {
                Error_Message.Visibility = Visibility.Visible;
            }
        }

        private void ApplyBlurEffect()
        {
            this.Effect = new BlurEffect { Radius = 10 };
        }

        private void RemoveBlurEffect()
        {
            this.Effect = null;
        }

        private void ShowModal(Window modal, double width, double height)
        {
            // Apply blur effect to the current window
            ApplyBlurEffect();

            // Set modal properties
            modal.Owner = this;
            modal.Width = width;
            modal.Height = height;

            // Center modal over the parent window
            modal.Left = this.Left + (this.Width - modal.Width) / 2;
            modal.Top = this.Top + (this.Height - modal.Height) / 2;

            // Show modal as a dialog
            modal.ShowDialog();

            // Remove blur effect
            RemoveBlurEffect();

            Close();
        }
    }
}
