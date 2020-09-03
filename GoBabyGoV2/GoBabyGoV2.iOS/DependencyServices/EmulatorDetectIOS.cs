using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.iOS.DependencyServices;
using ObjCRuntime;
using Xamarin.Forms;

[assembly: Dependency(typeof(EmulatorDetectIOS))]

namespace GoBabyGoV2.iOS.DependencyServices
{
    public class EmulatorDetectIOS : IEmulatorDetect
    {
        public bool IsRunningInEmulator()
        {
            return Runtime.Arch == Arch.SIMULATOR;
        }
    }
}