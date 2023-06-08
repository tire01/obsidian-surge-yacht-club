using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Contracts;
using UnitsNet;

namespace RowerMoniter.Model
{
    public class EndStroke : RowEvent
    {
        private static long _Index = 1;

        public EndStroke(Length length, TimeSpan duration) : base(_Index++)
        {
            Length = length;
            Duration = duration;
        }

        public Length Length { get; }
        public TimeSpan Duration { get; }

        public static EndStroke From(EndStrokeMessage message)
        {
            var length = Length.FromInches(message.Length * 14);
            var duration = TimeSpan.FromMilliseconds(message.Duration);

            return new EndStroke(length, duration);
        }
    }
}
