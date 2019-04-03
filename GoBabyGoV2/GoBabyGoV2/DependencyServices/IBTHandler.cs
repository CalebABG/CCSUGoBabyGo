using System;
using System.Collections.Generic;
using System.Text;

namespace GoBabyGoV2.DependencyServices
{
    public interface IBTHandler
    {
        void OnMessageStateChange(Action action);

        void OnMessageWrite(Action action);

        void OnIncomingData(Action action);


    }
}
