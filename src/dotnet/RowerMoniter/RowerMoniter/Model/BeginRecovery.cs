using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Json;

namespace RowerMoniter.Model
{
    public class BeginRecovery : RowEvent
    {
        private static long _Index = 1;

        public BeginRecovery() : base(_Index++)
        {
        }

        public static BeginRecovery From(BeginRecoveryMessage message) 
        {
            return new BeginRecovery();
        }
    }

    public abstract class RowEvent : IRowEvent 
    {
        public long Index { get; }

        public DateTime Time { get; }

        protected RowEvent(long index) 
        {
            Index = index;
            Time = DateTime.Now;
        }
    }

    public interface IRowEvent 
    {
        DateTime Time { get;  }
    }
}
