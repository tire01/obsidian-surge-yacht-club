using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Json;
using UnitsNet;

namespace RowerMoniter.Model
{
    public class FlywheelStatus : RowEvent
    {
        private static long _Index = 1;

        public Angle RadiansPerSecond { get; }

        public FlywheelStatus(Angle radiansPerSecond) : base(_Index++)
        {
            RadiansPerSecond = radiansPerSecond;
        }

        public static FlywheelStatus From(UpdateMessage message) 
        {

            return new FlywheelStatus(Angle.FromRadians(message.RadiansPerSecond));
        }
    }
}
