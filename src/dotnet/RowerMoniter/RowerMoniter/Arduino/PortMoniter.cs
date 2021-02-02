using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Arduino
{
    public class PortMoniter  : IDisposable
    {
        byte[] _buffer = new byte[1024];

        StringBuilder sb = new StringBuilder();

        SerialPort _port;

        public void StartPort(SerialPort port) 
        {
            _port = port;
            port.DataReceived += DataReceivedHandler;
        }

        private object _lockSerialPortRead = new object();

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_lockSerialPortRead)
            {
                SerialPort sp = (SerialPort)sender;
                int bytesRead = sp.Read(_buffer, 0, sp.BytesToRead);

                byte[] c = new byte[1];

                // Not the most efficient IO here, but it seems ot be able to keep up with 2000 lines per second.
                for (int i = 0; i < bytesRead; i++)
                {
                    c[0] = _buffer[i];

                    switch (c[0])
                    {
                        case 13: // CR
                            {
                                var line = sb.ToString();
                                Task.Run(() => LineReceived(line));
                                sb = new StringBuilder();
                            }
                            break;

                        case 10: // LF (ignore)
                            break;

                        default:
                            sb.Append(Encoding.ASCII.GetString(c));
                            break;
                    }

                }

                // Guard against bad input data eating up resources.
                if (sb.Length >= 1024)
                {
                    sb = new StringBuilder();
                }
            }
        }

        public static void LineReceived(string line) 
        {
            Console.WriteLine(line);
        }

        public void Dispose()
        {
            _port.DataReceived -= DataReceivedHandler;
        }

        private string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }

            return null;
        }
    }
}
