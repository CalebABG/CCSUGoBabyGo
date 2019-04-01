using GoBabyGoV2.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class SensorCalibrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation ParentNavigation { get; }

        public ICommand DoneButtonCommand { get; set; }

        public ICommand SetDefaultCalibrationCommand { get; set; }

        public ICommand ResetCalibrationCommand { get; set; }

        public SensorCalibrationViewModel(INavigation parentNavigation)
        {
            ParentNavigation = parentNavigation;

            DoneButtonCommand = new Command(async () => await ParentNavigation.PopModalAsync(true));

            SetDefaultCalibrationCommand = new Command(() =>
            {
                AccelMonitor.StopAccelMonitor();

                AccelMonitor.AccelMinX = AccelMinX = AccelMonitor.AccelCalibDefault[0];
                AccelMonitor.AccelMaxX = AccelMaxX = AccelMonitor.AccelCalibDefault[1];
                AccelMonitor.AccelMinY = AccelMinY = AccelMonitor.AccelCalibDefault[2];
                AccelMonitor.AccelMaxY = AccelMaxY = AccelMonitor.AccelCalibDefault[3];

                if (DoneButtonCommand.CanExecute(null))
                    DoneButtonCommand.Execute(null);
            });

            ResetCalibrationCommand = new Command(() =>
            {
                AccelMonitor.AccelMinX = AccelMinX = 0;
                AccelMonitor.AccelMaxX = AccelMaxX = 0;
                AccelMonitor.AccelMinY = AccelMinY = 0;
                AccelMonitor.AccelMaxY = AccelMaxY = 0;
            });
        }

        public void Init()
        {
            Accelerometer.ReadingChanged += AccelerometerOnReadingChanged;

            AccelMonitor.StartAccelMonitor();
        }

        public void Dispose()
        {
            AccelMonitor.StopAccelMonitor();

            Accelerometer.ReadingChanged -= AccelerometerOnReadingChanged;

            //            Xamarin.Forms.DependencyService.Get<IToast>().ShortAlert("RemovedAccelF");
            //            Debug.Write("Removed Accel Reading Method");
        }


        private float _accelMinX;
        public float AccelMinX { get => _accelMinX; set => SetProperty(ref _accelMinX, value); }

        private float _accelMaxX;
        public float AccelMaxX { get => _accelMaxX; set => SetProperty(ref _accelMaxX, value); }

        private float _accelMinY;
        public float AccelMinY { get => _accelMinY; set => SetProperty(ref _accelMinY, value); }

        private float _accelMaxY;
        public float AccelMaxY { get => _accelMaxY; set => SetProperty(ref _accelMaxY, value); }


        private void AccelerometerOnReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;

            var accelX = data.Acceleration.X;
            var accelY = data.Acceleration.Y;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (accelX < AccelMinX) AccelMinX = accelX;
                if (accelX > AccelMaxX) AccelMaxX = accelX;

                if (accelY < AccelMinY) AccelMinY = accelY;
                if (accelY > AccelMaxY) AccelMaxY = accelY;

                AccelMonitor.AccelMinX = AccelMinX;
                AccelMonitor.AccelMaxX = AccelMaxX;
                AccelMonitor.AccelMinY = AccelMinY;
                AccelMonitor.AccelMaxY = AccelMaxY;

            });
        }

        #region PropertyChanged

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
