namespace azureservicebusdeadletter.shared.Messages
{
    public abstract class IntegrationMessage
    {
        public DateTime Date { get; set; }
        public Guid CorrelationId { get; set; }

        public IntegrationMessage(Guid correlationId)
        {
            CorrelationId = correlationId;
            Date = DateTime.Now;
        }
    }
}