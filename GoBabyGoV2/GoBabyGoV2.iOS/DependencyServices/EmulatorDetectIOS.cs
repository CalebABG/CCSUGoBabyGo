﻿using System;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.iOS.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(EmulatorDetectIOS))]
namespace GoBabyGoV2.iOS.DependencyServices
{
    public class EmulatorDetectIOS : IEmulatorDetect
    {
        public EmulatorDetectIOS()
        {
        }

        public bool IsRunningInEmulator()
        {
            if (ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.SIMULATOR) return true;
            return false;
        }
    }
}
