using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeviceRegister.Models;

namespace DeviceRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesContext _context;

        public DevicesController(DevicesContext context)
        {
            _context = context;
        }
  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            var EnergyMeterList = new List<EnergyMeter>();
            var WaterMeterList = new List<WaterMeter>();
            var GatewayList = new List<Gateway>();
            var deviceList = new List<Device>();

            EnergyMeterList = await _context.EnergyMeter.ToListAsync();
            WaterMeterList = await _context.WaterMeter.ToListAsync();
            GatewayList = await _context.Gateway.ToListAsync();

            deviceList = EnergyMeterList.ConvertAll(x => (Device)x).Concat(WaterMeterList.ConvertAll(x => (Device)x)).Concat(GatewayList.ConvertAll(x => (Device)x)).ToList();

            return deviceList;
        }
        
        // GET: api/Devices/5       
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(Guid id)
        {
            Device device = await _context.EnergyMeter.FindAsync(id);

            if(device == null)
            {
                device = await _context.WaterMeter.FindAsync(id);
                if (device == null)
                {
                    device = await _context.Gateway.FindAsync(id);
                    if (device == null)
                        return NotFound();
                }
            }
            return Ok(device);
        }

        public async Task<ActionResult<Device>> GetDeviceBySerialNumber(string serialNumber, string type)
        {
            Device _device = null;

            try
            {
                switch (type)
                {
                    case "DeviceRegister.Models.WaterMeter":
                        _device = await _context.WaterMeter.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber); break;

                    case "DeviceRegister.Models.EnergyMeter":
                        _device = await _context.EnergyMeter.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber); break;

                    case "DeviceRegister.Models.Gateway":
                        _device = await _context.Gateway.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber); break;
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

            return Ok(_device);
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            try
            {
                //If is coming from an external source as Base type is casted to a specific type
                string checkIfDeviceUndefined = device.GetType().ToString();
                if (checkIfDeviceUndefined == "DeviceRegister.Models.Device")
                    device = DefinedDeviceFactory.MakeSpecific(device);

                //Check if there is already a device of a specific type with that serial number
                var actionResult = await device.AlreadyExist(_context);
                bool exists = actionResult.Value;
               
                if (!exists) //if the device was not found on the previous section, then create it.
                {
                    await device.SaveDeviceInDB(_context);
                }
                else //return error if the device already exists in the DB
                {
                    ModelState.AddModelError(device.SerialNumber, "The serial number already exists!");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

            return Ok(device);

        }                       

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Device>> DeleteDevice(Guid id)
        {      
            try {
                Device device = null;
                device = await _context.EnergyMeter.FindAsync(id);
                await _context.SaveChangesAsync();

                if (device != null)
                    _context.EnergyMeter.Remove((EnergyMeter)device);
                else
                {
                    device = await _context.WaterMeter.FindAsync(id);
                    if (device != null)
                        _context.WaterMeter.Remove((WaterMeter)device);
                    else
                    {
                        device = await _context.Gateway.FindAsync(id);
                        if (device != null)
                            _context.Gateway.Remove((Gateway)device);
                        else
                            return NotFound();
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(device);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
