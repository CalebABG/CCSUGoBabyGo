using Android.OS;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(EmulatorDetectAndroid))]

namespace GoBabyGoV2.Droid.DependencyServices
{
    public class EmulatorDetectAndroid : IEmulatorDetect
    {
        public bool IsRunningInEmulator()
        {
            if (Build.Fingerprint != null)
                if (Build.Fingerprint.Contains("vbox") ||
                    Build.Fingerprint.Contains("generic"))
                    return true;
            return false;
        }
    }
}