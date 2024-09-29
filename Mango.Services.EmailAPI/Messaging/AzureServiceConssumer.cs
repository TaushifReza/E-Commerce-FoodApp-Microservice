using System.Text;
using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.Dtos;
using Newtonsoft.Json;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string serviceBusConectionString;
        private readonly string emailCartQueue;

        private ServiceBusProcessor _emailCartProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            serviceBusConectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
        }

        public async Task StartAsync()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
        {
            // this is where u received message
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //TODO - try to log email
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task StopAsync()
        {
            await _emailCartProcessor.StartProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }
    }
}
