using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Newtonsoft.Json;
using RowerMoniter.Json;

namespace RowerMoniter.Moniter
{
    public class Router
    {
        //  beginStroke:{"count":1}
        //  update: { "rps":1.1073647484}
        //  endStroke: { "length":1,"duration":1000}
        //  beginRecovery: { }
        //  endRecovery: { "length":1,"duration":2000}
        //  idle: { }

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
                    return Some((PocoObject)JsonConvert.DeserializeObject<UpdateMessage>(line.Substring(jsonStartIndex)));
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

    }
}
