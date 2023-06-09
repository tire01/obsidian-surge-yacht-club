using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowerMoniter.Contracts;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RowerMoniter.Model
{
    public class EventFactory
    {
        public static RowEvent CreateEvent(Poco message)
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

            throw new NotImplementedException($"Cannot create event for  {message.GetType().Name}.");
        }

        public static Poco ToMessage(IRowEvent e)
        {
            switch (e)
            {
                case BeginRecovery br:
                    return new BeginRecoveryMessage() { };
                case EndRecovery er:
                    return new EndRecoveryMessage() { Duration = (int)er.Duration.TotalMilliseconds, Length = (int)er.Length.Inches };
                case BeginStroke bs:
                    return new BeginStrokeMessage() { Count = bs.Count };
                case EndStroke es:
                    return new EndStrokeMessage() { Duration = (int)es.Duration.TotalMilliseconds, Length = (int)es.Length.Inches };
                case Idle idle:
                    return new IdleMessage() { };
                case FlywheelStatus fs:
                    return new FlywheelSensorMessage() { RadiansPerSecond = (decimal)fs.RadiansPerSecond.Revolutions };
            }

            throw new NotImplementedException($"No serializeable type known for {e.GetType().Name}.");
        }
    }
}
