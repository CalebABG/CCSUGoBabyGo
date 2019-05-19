using System;
using System.Collections.Generic;
using System.Text;

namespace GoBabyGoV2.DependencyServices
{
    public interface IBluetoothManager
    {
        void Connect(string macAddress, bool secureConnection = false);

        int GetConnectionState();

        void Start();

        void Stop();
    }
}
