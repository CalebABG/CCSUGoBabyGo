using GoBabyGoV2.Utilities;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class AccelerometerCalibrationViewModel
    {
        #region Properties

        public ICommand DoneButtonCommand { get; set; }

        public ICommand SetDefaultCalibrationCommand { get; set; }

        public ICommand ResetCalibrationCommand { get; set; }

        #endregion

        #region Ctor

        public AccelerometerCalibrationViewModel()
        {
            #region SetupCommands

            DoneButtonCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync(true));

            SetDefaultCalibrationCommand = new Command(async () => await SetDefaultCalibration());

            ResetCalibrationCommand = new Command(async () => await ResetCalibration());

            #endregion
        }

        #endregion

        #region Methods

        public void UpdateAxisX(float axisValue)
        {
            AccelerometerSensor.Monitor.Calibration.MinX = axisValue;
            AccelerometerSensor.Monitor.Calibration.MaxX = axisValue;
        }

        public void UpdateAxisY(float axisValue)
        {
            AccelerometerSensor.Monitor.Calibration.MinY = axisValue;
            AccelerometerSensor.Monitor.Calibration.MaxY = axisValue;
        }

        public async Task SetDefaultCalibration()
        {
            await Task.Run(() =>
            {
                AccelerometerSensor.Monitor.Calibration.MinX = AccelerometerSensor.DefaultCalibration[0];
                AccelerometerSensor.Monitor.Calibration.MaxX = AccelerometerSensor.DefaultCalibration[1];
                AccelerometerSensor.Monitor.Calibration.MinY = AccelerometerSensor.DefaultCalibration[2];
                AccelerometerSensor.Monitor.Calibration.MaxY = AccelerometerSensor.DefaultCalibration[3];
            });
        }

        public async Task ResetCalibration()
        {
            await Task.Run(() =>
            {
                switch (AccelerometerSensor.CalibrationFreezeAxis)
                {
                    case CalibrationFreeze.X:
                        AccelerometerSensor.Monitor.UpdateCalibrationAxisY(0f, UpdateAxisY);
                        break;

                    case CalibrationFreeze.Y:
                        AccelerometerSensor.Monitor.UpdateCalibrationAxisX(0f, UpdateAxisX);
                        break;

                    default:
                        AccelerometerSensor.Monitor.UpdateCalibrationAxisX(0f, UpdateAxisX);
                        AccelerometerSensor.Monitor.UpdateCalibrationAxisY(0f, UpdateAxisY);
                        break;
                }
            });
        }

        #endregion
    }
}