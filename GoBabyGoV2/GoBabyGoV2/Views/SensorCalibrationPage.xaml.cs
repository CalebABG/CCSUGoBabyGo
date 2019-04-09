using GoBabyGoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoBabyGoV2.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBabyGoV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorCalibrationPage : ContentPage
    {
        public SensorCalibrationPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                AccelMonitor.AccelCalib.ShouldCalibrate = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                AccelMonitor.AccelCalib.ShouldCalibrate = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}