using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Contracts;

namespace RowerMoniter.Model
{
    public class EventFactory 
    {
        public static RowEvent CreateEvent(PocoObject message) 
        {
            switch (message)
            {
                case BeginRecoveryMessage br:
                    return BeginRecovery.From(br);
                case EndRecoveryMessage er:
                    return EndRecovery.From(er);
                case BeginStrokeMessage bs:
                    return BeginStroke.From(bs);
                case EndStrokeMessage es:
                    return EndStroke.From(es);
                case IdleMessage idle:
                    return Idle.From(idle);
                case FlywheelSensorMessage fs:
                    return FlywheelStatus.From(fs);

            }

            throw new NotImplementedException($"No event found for {message.GetType().Name}");
        }
    }
}
