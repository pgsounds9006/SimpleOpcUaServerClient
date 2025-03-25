using Opc.Ua;
using Opc.Ua.Server;
using Opc.Ua.Configuration;
using Server;

var application = new ApplicationInstance();
application.ApplicationType = ApplicationType.Server;
application.ConfigSectionName = "OpcUaServer";
await application.LoadApplicationConfiguration(false);

// check the application certificate.
await application.CheckApplicationInstanceCertificates(false);

// start the server.

var server = new OpcUaServer();

await application.Start(server);

var endpointUrls = server.GetEndpoints().Select(o => o.EndpointUrl).Distinct();

foreach (var url in endpointUrls)
{
    Console.WriteLine(url);
}
Console.ReadLine();