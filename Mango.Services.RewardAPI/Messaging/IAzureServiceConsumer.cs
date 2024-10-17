namespace Mango.Services.RewardAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task StartAsync();
        Task StopAsync();
    }
}
