﻿namespace GoBabyGoV2.DependencyServices
{
    public interface IToast
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}