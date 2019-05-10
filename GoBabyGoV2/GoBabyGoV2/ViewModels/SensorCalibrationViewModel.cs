using GoBabyGoV2.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GoBabyGoV2.Interfaces;
using GoBabyGoV2.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class SensorCalibrationViewModel : IAccelerometerCalibrationChanged
    {
        #region Properties

        public ICommand DoneButtonCommand { get; set; }

        public ICommand SetDefaultCalibrationCommand { get; set; }

        public ICommand ResetCalibrationCommand { get; set; }

        #endregion

        #region Ctor

        public SensorCalibrationViewModel()
        {
            #region SetupCommands

            DoneButtonCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync(true));

            SetDefaultCalibrationCommand = new Command(async () => await SetDefaultCalibration());

            ResetCalibrationCommand = new Command(async () => await ResetCalibration());

            #endregion
        }

        #endregion

        #region Methods

        public void UpdateCalibrationAxisX(float axisValue)
        {
            CarControlPage.AccelerometerMonitor.Calibration.MinX = axisValue;
            CarControlPage.AccelerometerMonitor.Calibration.MaxX = axisValue;
        }

        public void UpdateCalibrationAxisY(float axisValue)
        {
            CarControlPage.AccelerometerMonitor.Calibration.MinY = axisValue;
            CarControlPage.AccelerometerMonitor.Calibration.MaxY = axisValue;
        }

        public async Task SetDefaultCalibration()
        {
            await Task.Run(() =>
            {
                CarControlPage.AccelerometerMonitor.Calibration.MinX = AccelerometerMonitor.DefaultCalibration[0];
                CarControlPage.AccelerometerMonitor.Calibration.MaxX = AccelerometerMonitor.DefaultCalibration[1];
                CarControlPage.AccelerometerMonitor.Calibration.MinY = AccelerometerMonitor.DefaultCalibration[2];
                CarControlPage.AccelerometerMonitor.Calibration.MaxY = AccelerometerMonitor.DefaultCalibration[3];
            });
        }

        public async Task ResetCalibration()
        {
            await Task.Run(() =>
            {
                switch (AccelerometerMonitor.CalibrationFreezeAxis)
                {
                    case CalibrationFreeze.X:
                        UpdateCalibrationAxisY(0f);
                        break;

                    case CalibrationFreeze.Y:
                        UpdateCalibrationAxisX(0f);
                        break;

                    default:
                        UpdateCalibrationAxisX(0f);
                        UpdateCalibrationAxisY(0f);
                        break;
                }
            });
        }

        #endregion
    }
}