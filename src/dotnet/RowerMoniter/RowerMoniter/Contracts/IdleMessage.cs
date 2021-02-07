using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RowerMoniter.Contracts
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "idle")]

    public sealed class IdleMessage : Poco
    {
    }


    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "log")]

    public class Log
    {
        public List<LoggedEvent> Events { get; } = new List<LoggedEvent>();
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "logEvent")]

    public class LoggedEvent
    {
        public int Ellapsed { get; set; }
        public string Message { get; set; }
    }
}
