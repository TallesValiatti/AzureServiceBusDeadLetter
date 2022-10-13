using azureservicebusdeadletter.shared.CorrelationId;
using azureservicebusdeadletter.shared.Integration;
using azureservicebusdeadletter.shared.Messages;
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
        await _paymentIntegrationBus.StartReceiveIntegrationMessages((message, attempt) => 
        {
            var scope = _serviceProvider.CreateScope();
            var paymentService = scope.ServiceProvider.GetService<IPaymentService>();
            var correlationId = scope.ServiceProvider.GetService<ICorrelationId>();

            correlationId!.Set(message.CorrelationId);
            
            _logger.LogInformation($" Correlation Id: {correlationId!.Get()} - Receiving message - attempt {attempt}"); 

            paymentService!.ProcessPayment(message.PaymentId);

            return Task.CompletedTask;
        });
    }   
}
