using System;
using System.Windows.Input;
using Flattinger.Core.Interface.ToastMessage;
using Flattinger.Core.MVVM;
using Flattinger.Core.Theme;
using Flattinger.UI.ToastMessage;
using Flattinger.UI.ToastMessage.Controls;

namespace No_Mole.Notification
{
    public class ViewModal : BaseViewModel
    {
        public ICommand SendMessageInspectionNotification { get; private set; }
        public ICommand SendRecordingStartedNotification { get; private set; }
        public ICommand SendRecordingSavedNotification { get; private set; }
        public ICommand SendImageCaptureNotification { get; private set; }

        private readonly ToastProvider _toastProvider;
        private readonly AppTheme _appTheme;

        // Constructor with NotificationContainer
        public ViewModal(NotificationContainer notificationContainer)
        {
            if (notificationContainer == null)
                throw new ArgumentNullException(nameof(notificationContainer));

            _toastProvider = new ToastProvider(notificationContainer);
            _appTheme = new AppTheme();

            // Assign commands to corresponding methods
            SendMessageInspectionNotification = new RelayCommand(_ => SendMessageInspection());
            SendRecordingStartedNotification = new RelayCommand(_ => SendRecordingStarted());
            SendRecordingSavedNotification = new RelayCommand(_ => SendRecordingSaved());
            SendImageCaptureNotification = new RelayCommand(_ => SendImageCapture());
        }

        // Parameterless constructor (optional)
        public ViewModal() : this(new NotificationContainer())
        {
        }

        // Methods for each button
        private void SendMessageInspection()
        {
            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.INFO,
                "Message Inspection",
                "Message inspection has started.",
                5,
                animationConfig: new AnimationConfig { }
            );
        }

        private void SendRecordingStarted()
        {
            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.INFO,
                "Recording Started",
                "Recording has started successfully.",
                5,
                animationConfig: new AnimationConfig { }
            );
        }

        private void SendRecordingSaved()
        {
            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.SUCCESS,
                "Recording Saved",
                "Your recording has been saved.",
                5,
                animationConfig: new AnimationConfig { }
            );
        }

        private void SendImageCapture()
        {
            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.INFO,
                "Image Capture",
                "An image has been captured.",
                5,
                animationConfig: new AnimationConfig { }
            );
        }
    }
}
