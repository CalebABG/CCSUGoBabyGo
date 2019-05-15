using System;
using System.Collections.Generic;
using System.Text;

namespace GoBabyGoV2.Interfaces
{
    public delegate void Update(float accelVal);

    public interface IAccelerometerCalibrationChanged
    {
        void UpdateCalibrationAxisX(float val, Update updateFunc);

        void UpdateCalibrationAxisY(float val, Update updateFunc);
    }
}
