using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GoBabyGoV2.Views;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class CarWelcomeViewModel
    {
        public ICommand GoToCarControlCommand { get; }

        public CarWelcomeViewModel(INavigation navigation)
        {
            GoToCarControlCommand = new Command(async () => await navigation.PushAsync(new CarControlPage(), true));
        }
    }
}
