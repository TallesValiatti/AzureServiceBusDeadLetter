using azureservicebusdeadletter.shared.CorrelationId;
using azureservicebusdeadletter.shared.Integration;
using azureservicebusdeadletter.worker.Services;

namespace azureservicebusdeadletter.worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IPaymentIntegrationBus _paymentIntegrationBus;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger,
                  IPaymentIntegrationBus paymentIntegrationBus, 
                  IServiceProvider serviceProvider)
    {
        _logger = logger;
        _paymentIntegrationBus = paymentIntegrationBus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _paymentIntegrationBus.StartReceiveIntegrationEvents((@event, attempt) => 
        {
            var scope = _serviceProvider.CreateScope();
            var paymentService = scope.ServiceProvider.GetService<IPaymentService>();
            var correlationId = scope.ServiceProvider.GetService<ICorrelationId>();

            correlationId!.Set(@event.CorrelationId);
            
            this.LogInformation($"Receiving event", correlationId.Get(), attempt);
            
            paymentService!.ProcessPayment(@event.PaymentId);

            this.LogInformation($"Event received", correlationId.Get(), attempt);
            return Task.CompletedTask;
        });
    }   

    private void LogInformation(string message, Guid correlationId, int attempt)
    {
        _logger.LogInformation($"{correlationId.ToString()} - {message} - attempt {attempt} - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")}");
    }
}
