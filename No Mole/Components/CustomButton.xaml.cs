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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(CustomButton), 
                new PropertyMetadata("../Resources/Icons/settings.png"));
        public string ImageSource
        {
            get
            {
                return (string)GetValue(ImageSourceProperty);
            }
            set {SetValue(ImageSourceProperty, value); }
        }

        public string ToolTipText
        {
            get { return (string)GetValue(ToolTipTextProperty); }
            set { SetValue(ToolTipTextProperty, value); }
        }

        public static readonly DependencyProperty ToolTipTextProperty =
            DependencyProperty.Register(nameof(ToolTipText), typeof(string), typeof(CustomButton), new PropertyMetadata(string.Empty));
    }
}
