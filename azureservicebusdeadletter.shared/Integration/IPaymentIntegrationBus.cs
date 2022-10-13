using azureservicebusdeadletter.shared.Messages;

namespace azureservicebusdeadletter.shared.Integration
{
    public interface IPaymentIntegrationBus
    {
        Task SendPaymentCreatedAsync(PaymentCreatedIntegrationMessage message);
        Task StartReceiveIntegrationMessages(Func<PaymentCreatedIntegrationMessage, int, Task> messageHandler);
        Task StartReceiveDeadLetterIntegrationMessages();
    }
}