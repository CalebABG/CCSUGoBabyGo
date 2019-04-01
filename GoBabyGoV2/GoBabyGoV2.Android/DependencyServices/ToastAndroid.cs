using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAndroid))]
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