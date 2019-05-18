using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace GoBabyGoV2.Droid
{
    [Activity(Label = "CCSUGoBabyGo", Icon = "@drawable/icon_round", 
        Theme = "@style/MyThemeSplash", MainLauncher = true, NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            Task.Run(() => 
            {
                Thread.Sleep(1000);
                StartActivity(typeof(MainActivity));
            });
        }
    }
}