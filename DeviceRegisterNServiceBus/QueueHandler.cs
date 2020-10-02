
using System.Threading.Tasks;
using DeviceRegister.Models;
using NServiceBus;
using NServiceBus.Logging;


namespace DeviceRegisterNServiceBus
{
    class QueueHandler :  IHandleMessages<AddDevice>
    {
        static ILog log = LogManager.GetLogger<QueueHandler>();

        public Task Handle(AddDevice message, IMessageHandlerContext context)
        {
            log.Info($"Destination has received a new {message.device.Type}, S/N = {message.device.SerialNumber}");


            var _context = new DevicesContext();


            //I need to perform the following conversion because of: System.InvalidCastException: Unable to cast object of type 'DeviceRegister.Models.IDevice' to type 'DeviceRegister.Models.EnergyMeter'.
            Device receivedDevice = message.device;

            switch (message.device.Type)
            {

                case "WaterMeter":

                    WaterMeter waterMeter = new WaterMeter(receivedDevice.SerialNumber, receivedDevice.Brand, receivedDevice.Model);
                    _context.WaterMeter.Add(waterMeter);

                    break;

                case "EnergyMeter":

                    EnergyMeter energyMeter = new EnergyMeter(receivedDevice.SerialNumber, receivedDevice.Brand, receivedDevice.Model);
                    _context.EnergyMeter.Add(energyMeter);

                    break;

                case "Gateway":

                    Gateway gateway = new Gateway(receivedDevice.SerialNumber, receivedDevice.Brand, receivedDevice.Model);
                    _context.Gateway.Add(gateway);

                    break;
            }

            _context.SaveChangesAsync();

            log.Info($"{message.device.Type}, S/N = {message.device.SerialNumber} sent to the DB.");

            return Task.CompletedTask;
        }
    }

}

