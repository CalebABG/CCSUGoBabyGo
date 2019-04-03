using System;
using System.Collections.Generic;
using System.Text;

namespace GoBabyGoV2.DependencyServices
{
    public interface IBluetoothManager
    {
        void Connect(string macaddr, bool secureconnection = false);

        int GetConnectionState();

        void Start();

        void Stop();
    }
}
