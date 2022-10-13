namespace azureservicebusdeadletter.worker.Services
{
    public interface IPaymentService
    {
        Task ProcessPayment(Guid paymentId);
    }
}