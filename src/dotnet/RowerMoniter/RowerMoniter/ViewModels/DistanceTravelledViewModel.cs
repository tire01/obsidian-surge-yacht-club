using UnitsNet;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Model;
using Caliburn.Micro;
using UnitsNet.Units;

namespace RowerMoniter.ViewModels
{
    public class DistanceTravelledViewModel : ReadoutViewModelBase
    {
        private const string Format = "S0"; 

        private Length _distanceRowed = Length.FromMeters(0);

        private string _distance;

        public string Distance 
        {
            get => _distance;
            set => Set(ref _distance, value);
        }


        public DistanceTravelledViewModel() 
        {
            LabelBox = new LabelBoxViewModel { Label = "Distance", Text = "0 m" };

            EventService.Instance.Pipeline.OfType<FlywheelStatus>().Select(o => PaceCalculator.MetersTravelledPerFlywheelRevolution / 4).Subscribe(UpdateDistance);
            Distance = _distanceRowed.ToString(Format);
        }

        private void UpdateDistance(Length distance) 
        {
            _distanceRowed += distance;
            Distance = _distanceRowed.ToUnit(LengthUnit.Meter).ToString(Format);
            LabelBox.Text = Distance;
        }
    }

}
