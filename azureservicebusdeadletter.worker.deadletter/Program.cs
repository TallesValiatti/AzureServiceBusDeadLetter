using azureservicebusdeadletter.shared.Integration;
using azureservicebusdeadletter.worker.deadletter;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddIntegrationBus(hostContext.Configuration);
        
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
