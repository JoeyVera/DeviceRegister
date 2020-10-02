using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DeviceRegister.Models
{
    public class WaterMeter : Device
    {
        public WaterMeter(string serialNumber, string brand, string model) : base(serialNumber, brand, model) {}

        public override async Task<ActionResult<bool>> AlreadyExist(DevicesContext dbContext)
        {
            WaterMeter device = null;
            device = await dbContext.WaterMeter.FirstOrDefaultAsync(x => x.SerialNumber == this.SerialNumber);
            if (device == null)
                return false;
            else
                return true;
        }

        public override async Task<ActionResult<Device>> SaveDeviceInDB(DevicesContext dbContext)
        {
            try
            {
                dbContext.WaterMeter.Add(this);
                await dbContext.SaveChangesAsync();
                return this;
            }
            catch (Exception err)
            {
                //todo
                return null;
            }
        }
    }
}
