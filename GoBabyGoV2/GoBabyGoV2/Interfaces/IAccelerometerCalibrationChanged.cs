using System;
using System.Collections.Generic;
using System.Text;

namespace GoBabyGoV2.Interfaces
{
    public interface IAccelerometerCalibrationChanged
    {
        void UpdateCalibrationAxisX(float axisValue);

        void UpdateCalibrationAxisY(float axisValue);
    }
}
