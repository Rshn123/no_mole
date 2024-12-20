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
        public ICommand SendTestNotification { get; private set; }
        private readonly ToastProvider _toastProvider;
        private readonly AppTheme _appTheme;

        // Constructor with NotificationContainer
        public ViewModal(NotificationContainer notificationContainer)
        {
            if (notificationContainer == null)
                throw new ArgumentNullException(nameof(notificationContainer));

            _toastProvider = new ToastProvider(notificationContainer);
            _appTheme = new AppTheme();
            SendTestNotification = new RelayCommand(_ => OnSend());
        }

        // Parameterless constructor (optional)
        public ViewModal()
        {
            var notificationContainer = new NotificationContainer();
            _toastProvider = new ToastProvider(notificationContainer);
            _appTheme = new AppTheme();
            SendTestNotification = new RelayCommand(_ => OnSend());
        }

        // Method to send notifications
        public void OnSend()
        {
            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.INFO,
                "Message Inspection",
                "Message inspection has started.",
                5,
                animationConfig: new AnimationConfig { }
            );

            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.INFO,
                "Recording Started",
                "Recording has started successfully.",
                5,
                animationConfig: new AnimationConfig { }
            );

            _toastProvider.NotificationService.AddNotification(
                Flattinger.Core.Enums.ToastType.SUCCESS,
                "Recording Saved",
                "Your recording has been saved.",
                5,
                animationConfig: new AnimationConfig { }
            );

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
