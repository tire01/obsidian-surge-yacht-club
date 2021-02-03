using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Newtonsoft.Json;
using RowerMoniter.Contracts;
using RowerMoniter.Model;
using System.Reactive.Subjects;

namespace RowerMoniter.Services
{
    public sealed class EventService {

        private static Lazy<EventService> _instance = new Lazy<EventService>(() => new EventService());

        public static EventService Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private EventService() { }

        private Subject<IRowEvent> _pipeline = new Subject<IRowEvent>();

        public IObservable<IRowEvent> Pipeline { get { return _pipeline; } }

        public static Option<PocoObject> ParseLine(string line)
        {
            var firstColonIndex = line.IndexOf(':');
            if (firstColonIndex < 0)
            {
                return Option<PocoObject>.None;
            }

            var messageType = line.Substring(0, firstColonIndex);
            var jsonStartIndex = firstColonIndex + 1;

            if (line.Length < jsonStartIndex)
            {
                return Option<PocoObject>.None;
            }

            switch (messageType)
            {
                case "beginStroke":
                    return Some((PocoObject)JsonConvert.DeserializeObject<BeginStrokeMessage>(line.Substring(jsonStartIndex)));
                case "update":
                    return Some((PocoObject)JsonConvert.DeserializeObject<FlywheelSensorMessage>(line.Substring(jsonStartIndex)));
                case "endStroke":
                    return Some((PocoObject)JsonConvert.DeserializeObject<EndStrokeMessage>(line.Substring(jsonStartIndex)));
                case "beginRecovery":
                    return Some((PocoObject)JsonConvert.DeserializeObject<BeginRecoveryMessage>(line.Substring(jsonStartIndex)));
                case "endRecovery":
                    return Some((PocoObject)JsonConvert.DeserializeObject<EndRecoveryMessage>(line.Substring(jsonStartIndex)));
                case "idle":
                    return Some((PocoObject)JsonConvert.DeserializeObject<IdleMessage>(line.Substring(jsonStartIndex)));
                default:
                    return Option<PocoObject>.None;
            }
        }

        public void ParseAndPublish(string line) 
        {
            ParseLine(line)
                .Map(some => EventFactory.CreateEvent(some))
                .Do(e => _pipeline.OnNext(e));
        }
        
    }
}
