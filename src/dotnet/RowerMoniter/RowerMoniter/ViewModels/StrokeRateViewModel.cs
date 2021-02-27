using Caliburn.Micro;
using RowerMoniter.Model;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RowerMoniter.ViewModels
{
    public class StrokeRateViewModel : ReadoutViewModelBase
    {

        private string _measuredSpm = "";
        private string _instantSpm = "";
        private string _rollingSpm = "";

        private Option<BeginStroke> _lastStroke;
        private RollingAverage _rollingSpmAccumulator = new RollingAverage();
        private MomentCollection _storkesPerMinute = new MomentCollection(TimeSpan.FromMinutes(1));

        public StrokeRateViewModel()
        {
            LabelBox = new LabelBoxViewModel { Label = "SPM", Text = "0" };

            EventService.Instance.Pipeline.OfType<BeginStroke>().Subscribe(OnNext);
        }

        public string MeasuredSpm
        {
            get => _measuredSpm;
            set => Set(ref _measuredSpm, value);  
        }

        public string InstantSpm { 
            get => _instantSpm;
            set => Set(ref _instantSpm, value); 
        }

        public string RollingSpm
        {
            get => _rollingSpm;
            set => Set(ref _rollingSpm, value);
        }

        public void OnNext(BeginStroke next)
        {
            var instantSpm = _lastStroke.Map(last => calculateInstantSpm(last, next));

            instantSpm.Do(updateRollAvergaeSpm);

            InstantSpm = instantSpm.Match(some => ((int)some).ToString(), "");
            LabelBox.Text = InstantSpm;

            updateMeasuredSpm(next);

            _lastStroke = next;
        }

        private void updateRollAvergaeSpm(decimal next)
        {
            _rollingSpmAccumulator.Queue(next);
            _rollingSpm = ((int)_rollingSpmAccumulator.Value).ToString();

        }

        private void updateMeasuredSpm(BeginStroke next)
        {
            _storkesPerMinute.Add(next);

            MeasuredSpm = _storkesPerMinute.Events().Count().ToString();
        }

        private decimal calculateInstantSpm(BeginStroke last, BeginStroke next)
        {
            return 60.0m / (decimal)(next.Time - last.Time).TotalSeconds;
        }
    }
}
