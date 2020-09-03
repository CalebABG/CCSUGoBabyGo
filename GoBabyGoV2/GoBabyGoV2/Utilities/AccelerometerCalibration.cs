using MvvmHelpers;

namespace GoBabyGoV2.Utilities
{
    public enum CalibrationFreeze
    {
        None,
        X,
        Y
    }

    public class AccelerometerCalibration : ObservableObject
    {
        private float _biasX;
        private float _biasY;
        private float _maxX;
        private float _maxY;
        private float _minX;
        private float _minY;

        public AccelerometerCalibration(float minX = 0.0f, float maxX = 0.0f, float minY = 0.0f, float maxY = 0.0f)
        {
            _minX = minX;
            _maxX = maxX;
            _minY = minY;
            _maxY = maxY;
        }

        public float MinX
        {
            get => _minX;
            set => SetProperty(ref _minX, value);
        }

        public float MaxX
        {
            get => _maxX;
            set => SetProperty(ref _maxX, value);
        }

        public float MinY
        {
            get => _minY;
            set => SetProperty(ref _minY, value);
        }

        public float MaxY
        {
            get => _maxY;
            set => SetProperty(ref _maxY, value);
        }

        public float BiasX
        {
            get => _biasX;
            set => SetProperty(ref _biasX, value);
        }

        public float BiasY
        {
            get => _biasY;
            set => SetProperty(ref _biasY, value);
        }
    }
}