using Caliburn.Micro;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace RowerMoniter.ViewModels
{
    public class SpeedViewModel : PropertyChangedBase
    {
        const string Format = "G2";

        private string _speed;

        public string Speed {
            get => _speed;
            set => Set(ref _speed, value);
        }

        public SpeedViewModel(PaceCalculator speedCalculator) 
        {
            speedCalculator.PacePipeline.Subscribe(Update);
        }

        private void Update(Speed speed)
        {
            Speed = speed.ToUnit(UnitsNet.Units.SpeedUnit.MeterPerSecond).ToString(Format);
        }
    }
}
