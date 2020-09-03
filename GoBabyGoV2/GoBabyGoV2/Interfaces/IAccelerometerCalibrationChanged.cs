namespace GoBabyGoV2.Interfaces
{
    public delegate void UpdateAccelAction(float accelVal);

    public interface IAccelerometerCalibrationChanged
    {
        void UpdateCalibrationAxisX(float val, UpdateAccelAction updateFunc);
        void UpdateCalibrationAxisY(float val, UpdateAccelAction updateFunc);
    }
}