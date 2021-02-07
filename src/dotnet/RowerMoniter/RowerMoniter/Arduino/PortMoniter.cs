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
        private readonly int _expectedMaxMessageLength;

        private byte[] _buffer = new byte[1024];
        
        private StringBuilder sb;
        private SerialPort _port;

        public PortMoniter(Action<string> lineRead, int expectedMaMessageLength = 64) 
        {
            _expectedMaxMessageLength = expectedMaMessageLength;
            _lineRead = lineRead;
        }

        public void StartPort(SerialPort port) 
        {
            sb = new StringBuilder(_expectedMaxMessageLength);
            _port = port;
            port.DiscardInBuffer();
            port.DataReceived += DataReceivedHandler;
        }

        private readonly Action<string> _lineRead;

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // Not the most efficient IO here but whatever it works.
            SerialPort sp = (SerialPort)sender;
            int bytesRead = sp.Read(_buffer, 0, sp.BytesToRead);

            byte[] c = new byte[1];

            for (int i = 0; i < bytesRead; i++)
            {
                c[0] = _buffer[i];

                switch (c[0])
                {
                    case 13: // CR
                        {
                            var line = sb.ToString();
                            Task.Run(() => _lineRead(line));
                            sb = new StringBuilder(_expectedMaxMessageLength);
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

        public void Dispose()
        {
            _port.DataReceived -= DataReceivedHandler;
        }

    }
}
