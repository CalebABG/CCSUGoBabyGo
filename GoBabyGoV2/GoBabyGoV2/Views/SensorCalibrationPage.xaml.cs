﻿using GoBabyGoV2.ViewModels;
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

            AccelerometerMonitor.ShouldCalibrate = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            AccelerometerMonitor.ShouldCalibrate = false;
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

            AccelerometerMonitor.CalibrationFreezeAxis = selectedValue;
        }

        #endregion
    }
}