using azureservicebusdeadletter.shared.CorrelationId;
using azureservicebusdeadletter.shared.Entities;
using azureservicebusdeadletter.shared.Integration;
using azureservicebusdeadletter.shared.Events;
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
        this.LogInformation("Creating payment");
     
        var payment = new Payment();
        
        this.LogInformation("Payment created");
        
        this.LogInformation("Salving payment");
        
        // Save the payment aggregate in the database
        
        this.LogInformation("Payment saved");
        
        this.LogInformation("Sending PaymentCreatedIntegrationEvent");
        
        var paymentCreatedMessage = new PaymentCreatedIntegrationEvent(payment.Id, _correlationId.Get());
        await _paymentIntegrationBus.SendPaymentCreatedAsync(paymentCreatedMessage);

        this.LogInformation("PaymentCreatedIntegrationEvent sent");        
        
        return Ok();
    }

    private void LogInformation(string message)
    {
        _logger.LogInformation($"{_correlationId.Get()} - {message} - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")}");
    }
}
