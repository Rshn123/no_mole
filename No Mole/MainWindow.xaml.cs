﻿using System.Windows;
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
using No_Mole.Notification;

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

        private readonly bool _isRecording = false;
        private bool _isCaptureRequested = false;
        // Timer for updating the clock
        private readonly DispatcherTimer _timer;
        // Tracks elapsed time
        private TimeSpan _elapsedTime;
        // Duration limit (15 seconds)
        private readonly TimeSpan _durationLimit;
        // Default value (no brightness change)
        private float _brightnessFactor = 1.0f;
        private float _zoomFactor = 1.0f;
        private bool zoomSliderVisibility = false;
        private bool brightnessSliderVisibility = false;
        Bitmap? bitmap = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModal(NotificationContainer);
            Recording_Info.Visibility = Visibility.Hidden;
            ZoomSliderUI.Visibility = Visibility.Hidden;
            BrightnessSliderUI.Visibility = Visibility.Hidden;
            this.Closed += Window_Closed!;
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
            CreateCaptureFolders();
            StartLive();
        }

        private static void CreateCaptureFolders()
        {
            // Get the base directory of the application
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Define the base "CapturedFiles" folder path
            string capturedFilesFolder = Path.Combine(projectDirectory, "CapturedFiles");

            // Define paths for "Images" and "Videos" folders
            string imagesFolder = Path.Combine(capturedFilesFolder, "Images");
            string videosFolder = Path.Combine(capturedFilesFolder, "Videos");

            // Create the folders if they don't exist
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            if (!Directory.Exists(videosFolder))
            {
                Directory.CreateDirectory(videosFolder);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1)); // Increment elapsed time

            // Update the display
            TimerText.Text = "recording...";
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

        }

        private void StopRecording()
        {

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
            // Stop the video source
            if (_videoSource != null)
            {
                _videoSource.SignalToStop();
                _videoSource.WaitForStop();
                _videoSource = null;
            }

            // Ensure all windows are closed
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this) // Skip the main window
                {
                    window.Close();
                }
            }

            // Shutdown the application
            Application.Current.Shutdown();
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

        private static void CaptureImage(Bitmap bitmap)
        {
            try
            {
                // Get the project directory
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Create a folder named "CapturedFiles" inside the project directory
                string captureFolder = Path.Combine(projectDirectory, "CapturedFiles/Images");
                Directory.CreateDirectory(captureFolder); // Ensure the folder exists
                string imagePath = Path.Combine(captureFolder,
                                                $"capture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                bitmap.Save(imagePath, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Image capture error: {ex.Message}");
            }
        }

        private static void ToggleUIElement(ref bool visibilityFlag, UIElement uiElement, CustomButton button)
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

            if (ButtonText.Text.ToString() == "Stop RVI")
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
                OpenModal(modal, 430, 615);

            }
            else
            {
                ChangeRVIButtonState("#D43E3E", "Resources/Icons/stop.png", "Stop RVI");
            }

        }

        public void ChangeRVIButtonState(string color, string image, string text)
        {
            RVIButton!.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));

            ChangeButtonImage(image);

            ChangeText(text);
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