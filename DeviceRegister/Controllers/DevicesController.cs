using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeviceRegister.Models;
using Newtonsoft.Json;

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

        // GET api/Devices
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var EnergyMeterList = new List<EnergyMeter>();
            var WaterMeterList = new List<WaterMeter>();
            var GatewayList = new List<Gateway>();
            var deviceList = new List<IDevice>();

            EnergyMeterList =  _context.EnergyMeter.ToList();
            WaterMeterList =  _context.WaterMeter.ToList();
            GatewayList =  _context.Gateway.ToList();

            deviceList = EnergyMeterList.ConvertAll(x => (IDevice)x).Concat(WaterMeterList.ConvertAll(x => (IDevice)x)).Concat(GatewayList.ConvertAll(x => (IDevice)x)).ToList();

            return Ok(JsonConvert.SerializeObject(deviceList));

        }
        
        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IDevice>>> GetDevice()
        {

            var EnergyMeterList = new List<EnergyMeter>();
            var WaterMeterList = new List<WaterMeter>();
            var GatewayList = new List<Gateway>();
            var deviceList = new List<IDevice>();

            EnergyMeterList = await _context.EnergyMeter.ToListAsync();
            WaterMeterList = await _context.WaterMeter.ToListAsync();
            GatewayList = await _context.Gateway.ToListAsync();

            deviceList = EnergyMeterList.ConvertAll(x => (IDevice)x).Concat(WaterMeterList.ConvertAll(x => (IDevice)x)).Concat(GatewayList.ConvertAll(x => (IDevice)x)).ToList();

            return deviceList;
        }
        
        // GET: api/Devices/5       
        [HttpGet("{id}")]
        public async Task<ActionResult<IDevice>> GetDevice(Guid id)
        {

            IDevice device = await _context.EnergyMeter.FindAsync(id);

            if (device != null)            
                device = await _context.EnergyMeter.FindAsync(id);            
            else
            {
                device = await _context.WaterMeter.FindAsync(id);
                if (device != null)                
                    device = await _context.WaterMeter.FindAsync(id);                
                else
                {
                    device = await _context.Gateway.FindAsync(id);
                    if (device != null)                    
                        device = await _context.Gateway.FindAsync(id);                    
                    else                    
                        return NotFound();                    
                }
            }

            return Ok(device);
        }

        public async Task<ActionResult<IDevice>> GetDeviceBySerialNumber(string serialNumber, string type)
        {


            IDevice _device = null;

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
        public async Task<ActionResult<IDevice>> PostDevice(IDevice device)
        {
            IDevice _device = null;
            string externalType = device.Type;
            string type;

            switch (externalType) //when receiving devices for external source - i.e. web POST - the type property must be checked to get the TypeOf of the object device
            {
                case "WaterMeter":
                    type = "DeviceRegister.Models.WaterMeter"; break;

                case "EnergyMeter":
                    type = "DeviceRegister.Models.EnergyMeter"; break;

                case "Gateway":
                    type = "DeviceRegister.Models.Gateway"; break;

                default:
                    type = device.GetType().ToString(); break;
            }
            try
            {
                switch (type)  //Check if there is already a device of a specific type with that serial number
                {
                    case "DeviceRegister.Models.WaterMeter":
                        _device = await _context.WaterMeter.FirstOrDefaultAsync(x => x.SerialNumber == device.SerialNumber); break;

                    case "DeviceRegister.Models.EnergyMeter":
                        _device = await _context.EnergyMeter.FirstOrDefaultAsync(x => x.SerialNumber == device.SerialNumber); break;

                    case "DeviceRegister.Models.Gateway":
                        _device = await _context.Gateway.FirstOrDefaultAsync(x => x.SerialNumber == device.SerialNumber); break;
                }


                if (_device == null) //if the device was not found on the previous section, then create it.
                {
                    switch (type)
                    {

                        case "DeviceRegister.Models.WaterMeter":
                            _device = new WaterMeter(device.SerialNumber, device.Brand, device.Model);
                            _context.WaterMeter.Add((WaterMeter)_device);
                            break;

                        case "DeviceRegister.Models.EnergyMeter":
                            _device = new EnergyMeter(device.SerialNumber, device.Brand, device.Model);
                            _context.EnergyMeter.Add((EnergyMeter)_device);
                            break;

                        case "DeviceRegister.Models.Gateway":
                            _device = new Gateway(device.SerialNumber, device.Brand, device.Model, device.Ip, device.Port);
                            _context.Gateway.Add((Gateway)_device);
                            break;
                    }

                    await _context.SaveChangesAsync();

                }
                else //return error if the device already exists in the DB
                {
                    ModelState.AddModelError(_device.SerialNumber, "The serial number already exists!");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

            return Ok(_device);
        }
                
        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IDevice>> DeleteDevice(Guid id)
        {

            try { 

                IDevice device = await _context.EnergyMeter.FindAsync(id);
                await _context.SaveChangesAsync();

                if (device != null)
                {
                    _context.EnergyMeter.Remove((EnergyMeter)device);
                }
                else
                {
                    device = await _context.WaterMeter.FindAsync(id);
                    if (device != null)
                    {
                        _context.WaterMeter.Remove((WaterMeter)device);
                    }
                    else
                    {
                        device = await _context.Gateway.FindAsync(id);
                        if (device != null)
                        {
                            _context.Gateway.Remove((Gateway)device);
                        }
                        else
                        {
                            return NotFound();
                        }
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
