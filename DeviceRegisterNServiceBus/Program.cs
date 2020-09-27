using System;
using System.Threading.Tasks;
using NServiceBus;

namespace DeviceRegisterNServiceBus
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "DeviceRegisterNServiceBus";

            // Define the endpoint name
            var endpointConfiguration = new EndpointConfiguration("DeviceRegisterNServiceBus");

            // Select the learning (filesystem-based) transport to communicate with other endpoints
            endpointConfiguration.UseTransport<LearningTransport>();

            // Enable monitoring errors, auditing, and heartbeats with the Particular Service Platform tools
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

            // Enable monitoring endpoint performance
            var metrics = endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

            // Start the endpoint
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}