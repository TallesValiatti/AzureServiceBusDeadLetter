using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace azureservicebusdeadletter.shared.Integration
{
    public static class IntegrationBusConfigurationExtensions
    {
        public static void AddIntegrationBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(builder =>
            {
            var serviceBusConnectionString = configuration.GetSection("ServiceBusConnectionString").Value;
            var serviceBusNamespace = configuration.GetSection("ServiceBusNamespace").Value;

            builder.AddServiceBusClient(serviceBusConnectionString)
                   .WithName(serviceBusNamespace);
            });

            services.AddSingleton<IPaymentIntegrationBus, PaymentIntegrationBus>();
        }
    }
}