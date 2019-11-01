using Android.App;
using Android.Content.PM;
using Android.OS;

namespace GoBabyGoV2.Droid
{
    [Activity(Label = "CCSUGoBabyGo", Icon = "@drawable/icon_round", 
        Theme = "@style/MyThemeSplash", MainLauncher = true, NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            //Thread.Sleep(1000);
            StartActivity(typeof(MainActivity));
        }
    }
}