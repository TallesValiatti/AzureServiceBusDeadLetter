namespace azureservicebusdeadletter.shared.Events
{
    public abstract class IntegrationEvent
    {
        public DateTime Date { get; set; }
        public Guid CorrelationId { get; set; }

        public IntegrationEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
            Date = DateTime.Now;
        }
    }
}