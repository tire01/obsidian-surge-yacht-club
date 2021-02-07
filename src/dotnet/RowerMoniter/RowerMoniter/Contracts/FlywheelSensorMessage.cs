using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Contracts
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "flywheer")]
    public sealed class FlywheelSensorMessage : Poco
    {
        [JsonProperty(PropertyName ="rps")]
        public decimal RadiansPerSecond {get; set;}

        [JsonProperty(PropertyName = "ellapsed")]
        public int Ellapsed { get; set; }

    }
}
