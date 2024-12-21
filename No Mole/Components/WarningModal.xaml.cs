
using System.IO;
using System.Windows;
using No_Mole.Notification;
using Path = System.IO.Path;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for WarningModal.xaml
    /// </summary>
    public partial class WarningModal : Window
    {
        private readonly SavedFileShow _savedFileShow;
        public WarningModal(SavedFileShow savedFileShow)
        {
            InitializeComponent();
            _savedFileShow = savedFileShow;
        }

        private void YesButton(object sender, RoutedEventArgs e)
        {

            Close();
            _savedFileShow.Close();
            DeleteImages();
        }

        private void NoButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public static void DeleteImages()
        {
            // Get the project directory
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Create a folder named "CapturedFiles/Images" inside the project directory
            string directoryPath = Path.Combine(projectDirectory, "CapturedFiles", "Images");

            // List of common image file extensions
            string[] value = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };
            string[] imageExtensions = value;

            try
            {
                // Check if the directory exists
                if (!Directory.Exists(directoryPath))
                {
                    MessageBox.Show("The directory does not exist.");
                    return;
                }

                // Get all files in the directory
                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    // Check if the file has a valid image extension
                    if (imageExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Release any locks and delete the file
                        ForceDeleteFile(file);
                        Console.WriteLine($"Deleted: {file}");
                    }
                }

                MessageBox.Show("All images deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Force deletes a file by releasing any locks.
        /// </summary>
        /// <param name="filePath">Path to the file to be deleted.</param>
        private static void ForceDeleteFile(string filePath)
        {
            // Ensure the file is unlocked before deletion
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete))
            {
                fileStream.Close(); // Explicitly close the stream to release the lock
            }

            // Delete the file
            File.Delete(filePath);
        }
    }
}
