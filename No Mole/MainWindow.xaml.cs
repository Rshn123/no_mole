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
using Color = System.Windows.Media.Color;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using System.Formats.Asn1;

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

        private bool _isRecording = false;
        private Process? _ffmpegProcess;
        private string _outputFileName = $"video_{DateTime.Now:yyyyMMdd_HHmmss}.mp4";
        private bool _isCaptureRequested = false;
        private string _captureImagePath = string.Empty;
        private DispatcherTimer _timer;        // Timer for updating the clock
        private TimeSpan _elapsedTime;        // Tracks elapsed time
        private TimeSpan _durationLimit;      // Duration limit (15 seconds)
        private float _brightnessFactor = 1.0f; // Default value (no brightness change)
        private float _zoomFactor = 1.0f;
        private bool zoomSliderVisibility = false;
        private bool brightnessSliderVisibility = false;
        Bitmap? bitmap = null;
        private string outputVideoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "video"); // Path for video folder

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

                // Check if an image capture was requested
                if (_isCaptureRequested)
                {
                    CaptureImage(bitmap);
                    _isCaptureRequested = false;
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

                if (_isRecording && _ffmpegProcess != null)
                {
                    using var ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Bmp);
                    var bmpData = ms.ToArray();

                    if (_ffmpegProcess.StandardInput.BaseStream.CanWrite)
                    {
                        _ffmpegProcess.StandardInput.BaseStream.Write(bmpData, 0, bmpData.Length);
                        _ffmpegProcess.StandardInput.BaseStream.Flush();
                    }
                }

                // Display the frame
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        IntPtr hBitmap = bitmap.GetHbitmap();
                        var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

                        // Ensure the image is centered and scaled to maintain aspect ratio
                        CameraImage.Source = bitmapSource;

                        // Optionally adjust the layout properties (to center the image within the container)
                        CameraImage.HorizontalAlignment = HorizontalAlignment.Center;
                        CameraImage.VerticalAlignment = VerticalAlignment.Center;

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

        private void StartRecording()
        {
            _isRecording = true;
            _ffmpegProcess = new Process
            {
                StartInfo =
            {
                FileName = "ffmpeg",
                Arguments = $"-y -f rawvideo -pix_fmt bgr24 -s {640}x{480} -r 30 -i pipe:0 -c:v libx264 {_outputFileName}",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
            };
            _ffmpegProcess.Start();
        }

        private void StopRecording()
        {
            if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
            {
                _ffmpegProcess.StandardInput.Close();
                _ffmpegProcess.WaitForExit();
                _ffmpegProcess.Dispose();
            }
            _isRecording = false;
            MessageBox.Show($"Video saved as {_outputFileName}");
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

        private void CaptureButtonClicked(object sender, RoutedEventArgs e)
        {
            _isCaptureRequested = true;
           
        }

        private void CaptureImage(Bitmap bitmap)
        {
            try
            {
                // Get the project directory
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Create a folder named "CapturedFiles" inside the project directory
                string captureFolder = Path.Combine(projectDirectory, "CapturedFiles");
                Directory.CreateDirectory(captureFolder); // Ensure the folder exists
                string imagePath = Path.Combine(captureFolder,
                                                $"capture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                bitmap.Save(imagePath, ImageFormat.Jpeg);
                Dispatcher.Invoke(() => MessageBox.Show($"Image saved to {imagePath}"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Image capture error: {ex.Message}");
            }
        }


        private void ToggleUIElement(ref bool visibilityFlag, UIElement uiElement, CustomButton button)
        {
            visibilityFlag = !visibilityFlag;
            uiElement.Visibility = visibilityFlag ? Visibility.Visible : Visibility.Hidden;
            button.CustomButtonBackground = visibilityFlag ? "#AD42AD" : "#ffffff";
        }

        private void ZoomButtonClicked(object sender, RoutedEventArgs e)
        {
            ToggleUIElement(ref zoomSliderVisibility, ZoomSliderUI, ZoomSliderButton);
        }

        private void BrightnessButtonClicked(object sender, RoutedEventArgs e)
        {
            ToggleUIElement(ref brightnessSliderVisibility, BrightnessSliderUI, BrightnessButton);
        }

        private void RecordButtonClicked(object sender, RoutedEventArgs e)
        {
            recordingVisibility = !recordingVisibility;
            ChangeRecordingVisibility(recordingVisibility);
            Record_Button.ImageSource = recordingVisibility
                ? "../Resources/Icons/stop.png"
                : "../Resources/Icons/recorder.png";
            Record_Button.CustomButtonBackground = recordingVisibility ? "#AD42AD" : "#ffffff";

            if (recordingVisibility)
            {
                StartTimer();
                StartRecording();
            }
            else
            {
                StopTimer();
                StopRecording();
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