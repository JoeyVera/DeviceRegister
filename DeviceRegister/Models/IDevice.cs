using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceRegister.Models
{
    public class IDevice
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        [NotMapped]
        public string Type { get; set; } // this property is used when receiving device instances from external sources.      
        public string Ip { get; set; }
        public int? Port { get; set; }
    }
}
