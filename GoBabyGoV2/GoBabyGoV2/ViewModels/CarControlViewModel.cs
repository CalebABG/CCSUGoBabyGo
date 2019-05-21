using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Views;
using MvvmHelpers;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class CarControlViewModel : BaseViewModel
    {
        #region Properties

        public ICommand CalibrateSensorCommand { get; set; }

        public ICommand StopIconCommand { get; set; }

        public ICommand ShieldIconCommand { get; set; }

        public ICommand ReconnectIconCommand { get; set; }

        public ICommand DisconnectIconCommand { get; set; }


        private bool _isCalcTicked;
        public bool IsCalcTicked
        {
            get => _isCalcTicked;
            set => SetProperty(ref _isCalcTicked, value);
        }

        private bool _parentalOverrideActive = true;
        public bool ParentalOverrideActive
        {
            get => _parentalOverrideActive;
            set => SetProperty(ref _parentalOverrideActive, value);
        }

        private bool _isCalibrationButtonBusy;
        public bool IsCalibrationButtonBusy
        {
            get => _isCalibrationButtonBusy;
            set
            {
                if (_isCalibrationButtonBusy != value)
                {
                    _isCalibrationButtonBusy = value;
                    ((Command) CalibrateSensorCommand).ChangeCanExecute();
                    OnPropertyChanged();
                }
            }
        }

        #endregion


        public CarControlViewModel()
        {
            #region SetupCommands

            // Set Calibrate Sensor Command
            CalibrateSensorCommand = new Command(async () => await NavigateToCalibrationPage(),
                () => IsCalibrationButtonBusy == false);


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
                    DependencyService.Get<IToast>().ShortAlert(ParentalOverrideActive
                        ? "Parental Override Active"
                        : "Deactivating Parental Override");
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

            DisconnectIconCommand = new Command(() =>
            {
                // end connection with bluetooth

                // toast
                DependencyService.Get<IToast>().ShortAlert("Disconnecting Bluetooth...");
            });

            #endregion
        }

        #region CalibrationNavigationMethod

        private async Task NavigateToCalibrationPage()
        {
            IsCalibrationButtonBusy = true;

            // Add new page to Navigation stack
            await Application.Current.MainPage.Navigation.PushModalAsync(new AccelerometerCalibrationPage());

            IsCalibrationButtonBusy = false;
        }

        #endregion
    }
}