/* Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RowerMoniter.Contracts;
using RowerMoniter.Services;
using RowerMoniter.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using LanguageExt;

namespace RowerMoniterTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void SerializeItems()
        {
            var beginStroke = new BeginStrokeMessage() { Count = 1 };
            var endRecovery = new EndRecoveryMessage() { Duration = 2000, Length = 1 };

            var flywheel = new FlywheelSensorMessage() { RadiansPerSecond = 1.1073647484M };
            var endStroke = new EndStrokeMessage() { Duration = 1000, Length = 1 };
            var beginRecovery = new BeginRecoveryMessage() { };

            string serialized = null;

            serialized = JsonConvert.SerializeObject(beginStroke);
            Console.WriteLine($"beginStroke:{serialized}");

            serialized = JsonConvert.SerializeObject(endStroke);
            Console.WriteLine($"endStroke:{serialized}");

            serialized = JsonConvert.SerializeObject(beginRecovery);
            Console.WriteLine($"beginRecovery:{serialized}");

            serialized = JsonConvert.SerializeObject(endRecovery);
            Console.WriteLine($"endRecovery:{serialized}");

            serialized = JsonConvert.SerializeObject(flywheel);
            Console.WriteLine($"flywheel:{serialized}");
        }


        //  beginStroke:{"count":1}
        //  flywheel: { "rps":1.1073647484}
        //  endStroke: { "length":1,"duration":1000}
        //  beginRecovery: { }
        //  endRecovery: { "length":1,"duration":2000}
        //  idle: { }




        [TestMethod]
        public void DeserializeItems()
        {
            string testData =
              "beginStroke:{\"count\":1}\r\n" +
              "update:{\"rps\":1.1073647484}\r\n" +
              "endStroke:{\"length\":1,\"duration\":1000}\r\n" +
              "beginRecovery:{}\r\n" +
              "endRecovery:{\"length\":1,\"duration\":2000}\r\n" +
              "idle:{}\r\n";
            
            var lines = testData.Split(new[] { '\r','\n' });
            var messages = new List<Option<PocoObject>>();
            
            foreach (var line in lines) 
            {
                //messages.Add(SerialMoniter.ParseLine(line));
            }

            var events = messages
                .Bind(o => o)
                .Map(EventFactory.CreateEvent);
        }

    }
}*/
