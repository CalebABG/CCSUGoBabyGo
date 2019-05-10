//#define USE_METERS_PER_SECOND


using GoBabyGoV2.Utilities;
using GoBabyGoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static System.Math;
using static GoBabyGoV2.Utilities.Utilities;


namespace GoBabyGoV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarControlPage : ContentPage, IAccelerometerCalibrationChanged
    {
        #region PublicProperties

        public static AccelerometerMonitor AccelerometerMonitor = new AccelerometerMonitor();

        #endregion

        #region Constructor

        public CarControlPage()
        {
            InitializeComponent();

            // Set the accelerometer change event in Monitor
            AccelerometerMonitor.SetAccelerometerChangeEvent(CarControlAccelerometerReadingChanged);

            // Debug X bias for OnePlus 6T
            AccelerometerMonitor.Calibration.BiasX = 0.15f;
            
            // The only time I don't want the accelerometer to start is if it's in
            // the emulator and it's iOS
            if (DependencyService.Get<IEmulatorDetect>().IsRunningInEmulator() && 
                Device.RuntimePlatform == Device.iOS)
            {
                Debug.WriteLine("Not Starting Accelerometer B/c Will Crash");
            }
            else
            {
                AccelerometerMonitor.StartMonitoring();
            }
        }

        #endregion

        #region Accelerometer

        public void UpdateCalibrationAxisX(float axisValue)
        {
            if (axisValue < AccelerometerMonitor.Calibration.MinX) AccelerometerMonitor.Calibration.MinX = axisValue;
            if (axisValue > AccelerometerMonitor.Calibration.MaxX) AccelerometerMonitor.Calibration.MaxX = axisValue;
        }

        public void UpdateCalibrationAxisY(float axisValue)
        {
            if (axisValue < AccelerometerMonitor.Calibration.MinY) AccelerometerMonitor.Calibration.MinY = axisValue;
            if (axisValue > AccelerometerMonitor.Calibration.MaxY) AccelerometerMonitor.Calibration.MaxY = axisValue;
        }

        public void CarControlAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            
            // Subtract out bias from each Axis
            var accelX = data.Acceleration.X - AccelerometerMonitor.Calibration.BiasX;
            var accelY = data.Acceleration.Y - AccelerometerMonitor.Calibration.BiasY;

            // Process Acceleration X and Y (Xamarin Essentials outputs in g-force units)
            uint calcAccelX = (uint) Map(accelX, AccelerometerMonitor.Calibration.MinX, AccelerometerMonitor.Calibration.MaxX, 255.0f, 0.0f);
            uint calcAccelY = (uint) Map(accelY, AccelerometerMonitor.Calibration.MinY, AccelerometerMonitor.Calibration.MaxY, 255.0f, 0.0f);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (AccelerometerMonitor.ShouldCalibrate)
                {
                    switch (AccelerometerMonitor.CalibrationFreezeAxis)
                    {
                        case CalibrationFreeze.X:
                            UpdateCalibrationAxisY(accelY);
                            break;

                        case CalibrationFreeze.Y:
                            UpdateCalibrationAxisX(accelX);
                            break;

                        default:
                            UpdateCalibrationAxisX(accelX);
                            UpdateCalibrationAxisY(accelY);
                            break;
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