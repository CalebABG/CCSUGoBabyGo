using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using GoBabyGoV2.Views;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class CarControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _switch;
        public bool Switch { get => _switch; set => SetProperty(ref _switch, value); }

        public ICommand CalibrateSensorCommand { get; set; }

        public INavigation ParentNavigation { get; }


        public CarControlViewModel(INavigation parentNavigation)
        {
            // Get the parent controls navigation handler
            ParentNavigation = parentNavigation;

            // Set Calibrate Sensor Command
            CalibrateSensorCommand = new Command(NavigateToCalibrationPage);
        }

        #region CalibrateSensorMethod

        private async void NavigateToCalibrationPage()
        {
            // If Bluetooth is connected, disconnect


            // Add new page to Navigation stack
            await ParentNavigation.PushModalAsync(new SensorCalibrationPage());
            // await Navigation.PushModalAsync(new SensorCalibrationPage
            // await Application.Current.NavigationProxy.PushModalAsync(new SensorCalibrationPage());
        }

        #endregion

        #region PropertyChangeHandler

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
