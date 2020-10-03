using System.Threading.Tasks;
using DeviceRegister.Models;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.AspNetCore.Mvc;

namespace DeviceRegister
{

    public class NServiceBusHandler :
        IHandleMessages<AddDevice>
    {
        static ILog log = LogManager.GetLogger<NServiceBusHandler>();


        public async Task Handle(AddDevice message, IMessageHandlerContext context)
        {
            var _context = new DevicesContext();

            //I need to perform the following conversion because of: System.InvalidCastException: Unable to cast object of type 'DeviceRegister.Models.Device' to type 'DeviceRegister.Models.EnergyMeter'.
            Device device = message.device;

            //After unwrap the device from the message, it needs to be casted to a specific type.
            string checkIfDeviceUndefined = device.GetType().ToString();
            if (checkIfDeviceUndefined == "DeviceRegister.Models.Device")
                device = DefinedDeviceFactory.MakeSpecific(device);

            //Check if there is already a device of a specific type with that serial number
            ActionResult<bool> actionResult = await device.AlreadyExist(_context);
            bool exists = actionResult.Value;

            if (!exists) //if the device was not found on the previous section, then create it.
            {
                await device.SaveDeviceInDB(_context);
            }
        }
    }


}


