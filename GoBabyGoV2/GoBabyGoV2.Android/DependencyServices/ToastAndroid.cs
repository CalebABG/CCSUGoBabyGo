using Android.Widget;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Droid.DependencyServices;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(ToastAndroid))]

namespace GoBabyGoV2.Droid.DependencyServices
{
    public class ToastAndroid : IToast
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}