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
        private StrokeSpeedViewModel _strokeSpeed;
        private DistanceTravelledViewModel _distance;
        private WorkTimeViewModel _workTime;
        private CaloriesViewModel _calories;

        private SpeedViewModel _speed;
        private SplitTimeViewModel _splitTime;


        private PaceCalculator _paceCalculator;
        private TotalCaloriesViewModel totalCals;

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
            StrokeSpeed = new StrokeSpeedViewModel();
            Distance = new DistanceTravelledViewModel();

            Speed = new SpeedViewModel(_paceCalculator);
            SplitTime = new SplitTimeViewModel(_paceCalculator);
            TotalCals = new TotalCaloriesViewModel(_paceCalculator);
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
        public StrokeSpeedViewModel StrokeSpeed
        {
            get => _strokeSpeed;
            private set => Set(ref _strokeSpeed, value);
        }
        public DistanceTravelledViewModel Distance
        {
            get => _distance;
            private set => Set(ref _distance, value);

        }
        public SpeedViewModel Speed
        {
            get => _speed;
            private set => Set(ref _speed, value);
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

        public TotalCaloriesViewModel TotalCals
        {
            get => totalCals;
            private set => Set(ref totalCals, value);
        }
    }
}
