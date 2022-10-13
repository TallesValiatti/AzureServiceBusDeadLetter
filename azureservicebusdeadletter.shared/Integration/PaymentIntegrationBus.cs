using System.Text.Json;
using Azure.Messaging.ServiceBus;
using azureservicebusdeadletter.shared.Messages;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace azureservicebusdeadletter.shared.Integration
{
    public class PaymentIntegrationBus : IPaymentIntegrationBus, IDisposable
    {
        private readonly ServiceBusClient  _serviceBusClient;
        private ServiceBusProcessor? _processor; 
        private readonly ILogger<PaymentIntegrationBus>  _logger;
        private readonly string  _queueName;
        private const string DEAD_LETTER_PATH = "$deadletterqueue";

        public PaymentIntegrationBus(IAzureClientFactory<ServiceBusClient> serviceBusClientFactory, 
                                     IConfiguration configuration, 
                                     ILogger<PaymentIntegrationBus> logger)
        {
            _serviceBusClient = serviceBusClientFactory.CreateClient(configuration.GetSection("ServiceBusNamespace").Value);
            _queueName = configuration.GetSection("QueueName").Value;
            _logger = logger;
        }

        public async Task SendPaymentCreatedAsync(PaymentCreatedIntegrationMessage message)
        {
            var sender = _serviceBusClient.CreateSender(_queueName);

            var body = JsonSerializer.Serialize(message);

            await sender.SendMessageAsync(new ServiceBusMessage(body));
        }
        public async Task StartReceiveIntegrationMessages(Func<PaymentCreatedIntegrationMessage, int, Task> messageHandler)
        {
            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
            };

            _processor = _serviceBusClient.CreateProcessor(_queueName, options);
            
            _processor.ProcessErrorAsync +=  ErrorHandler;
            
            _processor.ProcessMessageAsync += async (arg) => 
            {
                try
                {
                    var message = JsonSerializer.Deserialize<PaymentCreatedIntegrationMessage>(arg.Message.Body.ToString());
                  
                    await messageHandler.Invoke(message!, arg.Message.DeliveryCount);    
                  
                    await arg.CompleteMessageAsync(arg.Message);
                }
                catch (System.Exception)
                {
                    await arg.AbandonMessageAsync(arg.Message);    
                }
                
                return;
            };

            // start processing
            await _processor.StartProcessingAsync();
        }   

         public async Task StartReceiveDeadLetterIntegrationMessages()
        {
            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
            };

            var deadLetterQueue = $"{_queueName}/{DEAD_LETTER_PATH}";

            _processor = _serviceBusClient.CreateProcessor(deadLetterQueue, options);
            
            _processor.ProcessErrorAsync +=  ErrorHandler;
            
            _processor.ProcessMessageAsync += async (arg) => 
            {
                _logger.LogInformation("Receiving dead letter message");

                var sender = _serviceBusClient.CreateSender(_queueName);

                await sender.SendMessageAsync(new ServiceBusMessage(arg.Message.Body.ToString()));
                
                await arg.CompleteMessageAsync(arg.Message);

                _logger.LogInformation("Dead letter message resubmitted");
            };

            // start processing
            await _processor.StartProcessingAsync();
        }  

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _processor?.DisposeAsync();
        }
    }
}