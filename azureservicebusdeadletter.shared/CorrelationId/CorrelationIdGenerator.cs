namespace azureservicebusdeadletter.shared.CorrelationId
{
    public static class CorrelationIdGenerator
    {
        public static Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}