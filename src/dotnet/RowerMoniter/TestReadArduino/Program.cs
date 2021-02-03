using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using ArduinoLibrary;
using System.Linq;
using RowerMoniter.Services;
using RowerMoniter.Model;
using System.Reactive.Linq;

namespace TestSerialRead
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
                return;
            }

            // Arduino reports as 9600 BAUD regardless of what's configured in the Serial library.
            port.BaudRate = 115200;
            port.Open();

            EventService svc = EventService.Instance;

            using (var moniter = new RowerMoniter.Arduino.PortMoniter(svc.ParseAndPublish))
            {
                moniter.StartPort(port);
                using (svc.Pipeline.Subscribe(new WriteStrokesToConsole()))
                {
                    while (Console.ReadKey().Key != ConsoleKey.Enter)
                        Thread.Sleep(100);
                }
            }
        }

        public class WriteStrokesToConsole : IObserver<IRowEvent>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(IRowEvent value)
            {
                Console.WriteLine(value.ToString());
            }
        }

    }
}
