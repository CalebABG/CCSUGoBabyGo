using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Views;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class CarControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isCalcTicked;
        public bool IsCalcTicked { get => _isCalcTicked; set => SetProperty(ref _isCalcTicked, value); }

        private bool _parentalOverrideActive = true;
        public bool ParentalOverrideActive { get => _parentalOverrideActive; set => SetProperty(ref _parentalOverrideActive, value); }

        public ICommand CalibrateSensorCommand { get; set; }

        public ICommand StopIconCommand { get; set; }

        public ICommand ShieldIconCommand { get; set; }

        public ICommand ReconnectIconCommand { get; set; }

        public INavigation ParentNavigation { get; }


        public CarControlViewModel(INavigation parentNavigation)
        {
            // Get the parent controls navigation handler
            ParentNavigation = parentNavigation;

            // Set Calibrate Sensor Command
            CalibrateSensorCommand = new Command(NavigateToCalibrationPage);

            StopIconCommand = new Command(() =>
            {
                // send stop motor command over Bluetooth

                // show toast
                DependencyService.Get<IToast>().ShortAlert("Stopping Motors!");
            });

            ShieldIconCommand = new Command(() =>
            {
                ParentalOverrideActive = !ParentalOverrideActive;

                try
                {
                    // send status to Arduino


                    // show toast
                    DependencyService.Get<IToast>().ShortAlert(ParentalOverrideActive ? 
                        "Parental Override Active" : "Deactivating Parental Override");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            ReconnectIconCommand = new Command(() =>
            {
                // start connection to bluetooth

                // toast
                DependencyService.Get<IToast>().ShortAlert("Connecting...");
            });
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
