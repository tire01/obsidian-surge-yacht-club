using Caliburn.Micro;
using RowerMoniter.Services;
using System;
using UnitsNet;

namespace RowerMoniter.ViewModels
{
    public class SplitTimeViewModel : ReadoutViewModelBase
    {
        private string _splitTime;

        public string SplitTime
        {
            get => _splitTime;
            set => Set(ref _splitTime, value);
        }

        public SplitTimeViewModel(PaceCalculator calculator)
        {
            LabelBox = new LabelBoxViewModel() { Label = "Split 500 m" };

            calculator.PacePipeline.Subscribe(Update);
        }

        private void Update(Speed speed)
        {
            if (speed.MetersPerSecond == 0) 
            {
                SplitTime = "00:00";
                LabelBox.Text = SplitTime;
                return;
            }

            // convert m/s => s/500m
            var pace = (1d / speed.MetersPerSecond) * 500;
            var splitTime = TimeSpan.FromSeconds(pace);

            SplitTime = $"{splitTime.Minutes}:{splitTime.ToString("ss")}";
            LabelBox.Text = SplitTime;
        }
    }
}
