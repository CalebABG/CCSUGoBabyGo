using System;
using System.Collections.Generic;
using System.Text;

namespace GoBabyGoV2.DependencyServices
{
    public interface IToast
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}