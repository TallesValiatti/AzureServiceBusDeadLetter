using azureservicebusdeadletter.shared.Events;

namespace azureservicebusdeadletter.shared.Integration
{
    public interface IPaymentIntegrationBus
    {
        Task SendPaymentCreatedAsync(PaymentCreatedIntegrationEvent @event);
        Task StartReceiveIntegrationEvents(Func<PaymentCreatedIntegrationEvent, int, Task> eventHandler);
        Task StartReceiveDeadLetterIntegrationEvents();
    }
}