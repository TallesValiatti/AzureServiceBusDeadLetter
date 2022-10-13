using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace azureservicebusdeadletter.shared.CorrelationId
{
    public static class CorrelationIdGeneratorExtensions
    {
        public static void AddCorrelationId(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationId, CorrelationId>();
        }
    }
}