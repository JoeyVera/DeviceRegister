using DeviceRegister.Models;
using NServiceBus;

namespace DeviceRegisterNServiceBus
{
    public class AddDevice :
        ICommand
    {
        public IDevice device { get; set; }
    }
}



