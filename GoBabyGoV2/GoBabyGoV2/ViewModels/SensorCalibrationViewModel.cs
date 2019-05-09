using GoBabyGoV2.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class SensorCalibrationViewModel
    {
        #region Properties

        public ICommand DoneButtonCommand { get; set; }

        public ICommand SetDefaultCalibrationCommand { get; set; }

        public ICommand ResetCalibrationCommand { get; set; }

        #endregion

        void UpdateCalibrationAxisX(float accelX)
        {
            AccelerometerMonitor.Calibration.MinX = accelX;
            AccelerometerMonitor.Calibration.MaxX = accelX;
        }

        void UpdateCalibrationAxisY(float accelY)
        {
            AccelerometerMonitor.Calibration.MinY = accelY;
            AccelerometerMonitor.Calibration.MaxY = accelY;
        }

        public SensorCalibrationViewModel()
        {
            #region SetupCommands

            DoneButtonCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync(true));

            SetDefaultCalibrationCommand = new Command(() =>
            {
                AccelerometerMonitor.Calibration.MinX = AccelerometerMonitor.DefaultCalibration[0];
                AccelerometerMonitor.Calibration.MaxX = AccelerometerMonitor.DefaultCalibration[1];

                AccelerometerMonitor.Calibration.MinY = AccelerometerMonitor.DefaultCalibration[2];
                AccelerometerMonitor.Calibration.MaxY = AccelerometerMonitor.DefaultCalibration[3];
            });

            ResetCalibrationCommand = new Command(() =>
            {
                if (AccelerometerMonitor.CalibrationFreezeAxis.Equals("freezex")) UpdateCalibrationAxisY(0f);
                else if (AccelerometerMonitor.CalibrationFreezeAxis.Equals("freezey")) UpdateCalibrationAxisX(0f);
                else
                {
                    UpdateCalibrationAxisX(0f);
                    UpdateCalibrationAxisY(0f);
                }
            });

            #endregion
        }
    }
}
