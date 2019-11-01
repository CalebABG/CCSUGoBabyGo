using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.InputKit;
using Xamarin.Forms.Platform.Android;

namespace GoBabyGoV2.Droid
{
    [Activity(Label = "CCSUGoBabyGo", Icon = "@drawable/icon_round", Theme = "@style/MainTheme", MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Xamarin Essentials
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Xamarin Forms
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // InputKit (Checkbox, RadioButton, etc. components)
            Plugin.InputKit.Platforms.Droid.Config.Init(this,savedInstanceState);

            // CrossCurrentActivity
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        #region Permissions

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}