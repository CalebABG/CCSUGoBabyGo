using GoBabyGoV2.Utilities;
using GoBabyGoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static GoBabyGoV2.Utilities.AccelMonitor;
using static System.Math;

namespace GoBabyGoV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarControlPage : ContentPage
    {
        // Set speed delay for monitoring changes. 
        public static SensorSpeed SensorSpeed { get; } = SensorSpeed.Game;

        private CarControlViewModel ControlViewModel { get; }


        #region Constructor

        public CarControlPage()
        {
            InitializeComponent();

            ControlViewModel = new CarControlViewModel(Navigation);

            BindingContext = ControlViewModel;
        }

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;

                AccelMonitor.StartAccelMonitor();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            /*WaitAndExecute(1000, async () =>
            {
                await Task.Delay(1500);
                Xamarin.Forms.DependencyService.Get<IToast>().ShortAlert("Welcome");
            });*/
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                AccelMonitor.StopAccelMonitor();

                // Unsub from reading changed
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // Xamarin.Forms.DependencyService.Get<IToast>().ShortAlert("RemovedAccelC");
        }

        #region Accelerometer

        public void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;

            var accelX = data.Acceleration.X;
            var accelY = data.Acceleration.Y;


            // Process Acceleration X and Y (Xamarin Essentials outputs in G-force units)

            uint calcAccelX = (uint) Round(Map(accelX, AccelMinX, AccelMaxX, 255.0, 0.0));
            uint calcAccelY = (uint) Round(Map(accelY, AccelMinY, AccelMaxY, 255.0, 0.0));

            /*uint calcAccelX = (uint) Floor(Map(Constrain(accelX, AccelMinX, AccelMaxX), AccelMinX,AccelMaxX, 255.0, 0.0));
            uint calcAccelY = (uint) Floor(Map(Constrain(accelY, AccelMinY, AccelMaxY), AccelMinY,AccelMaxY, 255.0, 0.0));*/

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (ControlViewModel.Switch)
                {
                    accelXLabel.Text = $"{calcAccelX}";
                    accelYLabel.Text = $"{calcAccelY}";
                }
                else
                {
                    var c = 9.81;

                    var ax = accelX * c;
                    var ay = accelY * c;

                    accelXLabel.Text = $"{ax:0.00}";
                    accelYLabel.Text = $"{ay:0.00}";

                    /*var ax = accelX * 9.81;
                    var ay = accelY * 9.81;

                    accelXLabel.Text = $"{ax:0.00} m/s\u00B2";
                    accelYLabel.Text = $"{ay:0.00} m/s\u00B2";*/
                }
            });
        }

        #endregion

        #region BackButtonPress

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform.Equals(Device.Android) ||
                Device.RuntimePlatform.Equals(Device.iOS))
            {
                OnClosePageRequested();
                return true;
            }

            else
            {
                base.OnBackButtonPressed();
                return false;
            }
        }

        private async void OnClosePageRequested()
        {
            var result = await DisplayAlert("Wait", "Do You Want to Exit?", "Yes", "Cancel");

            if (result) await Navigation.PopAsync(true);
            //            if (result) await ControlViewModel.ParentNavigation.PopAsync(true);
        }

        #endregion

        #region Utility

        protected void WaitAndExecute(int milisec, Action actionToExecute)
        {
            actionToExecute();
        }

        /*protected async Task WaitAndExecute(int milisec, Action actionToExecute)
        {
            await Task.Delay(milisec);
            actionToExecute();
        }*/

        #endregion
    }
}