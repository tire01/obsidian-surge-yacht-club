using RowerMoniter.Model;
using RowerMoniter.Services;
using System;
using System.Reactive;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Caliburn.Micro;

namespace RowerMoniter.ViewModels
{
    public class StrokeCounterViewModel : PropertyChangedBase, IObserver<BeginStroke>
    {
        private string _strokeCount = "0";
        private int _strokeCountInt = 0;

        public string StrokeCount
        {
            get => _strokeCount;
            set => Set(ref _strokeCount,  value);
        }

        public StrokeCounterViewModel()
        {
            EventService.Instance.Pipeline.OfType<BeginStroke>().Subscribe(this);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(BeginStroke value)
        {
            _strokeCountInt++;
            StrokeCount = _strokeCountInt.ToString();
        }
    }
}
