using System;
using GoBabyGoV2.Utilities;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBabyGoV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccelerometerCalibrationPage : ContentPage
    {
        #region Ctor

        public AccelerometerCalibrationPage()
        {
            InitializeComponent();
        }

        #endregion

        #region OnAppearingDisappearing

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AccelerometerSensor.ShouldCalibrate = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            AccelerometerSensor.ShouldCalibrate = false;
        }

        #endregion

        #region BackButtonPressed

        protected override bool OnBackButtonPressed(){ return true; }

        #endregion

        #region CalibrationFreezeAxis

        private void RadioButtonGroupView_OnSelectedItemChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButtonGroupView radiobuttonGroup)) return;

            var selectedValue = (CalibrationFreeze) radiobuttonGroup.SelectedItem;

            AccelerometerSensor.CalibrationFreezeAxis = selectedValue;
        }

        #endregion
    }
}