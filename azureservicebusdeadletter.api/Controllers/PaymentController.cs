using azureservicebusdeadletter.shared.CorrelationId;
using azureservicebusdeadletter.shared.Entities;
using azureservicebusdeadletter.shared.Integration;
using azureservicebusdeadletter.shared.Messages;
using Microsoft.AspNetCore.Mvc;

namespace azureservicebusdeadletter.api.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentIntegrationBus _paymentIntegrationBus;
    private readonly ICorrelationId _correlationId ;

    public PaymentController(ILogger<PaymentController> logger, 
                             IPaymentIntegrationBus paymentIntegrationBus, 
                             ICorrelationId correlationId)
    {
        _logger = logger;
        _paymentIntegrationBus = paymentIntegrationBus;
        _correlationId = correlationId;
    }

    [HttpPost(Name = "CreatePayment")]
    public async Task<IActionResult> Post()
    {
        _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Creating payment");
        
        var payment = new Payment();
        
        _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Payment created");
        
        _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Salving payment");
        
        // Save the payment aggregate in the database
        
        _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Payment saved");
        
        _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - Sending PaymentCreatedIntegrationMessage");
        
        var paymentCreatedMessage = new PaymentCreatedIntegrationMessage(payment.Id, _correlationId.Get());
        await _paymentIntegrationBus.SendPaymentCreatedAsync(paymentCreatedMessage);
        
        _logger.LogInformation($" Correlation Id: {_correlationId.Get()} - PaymentCreatedIntegrationMessage sent");
        
        return Ok();
    }
}
