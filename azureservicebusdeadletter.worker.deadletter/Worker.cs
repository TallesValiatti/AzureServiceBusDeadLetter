using azureservicebusdeadletter.shared.Integration;

namespace azureservicebusdeadletter.worker.deadletter;

public class Worker : BackgroundService
{
    private readonly IPaymentIntegrationBus _paymentIntegrationBus;

    public Worker(IPaymentIntegrationBus paymentIntegrationBus)
    {
        _paymentIntegrationBus = paymentIntegrationBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _paymentIntegrationBus.StartReceiveDeadLetterIntegrationEvents();
    }
}
