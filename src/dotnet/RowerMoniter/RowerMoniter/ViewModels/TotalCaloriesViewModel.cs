using Caliburn.Micro;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace RowerMoniter.ViewModels
{
    public class TotalCaloriesViewModel : PropertyChangedBase
    {

        private const string Format = "S0";

        private Energy _total = Energy.FromKilocalories(0);

        private string _totalCals;

        public string TotalCals 
        {
            get => _totalCals;
            set => Set(ref _totalCals, value);
        }

        public TotalCaloriesViewModel(PaceCalculator paceCalculator)
        {
            paceCalculator.KCalPipeline.Subscribe(Update);

        }

        private void Update(double calsBurnt)
        {
            _total += Energy.FromCalories(calsBurnt);

            TotalCals = _total.ToUnit(EnergyUnit.Calorie).ToString(Format);
            System.Diagnostics.Debug.WriteLine(_totalCals);

        }
    }
}
