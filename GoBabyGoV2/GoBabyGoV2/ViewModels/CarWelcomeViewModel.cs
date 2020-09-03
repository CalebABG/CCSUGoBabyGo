using System.Windows.Input;
using GoBabyGoV2.Views;
using Xamarin.Forms;

namespace GoBabyGoV2.ViewModels
{
    public class CarWelcomeViewModel
    {
        public CarWelcomeViewModel()
        {
            GoToCarControlCommand = new Command(async () =>
                await Application.Current.MainPage.Navigation.PushAsync(new CarControlPage(), true));
        }

        public ICommand GoToCarControlCommand { get; }
    }
}