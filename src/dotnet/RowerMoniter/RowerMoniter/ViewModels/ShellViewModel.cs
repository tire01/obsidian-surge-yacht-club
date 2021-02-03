using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private StrokeCounterViewModel _strokeCounterViewModel;


        public StrokeCounterViewModel StrokeCounter {
            get => _strokeCounterViewModel;
            set => Set(ref _strokeCounterViewModel, value);
        }

        public ShellViewModel()
        {
            StrokeCounter = new StrokeCounterViewModel();
        }


    }
}
