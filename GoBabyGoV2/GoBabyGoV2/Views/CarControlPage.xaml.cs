//#define USE_METERS_PER_SECOND


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

using static GoBabyGoV2.Utilities.AccelerometerMonitor;
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
            else { StartMonitoring(); }
        }

        #endregion

        #region Accelerometer

        void UpdateCalibrationAxisX(float accelX)
        {
            if (accelX < Calibration.MinX) Calibration.MinX = accelX;
            if (accelX > Calibration.MaxX) Calibration.MaxX = accelX;
        }

        void UpdateCalibrationAxisY(float accelY)
        {
            if (accelY < Calibration.MinY) Calibration.MinY = accelY;
            if (accelY > Calibration.MaxY) Calibration.MaxY = accelY;
        }

        public void CarControlAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            var accelX = data.Acceleration.X - 0.15f;
            var accelY = data.Acceleration.Y;

            // Process Acceleration X and Y (Xamarin Essentials outputs in g-force units)
            uint calcAccelX = (uint) Map(accelX, Calibration.MinX, Calibration.MaxX, 255.0, 0.0);
            uint calcAccelY = (uint) Map(accelY, Calibration.MinY, Calibration.MaxY, 255.0, 0.0);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Calibration.ShouldCalibrate)
                {
                    if (CalibrationFreezeAxis.Equals("freezex")) UpdateCalibrationAxisY(accelY);
                    else if (CalibrationFreezeAxis.Equals("freezey")) UpdateCalibrationAxisX(accelX);
                    else
                    {
                        UpdateCalibrationAxisX(accelX);
                        UpdateCalibrationAxisY(accelY);
                    }
                }

                if (((CarControlViewModel) BindingContext).IsCalcTicked)
                {
                    AccelXLabel.Text = $"{calcAccelX} B";
                    AccelYLabel.Text = $"{calcAccelY} B";
                }
                else
                {
                    #if USE_METERS_PER_SECOND

                    const float c = 9.81f;
                    var ax = accelX * c;
                    var ay = accelY * c;

                    AccelXLabel.Text = $"{ax:0.00} m/s\u00B2";
                    AccelYLabel.Text = $"{ay:0.00} m/s\u00B2";

                    #else

                    AccelXLabel.Text = $"{accelX:0.00} g";
                    AccelYLabel.Text = $"{accelY:0.00} g";

                    #endif
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
    }
}