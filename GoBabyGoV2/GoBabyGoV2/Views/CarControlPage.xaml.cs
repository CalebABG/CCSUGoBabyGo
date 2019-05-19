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
    public partial class CarControlPage : ContentPage
    {
        #region Constructor

        public CarControlPage()
        {
            InitializeComponent();

            // Set the accelerometer change event in Monitor
            AccelerometerSensor.Monitor.SetAccelerometerChangeEvent(CarControlAccelerometerReadingChanged);

            // Debug X bias for OnePlus 6T (Model A6013)
            if (DeviceInfo.Model.ToLower().Contains("oneplus") && DeviceInfo.Model.ToLower().Contains("a6013"))
                AccelerometerSensor.Monitor.Calibration.BiasX = 0.15f;
            
            // Start monitoring
            AccelerometerSensor.Monitor.StartMonitoring();
        }

        #endregion

        #region Accelerometer

        void UpdateAxisX(float axisValue)
        {
            if (axisValue < AccelerometerSensor.Monitor.Calibration.MinX)
                AccelerometerSensor.Monitor.Calibration.MinX = axisValue;

            if (axisValue > AccelerometerSensor.Monitor.Calibration.MaxX)
                AccelerometerSensor.Monitor.Calibration.MaxX = axisValue;
        }

        void UpdateAxisY(float axisValue)
        {
            if (axisValue < AccelerometerSensor.Monitor.Calibration.MinY)
                AccelerometerSensor.Monitor.Calibration.MinY = axisValue;

            if (axisValue > AccelerometerSensor.Monitor.Calibration.MaxY)
                AccelerometerSensor.Monitor.Calibration.MaxY = axisValue;
        }


        public void CarControlAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            
            // Subtract out bias from each Axis
            var accelX = data.Acceleration.X - AccelerometerSensor.Monitor.Calibration.BiasX;
            var accelY = data.Acceleration.Y - AccelerometerSensor.Monitor.Calibration.BiasY;

            // Process Acceleration X and Y (Xamarin Essentials outputs in g-force units)
            uint calcAccelX = (uint) Map(accelX, AccelerometerSensor.Monitor.Calibration.MinX,
                AccelerometerSensor.Monitor.Calibration.MaxX, 255.0f, 0.0f);

            uint calcAccelY = (uint) Map(accelY, AccelerometerSensor.Monitor.Calibration.MinY,
                AccelerometerSensor.Monitor.Calibration.MaxY, 255.0f, 0.0f);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (AccelerometerSensor.ShouldCalibrate)
                {
                    switch (AccelerometerSensor.CalibrationFreezeAxis)
                    {
                        case CalibrationFreeze.X:
                            AccelerometerSensor.Monitor.UpdateCalibrationAxisY(accelY, UpdateAxisY);
                            break;

                        case CalibrationFreeze.Y:
                            AccelerometerSensor.Monitor.UpdateCalibrationAxisX(accelX, UpdateAxisX);
                            break;

                        default:
                            AccelerometerSensor.Monitor.UpdateCalibrationAxisX(accelX, UpdateAxisX);
                            AccelerometerSensor.Monitor.UpdateCalibrationAxisY(accelY, UpdateAxisY);
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