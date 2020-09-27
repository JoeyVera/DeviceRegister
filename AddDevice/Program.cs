using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AddDevice
{
    public class Program
    {
        const string url = "http://localhost:26647/api/Devices/";

        public static async Task Main(string[] args)
        {

            //First part of the code is merely build the help response and allocate args

            StringBuilder helpbuilder = new StringBuilder();

            helpbuilder.Append("Registers a device in the database.\n\n");
            helpbuilder.Append("DOTNET ADDDEVICE.DLL -[type] -s:'[serial number]' -b:'[brand]' -m:'[model]' -i:'[ip]' -p:'[port]'\n\n");
            helpbuilder.Append("type      REQUIRED: Specifies the type of device: EnergyMeter: em, WaterMeter: wm, Gateway: gw\n");
            helpbuilder.Append("s:        REQUIRED: Serial number of the device\n");
            helpbuilder.Append("b:        Brand of the device\n");
            helpbuilder.Append("m:        Model of the device\n");
            helpbuilder.Append("i:        Gateway IP address\n");
            helpbuilder.Append("p:        INTEGER NUMBER: Gateway port\n\n");
            helpbuilder.Append("Spaces in names must be filled with underscore, i.e.: brand 'General Electrics' should be imput as 'General_Electrics'\n\n");
            helpbuilder.Append("Example: dotnet adddevice.dll -em -s:'114-b12_23' -b:'General_Electrics'");

            string help = helpbuilder.ToString();

            using (var client = new HttpClient())
            {
                if (args.Length == 0)
                    Console.WriteLine(help);
                else
                {
                    if (args[0] == "/?" || args[0] == "-?" || (args[0] != "-em" && args[0] != "-wm" && args[0] != "-gw"))
                    {
                        Console.WriteLine(help);
                    }
                    else
                    {
                        try
                        {
                            if (args[1].Substring(0, 4) != "-s:'" || args[1].Length < 6)
                            {
                                Console.WriteLine("Serial number is required\n");
                                Console.WriteLine("Example: dotnet adddevice.dll -em -s:'114-b12_23' -b:'General_Electrics'");
                            }
                            else
                            {
                                string serialnumber = "";
                                string brand = "";
                                string model = "";
                                string ip = "";
                                string port = ""; 
                                string type="";

                                switch (args[0])
                                {
                                    case "-em":
                                        type = "EnergyMeter"; break;
                                    case "-wm":
                                        type = "WaterMeter"; break;
                                    case "-gw":
                                        type = "Gateway"; break;
                                        throw new System.InvalidOperationException("The device type parameter is incorrect.");
                                }

                                foreach (string arg in args)
                                {
                                    if (arg.Substring(0, 3) == "-s:")
                                        serialnumber = adjustArg(arg);
                                    if (arg.Substring(0, 3) == "-b:")
                                        brand = adjustArg(arg);
                                    if (arg.Substring(0, 3) == "-m:")
                                        model = adjustArg(arg);
                                    if (arg.Substring(0, 3) == "-i:")
                                        ip = adjustArg(arg);
                                    if (arg.Substring(0, 3) == "-p:")
                                        port = adjustArg(arg);
                                }

                                if (port.Length > 0 && !int.TryParse(port, out int portInt) && type == "Gateway")
                                    throw new System.InvalidOperationException("Port number must be integer.");

                                StringBuilder RequestBuilder = new StringBuilder();
                                RequestBuilder.Append("{'SerialNumber': '");
                                RequestBuilder.Append(serialnumber);
                                RequestBuilder.Append("', 'Brand': '");
                                RequestBuilder.Append(brand);
                                RequestBuilder.Append("', 'Model': '");
                                RequestBuilder.Append(model);
                                RequestBuilder.Append("', 'Ip': '");
                                RequestBuilder.Append(ip);
                                RequestBuilder.Append("', 'Port': '");
                                RequestBuilder.Append(port);
                                RequestBuilder.Append("', 'Type': '");
                                RequestBuilder.Append(type);
                                RequestBuilder.Append("'}");                            


                                // next lines send the POST request to the backend.
                                var data = new StringContent(RequestBuilder.ToString(), Encoding.UTF8, "application/json");
                                var response = await client.PostAsync(url, data);
                                string result = response.Content.ReadAsStringAsync().Result;
                                Console.WriteLine("Device added: " + result);
                            }
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }          
        }

        
        public static string adjustArg(string arg) //unwrap the arg and restore spaces
        {
            return arg.Substring(4, arg.Length - 5).Replace("_", " ");
        }

    }
}
