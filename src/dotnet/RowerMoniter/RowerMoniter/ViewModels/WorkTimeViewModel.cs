using Caliburn.Micro;
using LanguageExt;
using RowerMoniter.Model;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static LanguageExt.Prelude;

namespace RowerMoniter.ViewModels
{
    public class WorkTimeViewModel : ReadoutViewModelBase
    {
        private string _worktime = string.Empty;
        public string WorkTime {
            get => _worktime;
            set => Set(ref _worktime, value); 
        }

        private Option<DateTime> _beginWork = Option<DateTime>.None;

        public WorkTimeViewModel() 
        {
            LabelBox = new LabelBoxViewModel() { Label = "Work Time", Text = "0" };

            Observable.Interval(TimeSpan.FromMilliseconds(100)).Subscribe(Update);
            
            EventService.Instance.Pipeline.OfType<BeginStroke>().Subscribe(Update);
            EventService.Instance.Pipeline.OfType<Idle>().Subscribe(Reset);
        }

        private void Update(long someLongValue)
        {

            var ellapsed = _beginWork.Match(currentTime => DateTime.Now - currentTime, TimeSpan.Zero);

            if (ellapsed < TimeSpan.FromHours(1))
            {
                WorkTime = $"{ellapsed.Minutes}:{ellapsed.ToString("ss")}";
                LabelBox.Text = WorkTime;
            }
            else
            {
                WorkTime = $"{ellapsed.Hours}:{ellapsed.Minutes}:{ellapsed.ToString("ss")}";
                LabelBox.Text = WorkTime;
            }
        }


        public void Update(BeginStroke beginStroke) 
        {
            if (_beginWork.IsNone)
            {
                _beginWork = Some(DateTime.Now);
            }
        }


        public void Reset(Idle beginStroke)
        {
            _beginWork = Option<DateTime>.None;
        }
    }
}
