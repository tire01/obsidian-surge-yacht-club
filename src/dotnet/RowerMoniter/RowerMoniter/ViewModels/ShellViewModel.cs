using RowerMoniter.Model;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace RowerMoniter.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private StrokeCounterViewModel _strokeCounter;
        private StrokeRateViewModel _strokeRate;
        private DistanceTravelledViewModel _distance;
        private WorkTimeViewModel _workTime;
        private CaloriesViewModel _calories;
        private SplitTimeViewModel _splitTime;

        private PaceCalculator _paceCalculator;

        public WorkTimeViewModel WorkTime
        {
            get => _workTime;
            private set => Set(ref _workTime, value);
        }

        public ShellViewModel()
        {

            _paceCalculator = new PaceCalculator(EventService.Instance.Pipeline.OfType<FlywheelStatus>().Select(o => PaceCalculator.MetersTravelledPerFlywheelRevolution / 4));

            WorkTime = new WorkTimeViewModel();
            StrokeCounter = new StrokeCounterViewModel();
            StrokeRate = new StrokeRateViewModel();
            Distance = new DistanceTravelledViewModel();

            SplitTime = new SplitTimeViewModel(_paceCalculator);
            Calories = new CaloriesViewModel(_paceCalculator.KCalPipeline);



        }

        public StrokeCounterViewModel StrokeCounter
        {
            get => _strokeCounter;
            set => Set(ref _strokeCounter, value);
        }

        public StrokeRateViewModel StrokeRate
        {
            get => _strokeRate;
            set => Set(ref _strokeRate, value);
        }

        public DistanceTravelledViewModel Distance
        {
            get => _distance;
            private set => Set(ref _distance, value);

        }

        public CaloriesViewModel Calories
        {
            get => _calories;
            private set => Set(ref _calories, value);
        }


        public SplitTimeViewModel SplitTime
        {
            get => _splitTime;
            private set => Set(ref _splitTime, value);
        }

    }
}
