using DeviceRegister;
using DeviceRegister.Controllers;
using DeviceRegister.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeviceRegisterUnitTest
{

    [TestClass]
    public class UnitTestController
    {

        // These Unit tests are using the actual DB to test the controller behaviour.
        // For a unit test it's preferible to use mock db in memory but I am taking advantage
        // here to perform some integration test with the DB at the same time.

        public static HttpClient TestHttpClient { get; private set; }
        public static TestServer testServer { get; private set; }
        public static HttpContext context { get; private set; }
        public static DevicesController controller;

        [ClassInitialize] //SetUp Context and Controller
        public static void InitializeTestServer(TestContext context)
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            TestHttpClient = testServer.CreateClient();
            var DBcontext = testServer.Host.Services.GetRequiredService<DevicesContext>();
            controller = new DevicesController(DBcontext);
        }

        public static async Task Cleanup() //Clean up tasks
        {
            IDevice device; Guid item;

            var actionResult = await controller.GetDeviceBySerialNumber("DUMMYTESTSN", "DeviceRegister.Models.EnergyMeter");
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            device = (IDevice)okor.Value;
            if (device != null)
            {
                item = device.Id;
                actionResult = await controller.DeleteDevice(item);
            }
            actionResult = await controller.GetDeviceBySerialNumber("DUMMYTESTSN", "DeviceRegister.Models.WaterMeter");
            okor = actionResult.Result as OkObjectResult;
            device = (IDevice)okor.Value;
            if (device != null)
            {
                item = device.Id;
                actionResult = await controller.DeleteDevice(item);
            }
            actionResult = await controller.GetDeviceBySerialNumber("DUMMYTESTSN", "DeviceRegister.Models.Gateway");
            okor = actionResult.Result as OkObjectResult;
            device = (IDevice)okor.Value;
            if (device != null)
            {
                item = device.Id;
                actionResult = await controller.DeleteDevice(item);
            }
        }

        [TestInitialize()] //Clean dummies before starting a task
        public async Task Initialize()
        {
            await Cleanup();
        }

        [AssemblyCleanup()] //Clean dummies after finalizing all tasks
        public static async Task AssemblyCleanup()
        {
            await Cleanup();
        }

        [TestMethod]
        public async Task Run_01_PostingEnergyMeter()
        {
            IDevice energyMeter = new EnergyMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(energyMeter);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            energyMeter = (IDevice)okor.Value;
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }
       
        [TestMethod]
        public async Task Run_02_PostingDuplicateEnergyMeter()
        {
            IDevice energyMeter = new EnergyMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(energyMeter);
            actionResult = await controller.PostDevice(energyMeter);
            string result = actionResult.Result.ToString();
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.BadRequestObjectResult");
        }
        
        [TestMethod]
        public async Task Run_03_GettingEnergyMeter()
        {
            Guid item;
            IDevice energyMeter = new EnergyMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(energyMeter);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            energyMeter = (IDevice)okor.Value;
            item = energyMeter.Id;
            var result = await controller.GetDevice(item);
            actionResult = await controller.GetDevice(item);
            okor = actionResult.Result as OkObjectResult;
            IDevice returnedMeter = (IDevice)okor.Value;
            Assert.AreEqual(returnedMeter.Id, item);        
        }
       
        [TestMethod]
        public async Task Run_04_DeletingEnergyMeter()
        {
            Guid item;
            IDevice energyMeter = new EnergyMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(energyMeter);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            energyMeter = (IDevice)okor.Value;
            item = energyMeter.Id;
            actionResult = await controller.DeleteDevice(item);
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }



        [TestMethod]
        public async Task Run_05_PostingWaterMeter()
        {
            IDevice WaterMeter = new WaterMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(WaterMeter);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            WaterMeter = (IDevice)okor.Value;
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }

        [TestMethod]
        public async Task Run_06_PostingDuplicateWaterMeter()
        {
            IDevice WaterMeter = new WaterMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(WaterMeter);
            actionResult = await controller.PostDevice(WaterMeter);
            string result = actionResult.Result.ToString();
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.BadRequestObjectResult");
        }

        [TestMethod]
        public async Task Run_07_GettingWaterMeter()
        {
            Guid item;
            IDevice WaterMeter = new WaterMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(WaterMeter);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            WaterMeter = (IDevice)okor.Value;
            item = WaterMeter.Id;
            var result = await controller.GetDevice(item);
            actionResult = await controller.GetDevice(item);
            okor = actionResult.Result as OkObjectResult;
            IDevice returnedMeter = (IDevice)okor.Value;
            Assert.AreEqual(returnedMeter.Id, item);
        }

        [TestMethod]
        public async Task Run_08_DeletingWaterMeter()
        {
            Guid item;
            IDevice WaterMeter = new WaterMeter("DUMMYTESTSN", "MyBrand", "MyModel");
            var actionResult = await controller.PostDevice(WaterMeter);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            WaterMeter = (IDevice)okor.Value;
            item = WaterMeter.Id;
            actionResult = await controller.DeleteDevice(item);
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }

        [TestMethod]
        public async Task Run_09_PostingGateway()
        {
            IDevice Gateway = new Gateway("DUMMYTESTSN", "MyBrand", "MyModel", "0.0.0.0", 8080);
            var actionResult = await controller.PostDevice(Gateway);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            Gateway = (IDevice)okor.Value;
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }

        [TestMethod]
        public async Task Run_10_PostingDuplicateGateway()
        {
            IDevice Gateway = new Gateway("DUMMYTESTSN", "MyBrand", "MyModel", "0.0.0.0", 8080);
            var actionResult = await controller.PostDevice(Gateway);
            actionResult = await controller.PostDevice(Gateway);
            string result = actionResult.Result.ToString();
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.BadRequestObjectResult");
        }

        [TestMethod]
        public async Task Run_11_GettingGateway()
        {
            Guid item;
            IDevice Gateway = new Gateway("DUMMYTESTSN", "MyBrand", "MyModel", "0.0.0.0", 8080);
            var actionResult = await controller.PostDevice(Gateway);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            Gateway = (IDevice)okor.Value;
            item = Gateway.Id;
            var result = await controller.GetDevice(item);
            actionResult = await controller.GetDevice(item);
            okor = actionResult.Result as OkObjectResult;
            IDevice returnedMeter = (IDevice)okor.Value;
            Assert.AreEqual(returnedMeter.Id, item);
        }

        [TestMethod]
        public async Task Run_12_DeletingGateway()
        {
            Guid item;
            IDevice Gateway = new Gateway("DUMMYTESTSN", "MyBrand", "MyModel", "0.0.0.0", 8080);
            var actionResult = await controller.PostDevice(Gateway);
            OkObjectResult okor = actionResult.Result as OkObjectResult;
            Gateway = (IDevice)okor.Value;
            item = Gateway.Id;
            actionResult = await controller.DeleteDevice(item);
            Assert.AreEqual(actionResult.Result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }

    }

}
