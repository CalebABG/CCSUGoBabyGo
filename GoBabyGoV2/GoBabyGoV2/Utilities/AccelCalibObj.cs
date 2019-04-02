using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GoBabyGoV2.Utilities
{
    public class AccelCalibObj : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _shouldCalibrate = false;
        public bool ShouldCalibrate { get => _shouldCalibrate; set => SetProperty(ref _shouldCalibrate, value); }

        private static float _accelMinX = AccelMonitor.AccelCalibDefault[0];
        public float AccelMinX { get => _accelMinX; set => SetProperty(ref _accelMinX, value); }

        private float _accelMaxX = AccelMonitor.AccelCalibDefault[1];
        public float AccelMaxX { get => _accelMaxX; set => SetProperty(ref _accelMaxX, value); }

        private float _accelMinY = AccelMonitor.AccelCalibDefault[2];
        public float AccelMinY { get => _accelMinY; set => SetProperty(ref _accelMinY, value); }

        private float _accelMaxY = AccelMonitor.AccelCalibDefault[3];
        public float AccelMaxY { get => _accelMaxY; set => SetProperty(ref _accelMaxY, value); }

        #region PropertyChanged

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

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
