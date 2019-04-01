using GoBabyGoV2.Utilities;
using GoBabyGoV2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBabyGoV2
{
    public partial class App : Application
    {
        /*
         * Build Info:
         *
         * When building, for Xamarin Previewer to work properly, comment out the 'HotReloader.Current.Start(this);'
         * line. For some reason the previewer throws a resource in use error.
         *
         * Additionally, if using Visual Studio 2017, in the App.xaml file, comment out the static resource definition for FontFamily
         * in both the 'buttonStyle' and 'labelStyle' Style definitions. Having these uncommented when previewing xaml designs
         * in VS2017 threw errors for the previewer.
         *
         */
        public App()
        {
            InitializeComponent();

            #if DEBUG
            //HotReloader.Current.Start(this);
            #endif

            MainPage = new NavigationPage(new CarWelcomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            AccelMonitor.StopAccelMonitor();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
