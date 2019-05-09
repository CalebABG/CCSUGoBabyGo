using GoBabyGoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoBabyGoV2.Utilities;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBabyGoV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorCalibrationPage : ContentPage
    {
        #region Ctor

        public SensorCalibrationPage()
        {
            InitializeComponent();
        }

        #endregion

        #region OnAppearingDisappearing

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AccelerometerMonitor.Calibration.ShouldCalibrate = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            AccelerometerMonitor.Calibration.ShouldCalibrate = false;
        }

        #endregion

        protected override bool OnBackButtonPressed(){ return true; }

        private void RadioButtonGroupView_OnSelectedItemChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButtonGroupView radiobuttonGroup)) return;

            var selectedvalue = radiobuttonGroup.SelectedItem.ToString();

            AccelerometerMonitor.CalibrationFreezeAxis = selectedvalue;
        }
    }
}