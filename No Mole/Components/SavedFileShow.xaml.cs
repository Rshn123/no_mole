using System.Diagnostics;
using System.IO;
using System.Windows;


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

                SavedFilesListBox.ItemsSource = files;

            } catch(Exception ex) {
                MessageBox.Show($"Error displaying files: {ex.Message}");
            }
        }
    }
}
