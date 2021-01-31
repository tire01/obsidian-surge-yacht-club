using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RowerMoniter.Json;
using System;

namespace RowerMoniterTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void SerializeItems()
        {
            var beginStroke = new BeginStroke() { Count = 1 };
            var endRecovery = new EndRevocery() { Duration = 2000, Length = 1 };

            var update = new Update() { RadiansPerSecond = 1.1073647484M };
            var endStroke = new EndStroke() { Duration = 1000, Length = 1 };
            var beginRecovery = new BeginRecovery() { };

            string serialized = null;

            serialized = JsonConvert.SerializeObject(beginStroke);
            Console.WriteLine($"beginStroke:{serialized}");
            
            serialized = JsonConvert.SerializeObject(beginStroke);
            Console.WriteLine($"beginStroke:{serialized}");

            serialized = JsonConvert.SerializeObject(beginStroke);
            Console.WriteLine($"beginStroke:{serialized}");

            serialized = JsonConvert.SerializeObject(beginStroke);
            Console.WriteLine($"beginStroke:{serialized}");

            serialized = JsonConvert.SerializeObject(beginStroke);
            Console.WriteLine($"beginStroke:{serialized}");
        }
    }

    //  beginStroke:{"count":1}
    //  update: { "rps":1.1073647484}
    //  endStroke: { "length":1,"duration":1000}
    //  beginRecovery: { }
    //  endRecovery: { "length":1,"duration":2000}
    //  idle: { }

    [TestMethod]
    public void DeserializeItems() 
    {
        
    }
}
