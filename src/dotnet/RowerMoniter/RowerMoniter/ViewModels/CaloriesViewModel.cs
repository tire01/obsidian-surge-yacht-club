using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.ViewModels
{
    public class CaloriesViewModel : ReadoutViewModelBase
    {
        public CaloriesViewModel(IObservable<double> caloriesBurned) 
        {
            LabelBox = new LabelBoxViewModel() { Label = "Calories", Text = "0" };

            caloriesBurned.Subscribe(OnNext);
        }

        private void OnNext(double cals)
        {
            LabelBox.Text = ((int)cals).ToString();
        }
    }
}
