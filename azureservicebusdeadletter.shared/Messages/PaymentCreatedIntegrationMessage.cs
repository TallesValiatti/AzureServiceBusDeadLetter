namespace azureservicebusdeadletter.shared.Messages
{
    public class PaymentCreatedIntegrationMessage : IntegrationMessage
    {
        public Guid PaymentId { get; private set; }

        public PaymentCreatedIntegrationMessage(Guid paymentId, Guid correlationId) : base(correlationId)
        {
            PaymentId = paymentId;
        }
    }
}