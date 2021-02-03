using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Contracts
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "update")]
    public sealed class FlywheelSensorMessage : PocoObject
    {
        [JsonProperty(PropertyName ="rps")]
        public decimal RadiansPerSecond {get; set;}
    }
}
