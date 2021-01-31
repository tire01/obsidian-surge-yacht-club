using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RowerMoniter.Json
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title ="beginStroke")]

    public sealed class BeginStrokeMessage : PocoObject
    {
        public int Count { get; set; }
    }
}
