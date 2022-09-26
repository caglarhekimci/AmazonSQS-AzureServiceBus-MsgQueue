using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Messaging.ServiceBus;
using Sender.Models;

namespace Sender.Services
{
    public class MessageSenderService : ISendMessageService
    {
        private IConfiguration _config;
        public MessageSenderService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> SendMessageAsync(MessageTypesVM message, bool isAmazon = true, bool isAzure = true)
        {
            return (isAmazon == false || await AmazonSendMsgAsync(message)) && (isAzure == false || await AzureSendMsgAsync(message));
        }

        private async Task<bool> AmazonSendMsgAsync(MessageTypesVM message)
        {
            string _accessKey = _config.GetConnectionString("AmazonAccessKey");
            string _secretKey = _config.GetConnectionString("AmazonSecretKey");
            var sqsClient = new AmazonSQSClient(_accessKey, _secretKey, RegionEndpoint.EUWest2);
            var request = new SendMessageRequest
            {
                QueueUrl = _config.GetConnectionString("AmazonQueueUrl"),
                MessageBody = message.myMessage.ToString()
            };
            await sqsClient.SendMessageAsync(request);
            return true;
        }

        private async Task<bool> AzureSendMsgAsync(MessageTypesVM message)
        {
            string _connString = _config.GetConnectionString("AzureConnString");
            string _queueName = _config.GetConnectionString("AzureQueueName");
            ServiceBusClient client = new ServiceBusClient(_connString);
            ServiceBusSender sender = client.CreateSender(_queueName);
            try
            {
                ServiceBusMessage messageAzure = new ServiceBusMessage(message.myMessage);
                await sender.SendMessageAsync(messageAzure);
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
            return true;
        }
    }
}
