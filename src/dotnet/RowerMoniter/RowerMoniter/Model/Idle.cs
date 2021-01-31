using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Model
{
    public class Idle : RowEvent
    {
        private static long _Index = 0;
        public Idle() : base(_Index++) 
        {
        }

        public static Idle Create(IdleMessage message) => new Idle();
    }
}
