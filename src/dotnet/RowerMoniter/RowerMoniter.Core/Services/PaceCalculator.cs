using RowerMoniter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace RowerMoniter.Services
{


    public class PaceCalculator
    {
        private readonly BehaviorSubject<Speed> _pace = new BehaviorSubject<Speed>(Speed.Zero.ToUnit(SpeedUnit.MeterPerSecond));
        private readonly BehaviorSubject<Power> _power = new BehaviorSubject<Power>(Power.Zero.ToUnit(PowerUnit.Watt));
        private readonly BehaviorSubject<double> _cals = new BehaviorSubject<double>(0);

        private Length _diatanceTotal = Length.Zero;
        private DateTime _strokeStart  = DateTime.Now;

        public IObservable<Speed> PacePipeline => _pace;
        public IObservable<Power> PowerPipeline => _power;
        public IObservable<double> KCalPipeline => _cals;


        // I've used an extremely scientify method to determine this:  Cause it feels about right.
        public static Length  MetersTravelledPerFlywheelRevolution => Length.FromMeters(1);

        public PaceCalculator(IObservable<Length> distanceSource)
        {
            distanceSource.Subscribe(DistanceUpdated);
            EventService.Instance.Pipeline.OfType<BeginStroke>().Subscribe(NextStroke);
        }

        public void DistanceUpdated(Length value)
        {
            _diatanceTotal += value; 
        }

        public void NextStroke(BeginStroke message) 
        {
            var lastStrokeDistance = _diatanceTotal;
            var nextStrokeDistance = Length.Zero;

            var nextStrokeStart = DateTime.Now;
            var lastStrokeStart = _strokeStart;

            var ellapsed = nextStrokeStart - lastStrokeStart;

            var speed = Speed.FromMetersPerSecond(lastStrokeDistance.Meters / ellapsed.TotalSeconds);
            var pace = 1d / speed.MetersPerSecond;

            var averagePower = Power.FromWatts(2.8 / Math.Pow(pace, 3));
            var calories = Energy.FromJoules(averagePower.Watts).ToUnit(EnergyUnit.Calorie);

            // This should just be KCals * ellapsed but multiplying it by 10 lines up with esitimates from Google searches.  
            var calorieBurned = calories.Kilocalories * ellapsed.Seconds * 10;

            _strokeStart = nextStrokeStart;
            _diatanceTotal = nextStrokeDistance;

            _pace.OnNext(speed);
            _power.OnNext(averagePower);
            _cals.OnNext(calorieBurned);
        }
    }


}
