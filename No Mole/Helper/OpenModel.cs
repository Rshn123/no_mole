using System.Windows.Media.Effects;
using System.Windows;

namespace No_Mole.Helper
{
    internal class OpenModel
    {
        public void OpenModal(Window modal, Window context, int height, int width)
        {
            BlurEffect blur = new()
            {
                Radius = 10
            };

            context.Effect = blur;

            modal.Owner = context;
            modal.Width = width;
            modal.Height = height;

            modal.Left = context.Left + (context.Width - modal.Width) / 2;
            modal.Top = context.Top + (context.Height - modal.Height) / 2;

            modal.ShowDialog();

            context.Effect = null;
        }
    }
}
