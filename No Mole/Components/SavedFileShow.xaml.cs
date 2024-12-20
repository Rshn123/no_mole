
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;


namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for SavedFileShow.xaml
    /// </summary>
    public partial class SavedFileShow : Window
    {
        public SavedFileShow()
        {
            InitializeComponent();
            DisplaySavedFiles();
        }


        private void DisplaySavedFiles()
        {
            try
            {
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string captureFolder = Path.Combine(projectDirectory, "CapturedFiles/Images");

                string[] files = Directory.GetFiles(captureFolder);

                foreach (string file in files)
                {
                    try
                    {
                        // Load the image
                        BitmapImage bitmap = new BitmapImage(new Uri(file, UriKind.Absolute));
                        Image image = new Image
                        {
                            Source = bitmap,
                            Width = 100,
                            Height = 100,
                            Margin = new Thickness(5),
                            Cursor = Cursors.Hand // Indicate that the image is clickable

                        };
                        // Add click event to preview the image
                        image.MouseLeftButtonDown += (s, e) => ShowPreviewWindow(bitmap);

                        // Add the image to the panel
                        ImageWrapPanel.Children.Add(image);
                        // Add the image to the panel
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image {file}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying files: {ex.Message}");
            }
        }

        private void ShowPreviewWindow(BitmapImage image)
        {
            // Create and show a new window with the preview
            PreviewWindow previewWindow = new PreviewWindow(image);
            previewWindow.ShowDialog(); // Show as a modal dialog
        }

        public void Close_Popup_Click(object sender, RoutedEventArgs e)
        {
            OpenModal(new WarningModal(this), 300, 600);
        }

        private void OpenModal(Window modal, int height, int width)
        {
            BlurEffect blur = new()
            {
                Radius = 10
            };

            this.Effect = blur;

            modal.Owner = this;
            modal.Width = width;
            modal.Height = height;

            modal.Left = this.Left + (this.Width - modal.Width) / 2;
            modal.Top = this.Top + (this.Height - modal.Height) / 2;

            modal.ShowDialog();

            this.Effect = null;
        }
    }
}
