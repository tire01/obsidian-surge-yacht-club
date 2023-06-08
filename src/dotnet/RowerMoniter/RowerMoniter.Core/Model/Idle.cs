using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Contracts;

namespace RowerMoniter.Model
{
    public class Idle : RowEvent
    {
        private static long _Index = 0;
        public Idle() : base(_Index++) 
        {
        }

        public static Idle From(IdleMessage message) => new Idle();
    }
}
