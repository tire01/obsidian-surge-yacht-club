using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RowerMoniter.ViewModels
{
    public class LabelBoxViewModel : PropertyChangedBase
    {
        private string _label;

        public string Label
        {
            get => _label;
            set => Set(ref _label, value);
        }
        private string _text;

        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }
    }
}
