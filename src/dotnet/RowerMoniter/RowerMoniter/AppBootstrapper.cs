using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoLibrary;
using Caliburn.Micro;
using RowerMoniter.Arduino;
using RowerMoniter.Services;
using RowerMoniter.ViewModels;

namespace RowerMoniter
{
    public class AppBootstrapper : BootstrapperBase
    { 
        private PortMoniter _moniter;
        private ArduinoDeviceManager _manager;

        public AppBootstrapper()
        {
            Initialize();

            _manager = new ArduinoDeviceManager();

        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {

            var port = _manager.SerialPorts.Values.FirstOrDefault();

            if (port == null)
            {
                Console.WriteLine("No Arduino found.  Is it plugged in?");
                return;
            }

            // Arduino reports as 9600 BAUD regardless of what's configured in the Serial library.
            port.BaudRate = 115200;
            port.Open();

            _moniter = new PortMoniter(EventService.Instance.ParseAndPublish);

            _moniter.StartPort(port);

            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
            _moniter.Dispose();
        }
    }
}
