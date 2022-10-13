namespace azureservicebusdeadletter.shared.CorrelationId
{
    public class CorrelationId : ICorrelationId
    {
        private Guid _correlationId;

        public Guid Get()
        {
           return _correlationId;
        }

        public void Set(Guid correlationId)
        {
            _correlationId = correlationId;
        }
    }
}