using System;

namespace DeviceRegister.Models
{
    public abstract class Device : IDevice
    {
        public Device(string serialNumber, string brand, string model)
        {
            if (string.IsNullOrEmpty(serialNumber.Trim(' ')))
                throw new ArgumentNullException(nameof(serialNumber));

            Id = Guid.NewGuid();
            Brand = brand;
            Model = model;
            SerialNumber = serialNumber;               
        }
    }
}
