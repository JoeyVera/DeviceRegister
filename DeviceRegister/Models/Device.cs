using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceRegister.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        [NotMapped]
        public string Type { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }

        public Device(string serialNumber, string brand, string model)
        {
            if (string.IsNullOrEmpty(serialNumber.Trim(' ')))
                throw new ArgumentNullException(nameof(serialNumber));

            Id = Guid.NewGuid();
            Brand = brand;
            Model = model;
            SerialNumber = serialNumber;
        }
        public virtual Task<ActionResult<bool>> AlreadyExist(DevicesContext dbContext) { return null; }
        public virtual Task<ActionResult<Device>> SaveDeviceInDB(DevicesContext dbContext) { return null; }
    }

    //Converts a IDevice coming from an external source - i.e. http post - into an expecific type
    public static class UndefinedDeviceFactory {
        public static Device MakeSpecific(Device device)
        {
            switch (device.Type) //when receiving devices for external source - i.e. web POST - the type property must be checked to get the TypeOf of the object device
            {
                case "WaterMeter":
                    return new WaterMeter(device.SerialNumber, device.Brand, device.Model);
                case "EnergyMeter":
                    return new EnergyMeter(device.SerialNumber, device.Brand, device.Model);
                case "Gateway":
                    return new Gateway(device.SerialNumber, device.Brand, device.Model, device.Ip, device.Port);

                default:
                    throw new InvalidCastException(device.Type);
            }

        }
    }
        
}
