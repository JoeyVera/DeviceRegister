using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace DeviceRegister.Models
{
    public class EnergyMeter : Device
    {
        public EnergyMeter(string serialNumber, string brand, string model) : base (serialNumber,  brand,  model) {}

        public override async Task<ActionResult<bool>> AlreadyExist(DevicesContext dbContext)
        {
            EnergyMeter device = null;
            device = await dbContext.EnergyMeter.FirstOrDefaultAsync(x => x.SerialNumber == this.SerialNumber);
            if (device == null)
                return false;
            else
                return true;
        }

        public override async Task<ActionResult<Device>> SaveDeviceInDB(DevicesContext dbContext)
        {
            try
            {
                dbContext.EnergyMeter.Add(this);
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
