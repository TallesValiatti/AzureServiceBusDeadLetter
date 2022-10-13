using azureservicebusdeadletter.shared.CorrelationId;

namespace azureservicebusdeadletter.worker.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly ICorrelationId _correlationId;
        public PaymentService(ILogger<PaymentService> logger, ICorrelationId correlationId)
        {
            _logger = logger;
            _correlationId = correlationId;
        }
        public Task ProcessPayment(Guid paymentId)
        {
            try
            {
                _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Starting payment processing");

                // Process the message
    
                // Random rnd = new Random();
                // int value  = rnd.Next(1, 3);

                // if(value.Equals(2))
                   throw new Exception("Some failured");

                _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Payment processing succeeded");   

                return Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($" Correlation Id: {_correlationId.Get()} - Payment processing failed - {ex.Message}");
                throw;
            }
        }
    }
}