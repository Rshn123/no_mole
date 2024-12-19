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
using System.Windows.Threading;
using System.Diagnostics;

namespace No_Mole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private FilterInfoCollection? _videoDevices;
        private VideoCaptureDevice? _videoSource;
        private bool recordingVisibility = false;

        private DispatcherTimer _timer;        // Timer for updating the clock
        private TimeSpan _elapsedTime;        // Tracks elapsed time
        private TimeSpan _durationLimit;      // Duration limit (15 seconds)
        private float _brightnessFactor = 1.0f; // Default value (no brightness change)
        private float _zoomFactor = 1.0f;
        private bool zoomSliderVisibility = false;
        private bool brightnessSliderVisibility = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Recording_Info.Visibility = Visibility.Hidden;
            ZoomSliderUI.Visibility = Visibility.Hidden;
            BrightnessSliderUI.Visibility = Visibility.Hidden;    
            // Initialize the timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);  // Update every second
            _timer.Tick += Timer_Tick!;
            Record_Button.IsEnabled = true;
            CameraViewBorder.Width = 400;
            // Set the duration limit to 15 seconds
            _durationLimit = TimeSpan.FromSeconds(15);

            // Initialize elapsed time
            _elapsedTime = TimeSpan.Zero;

            StartLive();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1)); // Increment elapsed time
            TimerText.Text = "recording...";  // Update the display
                                              // Toggle the visibility of the TextBlock
            if (TimerText.Visibility == Visibility.Visible)
            {
                TimerText.Visibility = Visibility.Hidden;
            }
            else
            {
                TimerText.Visibility = Visibility.Visible;
            }
            // Stop the timer if the elapsed time reaches the limit
            if (_elapsedTime >= _durationLimit)
            {
                TimerText.Visibility = Visibility.Visible;
                StopTimer();
                MessageBox.Show("Recording finished!");  // Optional: Show a message box or update UI
            }
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
            _videoSource = new VideoCaptureDevice(_videoDevices[0].MonikerString);
            _videoSource.NewFrame += VideoSource_NewFrame;

            // Start video capture
            _videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap? bitmap = null;

            try
            {
                bitmap = (Bitmap)eventArgs.Frame.Clone();

                // Apply zoom
                if (_zoomFactor > 1.0f)
                {
                    try
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
                    catch (ArgumentException ex)
                    {
                        // Handle cases where the crop rectangle is invalid
                        Debug.WriteLine($"Zoom error: {ex.Message}");
                    }
                }

                // Apply brightness
                if (_brightnessFactor != 1.0f)
                {
                    try
                    {
                        AdjustBrightness(bitmap, _brightnessFactor);
                    }
                    catch (Exception ex)
                    {
                        // Handle brightness adjustment errors
                        Debug.WriteLine($"Brightness adjustment error: {ex.Message}");
                    }
                }

                // Display the frame
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        IntPtr hBitmap = bitmap.GetHbitmap();
                        CameraImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

                        // Ensure proper cleanup of unmanaged resources
                        DeleteObject(hBitmap);
                    }
                    catch (Exception ex)
                    {
                        // Handle display errors
                        Debug.WriteLine($"Display error: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors in the overall frame processing
                Debug.WriteLine($"NewFrame error: {ex.Message}");
            }
            finally
            {
                // Ensure bitmap resources are released
                bitmap?.Dispose();
            }
        }


        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        private static void AdjustBrightness(Bitmap bitmap, float brightnessFactor)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData? data = null;

            try
            {
                data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

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
            catch (Exception ex)
            {
                // Handle brightness adjustment errors
                Debug.WriteLine($"AdjustBrightness error: {ex.Message}");
            }
            finally
            {
                if (data != null)
                {
                    bitmap.UnlockBits(data);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _videoSource!.SignalToStop();
            _videoSource.WaitForStop();
            _videoSource = null;
        }

        private void To_400(object sender, RoutedEventArgs e)
        {
            CameraViewBorder.Width = 400;
        }

        private void To_500(object sender, RoutedEventArgs e)
        {
            CameraViewBorder.Width = 500;
        }

        private void To_600(object sender, RoutedEventArgs e)
        {
            CameraViewBorder.Width = 600;
        }
        private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _brightnessFactor = (float)e.NewValue;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _zoomFactor = (float)e.NewValue;
        }
      
        private void Change_Resolution(int resolution)
        {
            CameraViewBorder.Width = resolution;
        }
        private void CaptureButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void ZoomButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void BrightnessButtonClicked(object sender, RoutedEventArgs e)
        {

        }
        private void RecordButtonClicked(object sender, RoutedEventArgs e)
        {
            
            if (recordingVisibility)
            {
                ChangeRecordingVisibility(false); 
                Record_Button.ImageSource = "../Resources/Icons/recorder.png";
                recordingVisibility = false;
                Record_Button!.CustomButtonBackground = "#ffffff";
                StopTimer();
            }
            else
            {
                ChangeRecordingVisibility(true);
                recordingVisibility = true;
                Record_Button.ImageSource = "../Resources/Icons/stop.png";
                Record_Button!.CustomButtonBackground = "#AD42AD";
                StartTimer();
            }

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

               ModalWindow modal = new(this, button!)
                {
                    Owner = this,
                    Width = 615,
                    Height = 430
                };

                modal.Left = this.Left + (this.Width - modal.Width) / 2;
                modal.Top = this.Top + (this.Height - modal.Height) / 2;

                modal.ShowDialog();

                this.Effect = null;

            }
            else
            {
                button!.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D43E3E"));

                ChangeButtonImage("Resources/Icons/stop.png");

                ChangeText("Stop Inspection");
            }

        }
        public void ChangeButtonImage(string imageSource)

        {
            ButtonImage.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));
        }

        public void ChangeRecordingVisibility(bool visible)
        {
            Recording_Info.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
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
            OpenModal(new ContactUs(), 550, 800);
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public void StopTimer()
        {
            _elapsedTime = TimeSpan.Zero;
            _timer.Stop();
        }

        private void Video_Click(object sender, RoutedEventArgs e)
        {
            OpenModal(new VideoEffect(), 300, 450);
        }
    }
}