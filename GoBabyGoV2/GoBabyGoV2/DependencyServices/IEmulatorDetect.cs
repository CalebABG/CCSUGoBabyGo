using System;
namespace GoBabyGoV2.DependencyServices
{
    public interface IEmulatorDetect
    {
        bool IsRunningInEmulator();
    }
}
