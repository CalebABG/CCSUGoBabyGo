using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.InputKit.Platforms.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

namespace GoBabyGoV2.Droid
{
    [Activity(Label = "CCSUGoBabyGo", Icon = "@drawable/icon_round", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Keyboard |
                               ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Xamarin Essentials
            Platform.Init(this, savedInstanceState);

            // Xamarin Forms
            Forms.Init(this, savedInstanceState);

            // InputKit (Checkbox, RadioButton, etc. components)
            Config.Init(this, savedInstanceState);

            // CrossCurrentActivity
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        #region Permissions

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}