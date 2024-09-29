namespace Mango.Services.EmailAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task StartAsync();
        Task StopAsync();
    }
}
