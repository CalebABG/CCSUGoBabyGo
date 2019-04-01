using GoBabyGoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBabyGoV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorCalibrationPage : ContentPage
    {
        public SensorCalibrationViewModel CalibrationViewModel { get; set; }
        public SensorCalibrationPage()
        {
            InitializeComponent();

            CalibrationViewModel = new SensorCalibrationViewModel(Navigation);

            BindingContext = CalibrationViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                CalibrationViewModel.Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                CalibrationViewModel.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}