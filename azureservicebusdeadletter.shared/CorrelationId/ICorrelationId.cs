namespace azureservicebusdeadletter.shared.CorrelationId
{
    public interface ICorrelationId
    {
        Guid Get();
        void Set(Guid correlationId);
    }
}