using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Json;

namespace RowerMoniter.Model
{
    public class BeginStroke : RowEvent
    {
        private static long _Index = 1;

        public int Count { get; }

        public BeginStroke(int count) : base(_Index++)
        {
            Count = count;
        }

        public static BeginStroke From(BeginStrokeMessage message) 
        {
            return new BeginStroke(message.Count);
        }
    }
}
