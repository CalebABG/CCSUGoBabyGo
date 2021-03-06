﻿using Foundation;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastIOS))]

namespace GoBabyGoV2.iOS.DependencyServices
{
    public class ToastIOS : IToast
    {
        private const double LongDelay = 3.5;
        private const double ShortDelay = 0.75;

        public void LongAlert(string message)
        {
            ShowAlert(message, LongDelay);
        }

        public void ShortAlert(string message)
        {
            ShowAlert(message, ShortDelay);
        }

        private void ShowAlert(string message, double seconds)
        {
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.ActionSheet);

            var alertDelay = NSTimer.CreateScheduledTimer(seconds, obj => { DismissMessage(alert, obj); });

            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (viewController.PresentedViewController != null)
                viewController = viewController.PresentedViewController;
            viewController.PresentViewController(alert, true, () =>
            {
                var tapGesture = new UITapGestureRecognizer(_ => DismissMessage(alert, null));
                alert.View.Superview?.Subviews[0].AddGestureRecognizer(tapGesture);
            });
        }

        private void DismissMessage(UIAlertController alert, NSTimer alertDelay)
        {
            alert?.DismissViewController(true, null);
            alertDelay?.Dispose();
        }
    }
}