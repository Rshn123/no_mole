using System.Windows;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Rectangle = System.Drawing.Rectangle;
using Image = System.Drawing.Image;
using No_Mole.Components;
using System.Windows.Media.Effects;
using System.Windows.Controls;
using System.Windows.Media;
using ColorConverter = System.Windows.Media.ColorConverter;
using System.ComponentModel;
using Color = System.Windows.Media.Color;

namespace No_Mole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int zoomLevel = 100; // Default zoom percentage (100% = no zoom)
        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoSource;

        private float _brightnessFactor = 1.0f; // Default value (no brightness change)
        private float _zoomFactor = 1.0f;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            StartLive();
        }

  
        private void StartLive()
        {
            // Enumerate cameras
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (_videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            // Select the first camera
            _videoSource = new VideoCaptureDevice(_videoDevices[1].MonikerString);
            _videoSource.NewFrame += VideoSource_NewFrame;

            // Start video capture
            _videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();

            // Apply zoom
            if (_zoomFactor > 1.0f)
            {
                int newWidth = (int)(bitmap.Width / _zoomFactor);
                int newHeight = (int)(bitmap.Height / _zoomFactor);
                var cropRect = new Rectangle(
                    (bitmap.Width - newWidth) / 2,
                    (bitmap.Height - newHeight) / 2,
                    newWidth,
                    newHeight);

                bitmap = bitmap.Clone(cropRect, bitmap.PixelFormat);
            }

            // Apply brightness
            if (_brightnessFactor != 1.0f)
            {
                AdjustBrightness(bitmap, _brightnessFactor);
            }

            // Display the frame
            Dispatcher.Invoke(() =>
            {
                CameraImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            });

            bitmap.Dispose();
        }

        private void AdjustBrightness(Bitmap bitmap, float brightnessFactor)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            try
            {
                // Get the total number of bytes in the image
                int bytesPerPixel = Image.GetPixelFormatSize(data.PixelFormat) / 8;
                int totalBytes = data.Stride * data.Height;

                // Copy the pixel data into a managed array
                byte[] pixelBuffer = new byte[totalBytes];
                Marshal.Copy(data.Scan0, pixelBuffer, 0, totalBytes);

                // Adjust brightness
                for (int i = 0; i < pixelBuffer.Length; i += bytesPerPixel)
                {
                    pixelBuffer[i] = (byte)Math.Clamp(pixelBuffer[i] * brightnessFactor, 0, 255);       // Blue
                    pixelBuffer[i + 1] = (byte)Math.Clamp(pixelBuffer[i + 1] * brightnessFactor, 0, 255); // Green
                    pixelBuffer[i + 2] = (byte)Math.Clamp(pixelBuffer[i + 2] * brightnessFactor, 0, 255); // Red
                }

                // Copy the modified pixel data back into the image
                Marshal.Copy(pixelBuffer, 0, data.Scan0, totalBytes);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _videoSource.SignalToStop();
            _videoSource.WaitForStop();
            _videoSource = null;
        }

        private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _brightnessFactor = (float)e.NewValue;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _zoomFactor = (float)e.NewValue;
        }
        private void CustomButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CustomButton_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void CustomButton_Loaded_2(object sender, RoutedEventArgs e)
        {

        }

        private void CustomButton_Loaded_3(object sender, RoutedEventArgs e)
        {

        }

        private void CustomButton_Loaded_4(object sender, RoutedEventArgs e)
        {

        }

        private void CustomButton_Loaded_5(object sender, RoutedEventArgs e)
        {

        }
            
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            if (ButtonText.Text.ToString() == "Stop Inspection")
            {
                BlurEffect blur = new()
                {
                    Radius = 10
                };
                this.Effect = blur;

                FailedInspection modal = new(button!, this)
                {
                    Owner = this,
                    Width = 615,
                    Height = 415
                };

                modal.Left = this.Left + (this.Width - modal.Width) / 2;
                modal.Top = this.Top + (this.Height - modal.Height) / 2;

                modal.ShowDialog();

                this.Effect = null;

            }
            else
            {
                BlurEffect blur = new()
                {
                    Radius = 10
                };
                this.Effect = blur;

                InspectionDetailModal modal = new(button!, this)
                {
                    Owner = this,
                    Width = 615,
                    Height = 415
                };

                modal.Left = this.Left + (this.Width - modal.Width) / 2;
                modal.Top = this.Top + (this.Height - modal.Height) / 2;
                modal.ShowDialog();

                this.Effect = null;
            }
          
        }
    
        public void ChangeButtonImage(string imageSource)

        {
            ButtonImage.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));
        }

        public void ChangeText(string text)

        {
            ButtonText.Text = text;
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

        private void Click_About(object sender, RoutedEventArgs e)
        {
            OpenModal(new About(), 450, 800);
        }

        private void Click_Help(object sender, RoutedEventArgs e)
        {
            OpenModal(new ContactUs(), 450, 800);
        }

       
    }
}