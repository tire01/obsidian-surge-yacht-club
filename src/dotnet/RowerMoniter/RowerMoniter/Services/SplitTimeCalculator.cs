using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using UnitsNet;

namespace RowerMoniter.Services
{
    public class SplitTimeCalculator  
    {
        private readonly Subject<TimeSpan> _pipeline = new Subject<TimeSpan>();

        private static readonly Length SplitDistance = Length.FromMeters(500);

        public IObservable<TimeSpan> Pipeline { get { return _pipeline; } }

        public SplitTimeCalculator(IObservable<Speed> speedSource)
        {
            speedSource.Subscribe(OnNext);
        }

        public void OnNext(Speed value)
        {
            var split = new TimeSpan((long)(SplitDistance.Meters * (1d / value.MetersPerMinutes) * TimeSpan.TicksPerMinute));
            _pipeline.OnNext(split);
        }

    }


}
