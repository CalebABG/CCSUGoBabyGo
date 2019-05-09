using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using MvvmHelpers;

namespace GoBabyGoV2.Utilities
{
    public class AccelerometerCalibration : ObservableObject
    {
        private bool  _shouldCalibrate;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        public AccelerometerCalibration(float minx = 0.0f, float maxx = 0.0f, float miny = 0.0f, float maxy = 0.0f)
        {
            _minX = minx;
            _maxX =  maxx;
            _minY = miny;
            _maxY =  maxy;
    }

        public bool ShouldCalibrate { get => _shouldCalibrate; set => SetProperty(ref _shouldCalibrate, value); }
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
    }
}
