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
    [Serializable]
    public sealed class IdleMessage : Poco
    {
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "log")]
    [Serializable]
    public class Log : Poco
    {
        public List<LoggedEvent> Events { get; } = new List<LoggedEvent>();
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "logEvent")]
    [Serializable]
    public class LoggedEvent : Poco
    {
        public int Ellapsed { get; set; }
        public string Message { get; set; }
    }
}
