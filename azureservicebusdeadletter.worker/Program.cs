using azureservicebusdeadletter.shared.CorrelationId;
using azureservicebusdeadletter.shared.Integration;
using azureservicebusdeadletter.worker;
using azureservicebusdeadletter.worker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddIntegrationBus(hostContext.Configuration);
        services.AddCorrelationId();

        services.AddScoped<IPaymentService, PaymentService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
