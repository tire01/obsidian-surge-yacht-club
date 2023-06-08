using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Contracts;
using UnitsNet;

namespace RowerMoniter.Model
{
    public class FlywheelStatus : RowEvent
    {
        private static long _Index = 1;

        public Angle RadiansPerSecond { get; }
        public TimeSpan Ellapsed { get; }

        public FlywheelStatus(Angle radiansPerSecond, TimeSpan ellapsed) : base(_Index++)
        {
            RadiansPerSecond = radiansPerSecond;
            Ellapsed = ellapsed;
        }

        public static FlywheelStatus From(FlywheelSensorMessage message) 
        {

            return new FlywheelStatus(Angle.FromRadians(message.RadiansPerSecond), TimeSpan.FromMilliseconds(message.Ellapsed));
        }
    }
}
