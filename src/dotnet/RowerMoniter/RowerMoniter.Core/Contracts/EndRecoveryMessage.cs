using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Contracts
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "endRecovery")]
    [Serializable]
    public sealed class EndRecoveryMessage : Poco
    {
        public int Length { get; set; }
        public int Duration { get; set; }
    }
}
