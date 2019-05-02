using GoBabyGoV2.Utilities;
using GoBabyGoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoBabyGoV2.DependencyServices;
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
        #region Constructor

        public CarControlPage()
        {
            InitializeComponent();

            AddAccelerometerCallback(CarControlAccelerometerReadingChanged);

            // the only time I don't want the accelerometer to start is if it's in
            // the emulator and it's iOS
            if (DependencyService.Get<IEmulatorDetect>().IsRunningInEmulator() && Device.RuntimePlatform == Device.iOS)
            {
                Console.WriteLine("Not starting Accelerometer b/c will crash");
            }
            else { StartAccelMonitor(); }
        }

        #endregion

        #region OnAppearDisappear

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /*WaitAndExecute(async () =>
            {
                await Task.Delay(1500);
                DependencyService.Get<IToast>().ShortAlert("Welcome");
            });*/
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            /*try
            {
                AccelMonitor.StopAccelMonitor();

                // Unsub from reading changed
                Accelerometer.ReadingChanged -= CarControlAccelerometerReadingChanged;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }*/
            // Xamarin.Forms.DependencyService.Get<IToast>().ShortAlert("RemovedAccelC");
        }

        #endregion

        #region Accelerometer

        public void CarControlAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            var accelX = data.Acceleration.X;
            var accelY = data.Acceleration.Y;

            // Process Acceleration X and Y (Xamarin Essentials outputs in G-force units)
            uint calcAccelX = (uint) Round(Map(accelX, AccelCalib.AccelMinX, AccelCalib.AccelMaxX, 255.0, 0.0));
            uint calcAccelY = (uint) Round(Map(accelY, AccelCalib.AccelMinY, AccelCalib.AccelMaxY, 255.0, 0.0));

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (AccelCalib.ShouldCalibrate)
                {
                    if (accelX < AccelCalib.AccelMinX) AccelCalib.AccelMinX = accelX;
                    if (accelX > AccelCalib.AccelMaxX) AccelCalib.AccelMaxX = accelX;

                    if (accelY < AccelCalib.AccelMinY) AccelCalib.AccelMinY = accelY;
                    if (accelY > AccelCalib.AccelMaxY) AccelCalib.AccelMaxY = accelY;
                }

                if (((CarControlViewModel) BindingContext).IsCalcTicked)
                {
                    accelXLabel.Text = $"{calcAccelX}";
                    accelYLabel.Text = $"{calcAccelY}";
                }
                else
                {
                    const float c = 9.81f;
                    var ax = accelX * c;
                    var ay = accelY * c;

                    accelXLabel.Text = $"{ax:0.00} m/s\u00B2";
                    accelYLabel.Text = $"{ay:0.00} m/s\u00B2";
                }
            });
        }

        #endregion

        #region BackButtonPress

        protected override bool OnBackButtonPressed() { return true; }

        private async void OnExitRequested()
        {
            var result = await DisplayAlert("Wait", "Do You Want to Exit?", "Yes", "Cancel");
            if (result) await Navigation.PopAsync(true);
        }

        private void MenuItem_OnExitClicked(object sender, EventArgs e)
        {
            OnExitRequested();
        }

        #endregion

        #region UtilityFunctions

        protected void WaitAndExecute(Action actionToExecute, int delay = 1000)
        {
            actionToExecute();
        }

        #endregion
    }
}