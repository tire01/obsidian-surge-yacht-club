using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Json;
using UnitsNet;

namespace RowerMoniter.Model
{
    public class EndRecovery : RowEvent
    {
        private static long _Index = 1;

        public EndRecovery(Length length, TimeSpan duration) : base(_Index++)
        {
            Length = length;
            Duration = duration;
        }

        public Length Length { get; }
        public TimeSpan Duration { get; }

        public static EndRecovery From(EndRecoveryMessage message) 
        {
            var length = Length.FromInches(message.Length * 14);
            var duration = TimeSpan.FromMilliseconds(message.Duration);

            return new EndRecovery(length, duration);
        }
    }
}
