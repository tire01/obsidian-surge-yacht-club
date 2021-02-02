using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using ArduinoLibrary;
using System.Linq;

namespace TestSerialRead
{
    class Program
    {
        static SerialPort _serialPort;

        static void Main(string[] args)
        {
            var manager = new ArduinoDeviceManager();

            var port = manager.SerialPorts.Values.FirstOrDefault();

            if (port == null)
            {
                Console.WriteLine("No Arduino found.  Is it plugged in?");
                return;
            }

            port.BaudRate = 115200;
            port.Open();

            using (var moniter = new RowerMoniter.Arduino.PortMoniter())
            {
                moniter.StartPort(port);

                while (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Thread.Sleep(100);
                }
            }
        }




        public static Task<string> ReadSerialAsync() 
        {
            return Task.FromResult("");
        }
    }
}
