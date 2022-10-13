namespace azureservicebusdeadletter.shared.Events
{
    public class PaymentCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid PaymentId { get; set; }

        public PaymentCreatedIntegrationEvent(Guid paymentId, Guid correlationId) : base(correlationId)
        {
            PaymentId = paymentId;
        }
    }
}