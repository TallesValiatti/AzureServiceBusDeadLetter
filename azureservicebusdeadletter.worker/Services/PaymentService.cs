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
                this.LogInformation($"Starting payment processing");
                
                //  Payment processing
                throw new Exception("Some exception");
    
                this.LogInformation($"Successful payment processing");
                
                return Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                this.LogError($"Payment processing failure: {ex.Message}");
                throw;
            }
            
        }
        private void LogInformation(string message)
        {
            _logger.LogInformation($"{_correlationId.Get().ToString()} - {message} - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")}");
        }

        private void LogError(string message)
        {
            _logger.LogError($"{_correlationId.Get().ToString()} - {message} - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")}");
        }      
    }
}