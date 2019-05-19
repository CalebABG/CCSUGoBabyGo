using GoBabyGoV2.Utilities;
using GoBabyGoV2.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBabyGoV2
{
    public partial class App : Application
    {
        /*
         * General Build Info:
         *
         * Android Emulator Adb command for XAMLator:
         * adb reverse tcp:8488 tcp:8488
         *
         * 
         * iOS Build Info:
         *
         * Use: Link Framework SDKs Only, if compilation errors arise due to linking
         * Recheck: Perform all 32-bit floating point operations as 64-bit, if needed for higher precision
         *
         *         
         * Bundle Signing:
         * Use Manual Provisioning(Info.plist): Xcode app uses Automatic signing (Personal Team)
         *
         */
        public App()
        {
            InitializeComponent();

            // Create MainPage as a NavigationPage
            MainPage = new NavigationPage(new CarWelcomePage());

            // Set Status bar Color (Specifically for iOS)
            MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Resources["StatusBarPrimary"]);
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

            // If not null, try stop monitoring
            AccelerometerSensor.Monitor?.StopMonitoring();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes

            // If not null, start monitoring again
            AccelerometerSensor.Monitor?.StartMonitoring();
        }
    }
}
