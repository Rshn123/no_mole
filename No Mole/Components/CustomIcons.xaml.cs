using System.Windows;
using System.Windows.Controls;

namespace No_Mole.Components
{
    public partial class CustomIcons : UserControl
    {
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(string), typeof(CustomIcons), new PropertyMetadata(null));

        public string IconSource
        {
            get => (string)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }

        public CustomIcons()
        {
            InitializeComponent();
        }
    }
}