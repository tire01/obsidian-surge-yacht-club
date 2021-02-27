using ArduinoLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RowerMoniter.Contracts;
using RowerMoniter.Model;
using RowerMoniter.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDataRecorder
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ArduinoDeviceManager();
            var port = manager.SerialPorts.Values.FirstOrDefault();

            if (port == null)
            {
                Console.WriteLine("No Arduino found.  Is it plugged in?");
                Console.ReadKey();
                return;
            }

            // Arduino reports as 9600 BAUD regardless of what's configured in the Serial library.
            port.BaudRate = 115200;
            try
            {
                port.Open();
            }
            catch (Exception)
            {
                port.Close();
                port.Open();
            }

            EventService svc = EventService.Instance;
            var log = new Log();
            var logger = new WriteStrokesToLog(log);

            using (var moniter = new RowerMoniter.Arduino.PortMoniter(logger.OnNext))
            {
                moniter.StartPort(port);
                
                while (Console.ReadKey().Key != ConsoleKey.Enter)
                    Thread.Sleep(100);
            
            }

            using (var file = File.CreateText(@".\rowerData.json"))
            {
                var serializer = JsonSerializer.Create(new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All  });
                serializer.Serialize(file, log);
            }
        }

        public class WriteStrokesToLog 
        {
            private readonly Log _log;
            private Stopwatch _sw;
            
            public WriteStrokesToLog(Log log) 
            {
                _sw = Stopwatch.StartNew();
                _log = log;
            }
            

            public void OnNext(string value)
            {
                var ellapsed = _sw.Elapsed.Milliseconds;

                var loggedEvent = new LoggedEvent() { Ellapsed = ellapsed, Message = value };
                _log.Events.Add(loggedEvent); 
                    
                _sw.Restart();
            }
        }
     }

}
