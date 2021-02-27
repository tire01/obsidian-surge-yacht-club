using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.ViewModels
{
    public class ReadoutViewModelBase : PropertyChangedBase
    {
        private LabelBoxViewModel _labelBox;

        public LabelBoxViewModel LabelBox
        {
            get => _labelBox;
            set => Set(ref _labelBox, value);

        }

    }
}
