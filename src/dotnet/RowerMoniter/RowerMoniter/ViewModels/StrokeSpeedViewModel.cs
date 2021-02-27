using Caliburn.Micro;
using LiveCharts;
using RowerMoniter.Model;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using LiveCharts.Wpf;
using System.Windows;

namespace RowerMoniter.ViewModels
{
    public class StrokeSpeedViewModel : PropertyChangedBase, IObserver<IRowEvent>
    {

        public SeriesCollection SeriesCollection {
            get => _seriesCollection; 
            set => Set(ref _seriesCollection, value);
        }

        private SeriesCollection _seriesCollection;

        private Option<IntervalCollectionBuilder<FlywheelStatus>> _builder = Option<IntervalCollectionBuilder<FlywheelStatus>>.None;

        public StrokeSpeedViewModel() 
        {
     //       EventService.Instance.Pipeline.Subscribe(this);
   //         SeriesCollection = new SeriesCollection();
        }

        public void OnNext(IRowEvent value)
        {
            switch (value) 
            {
                case BeginStroke beginStroke:

                    _builder.IfSome(b =>
                    {
                        Application.Current.Dispatcher.Invoke(() => { RenderGraph(b.ToImmutable()); });
                     }); 
                    _builder = Some(new IntervalCollectionBuilder<FlywheelStatus>());
                    break;

                case FlywheelStatus flywheelStatus:
                    if (flywheelStatus.Ellapsed.TotalMilliseconds < 2000)
                    {
                        _builder.Do(b => b.Add(flywheelStatus));
                    }
                    break;
            }
        }

        private void RenderGraph(IntervalCollection<FlywheelStatus> intervalCollection)
        {
            var values = new ChartValues<decimal>();
            int intervalCount = 25;

            var duration = intervalCollection.Duration.Match(s => s.TotalSeconds, 0) / intervalCount;

            if (duration == 0) 
            {
                return;
            }

            for (int i = 0; i <= intervalCount; i++) 
            {
                TimeSpan offsetTime  = TimeSpan.FromSeconds(duration * i);
                var value = intervalCollection.ValueAtTime(offsetTime, Interpolate);
                value.IfSome(values.Add);
            }

            var lineSeries = new LineSeries() { Values = values };
            _seriesCollection.Clear();
            _seriesCollection.Add(lineSeries);
        }

        private decimal Interpolate(FlywheelStatus arg1, FlywheelStatus arg2)
        {
            var f1 = 1000.0m / (decimal)(arg1.Ellapsed.TotalMilliseconds * 4);
            var f2 = 1000.0m / (decimal)(arg2.Ellapsed.TotalMilliseconds * 4);

            return (f1 + f2) / 2.0m;

        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
