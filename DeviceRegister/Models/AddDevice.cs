using DeviceRegister.Models;
using NServiceBus;

namespace DeviceRegister.Models
{
    public class AddDevice :
        ICommand
    {
        public Device device { get; set; }
    }
}



