namespace DeviceRegister.Models
{
    public class Gateway : Device
    {
        public Gateway(string serialNumber, string brand , string model , string ip = "", int? port = null) : base(serialNumber, brand, model)
        {

            Ip = ip;
            Port = port;

        }
    }
}
