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

        public SensorCalibrationViewModel()
        {
            #region SetupCommands

            DoneButtonCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync(true));

            SetDefaultCalibrationCommand = new Command(() =>
            {
                AccelMonitor.AccelCalib.AccelMinX = AccelMonitor.AccelCalibDefault[0];
                AccelMonitor.AccelCalib.AccelMaxX = AccelMonitor.AccelCalibDefault[1];
                AccelMonitor.AccelCalib.AccelMinY = AccelMonitor.AccelCalibDefault[2];
                AccelMonitor.AccelCalib.AccelMaxY = AccelMonitor.AccelCalibDefault[3];

                if (DoneButtonCommand.CanExecute(null))
                    DoneButtonCommand.Execute(null);
            });

            ResetCalibrationCommand = new Command(() =>
            {
                AccelMonitor.AccelCalib.AccelMinX = 0.0f;
                AccelMonitor.AccelCalib.AccelMaxX = 0.0f;
                AccelMonitor.AccelCalib.AccelMinY = 0.0f;
                AccelMonitor.AccelCalib.AccelMaxY = 0.0f;
            });

            #endregion
        }
    }
}
