using DeviceRegister.Models;
using NServiceBus;

namespace DeviceRegisterNServiceBus
{
    public class AddDevice :
        ICommand
    {
        public Device device { get; set; }
    }
}



