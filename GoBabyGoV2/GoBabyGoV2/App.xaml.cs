using GoBabyGoV2.Utilities;
using GoBabyGoV2.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace GoBabyGoV2
{
    /*
     * General Build Info:
     *
     * Change Configuration Mode (Debug/Release): Open Configuration Manager
     * Build > Configuration Manager
     *
     *
     * Android Release:
     * Linker: Don't link
     *
     * Comment out 'HotReloader.Current.Start(this);' if using Xamarin Previewer
     * Android Emulator Adb command for Hot.Reload:
     * adb forward tcp:8000 tcp:8000
     *
     * 
     * 
     * iOS Build Info:
     *
     * When targeting Release for deployment on Android, un-load iOS project if unable to Archive Android Solution (same key used)
     *
     * Use: Link Framework SDKs Only, if compilation errors arise due to linking
     * Recheck: Perform all 32-bit floating point operations as 64-bit, if needed for higher precision
     *
     *         
     * Bundle Signing:
     * Use Manual Provisioning(Info.plist): Xcode app uses Automatic signing (Personal Team)
     *
     */
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

#if DEBUG
            // HotReloader.Current.Start(this);
#endif

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