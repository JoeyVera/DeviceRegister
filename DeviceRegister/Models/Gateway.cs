using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DeviceRegister.Models
{
    public class Gateway : Device
    {
        public Gateway(string serialNumber, string brand , string model , string ip = "", int? port = null) : base(serialNumber, brand, model)
        {
            Ip = ip;
            Port = port;
        }

        public override async Task<ActionResult<bool>> AlreadyExist(DevicesContext dbContext)
        {
            Gateway device = null;
            device = await dbContext.Gateway.FirstOrDefaultAsync(x => x.SerialNumber == this.SerialNumber);
            if (device == null)
                return false;
            else
                return true;
        }

        public override async Task<ActionResult<Device>> SaveDeviceInDB(DevicesContext dbContext)
        {
                dbContext.Gateway.Add(this);
                await dbContext.SaveChangesAsync();
                return this;
        }
    }
}
