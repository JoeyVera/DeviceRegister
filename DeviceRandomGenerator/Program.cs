using System;
using NServiceBus;
using System.Timers;
using System.Threading.Tasks;
using DeviceRegister.Models;
using System.Net.Http;
using System.Threading;

namespace DeviceRandomGenerator
{
    class Program
    {
        static System.Timers.Timer TTimer;
        static ConsoleColor defaultC = Console.ForegroundColor;
        static IEndpointInstance _endpointInstance;

        public static async Task Main()
        {
            Console.Title = "Random Device Generator";

            var endpointConfiguration = new EndpointConfiguration("RandomGenerator");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var routerConfig = transport.Routing();
            routerConfig.RouteToEndpoint(
                assembly: typeof(AddDevice).Assembly,
                //destination: "DeviceRegisterNServiceBus"); //change swap this line for the one above to change the receiver project
                destination: "DeviceRegister");            

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

            var metrics = endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

            _endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

#if (DEBUG)
            // IIS can be configure to fully start the app but IIS Express doesn't do it until receives a http request.
            // This subrutine allows DeviceRandomGenerator to WakeUp the serve in debug mode.

            Thread.Sleep(2000); // Give the backend some time to start

            using (var client = new HttpClient())
            {
                Guid item = new Guid();
                var result = await client.GetAsync("http://localhost:26647/api/Devices/" + item.ToString());                
            }

            Thread.Sleep(2000); // Give the backend some time wakeup
#endif

            TTimer = new System.Timers.Timer(10000);
            TTimer.Elapsed += new ElapsedEventHandler(MakeUpSomeDevice);
            TTimer.Start();         

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await _endpointInstance.Stop().ConfigureAwait(false);
        }

        static void MakeUpSomeDevice(object sender, ElapsedEventArgs e)
        {
            Device _device = null;
            Random rnd = new Random();
            int intType = rnd.Next(1, 4);

            string serialNumber = rnd.Next(1, 9000000).ToString();
            string brand = "MyBrand" + rnd.Next(1, 16).ToString();
            string model = "MyModel" + rnd.Next(1, 33).ToString();

            switch (intType)
            {
                case 1:
                    _device = new EnergyMeter(serialNumber, brand, model);
                    //This is required because when unwrapping the device out the message, the Subtype is lost.
                    _device.Type = "EnergyMeter"; 
                    break;
                case 2:
                    _device = new WaterMeter(serialNumber, brand, model);
                    _device.Type = "WaterMeter";
                    break;
                case 3:
                    _device = new Gateway(serialNumber, brand, model);
                    _device.Type = "Gateway";
                    break;
            }

            var addDevice = new AddDevice
            {
                device = _device
            };

            // Send the command
            _endpointInstance.Send(addDevice).ConfigureAwait(false);

            Console.WriteLine("Random " + _device.Type + " - Id: " + _device.Id.ToString() + "; serial number: " + _device.SerialNumber + " - created and sent to the queue for addition to the database");

        }
    }
}