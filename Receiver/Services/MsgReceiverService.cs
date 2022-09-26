using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Messaging.ServiceBus;
//using Azure.Messaging.ServiceBus.Administration;
using Receiver.Models;

namespace Receiver.Services
{
    public class MsgReceiverService : IReceiveMsgService
    {
        private IConfiguration _config;
        //static ServiceBusClient client;
        static ListMsgTypesVM model = new ListMsgTypesVM();
        static TimeSpan _maxWaitTime = new TimeSpan(0, 0, 4);
        public MsgReceiverService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<ListMsgTypesVM> ReceiveMessageAsync(bool isAmazon = true, bool isAzure = true)
        {
            model.amazonMessage = "NO MESSAGE on Amazon";
            model.azureMessage = "NO MESSAGE on Azure";

            if (isAmazon == true && isAzure == true)
            {
                // without await
                await AmazonReceiveMsgAsync();
                await AzureReceiveMsgAsync();
            }
            else
            {
                if (isAmazon == true) await AmazonReceiveMsgAsync();
                else await AzureReceiveMsgAsync();
            }
            return model;
        }

        public async Task<string> AmazonReceiveMsgAsync()
        {
            string _accessKey = _config.GetConnectionString("AmazonAccessKey");
            string _secretKey = _config.GetConnectionString("AmazonSecretKey");
            string _queueUrl = _config.GetConnectionString("AmazonQueueUrl");
            var _sqsClient = new AmazonSQSClient(_accessKey, _secretKey, RegionEndpoint.EUWest2);

            var receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = _queueUrl;
            receiveMessageRequest.VisibilityTimeout = 10;
            receiveMessageRequest.WaitTimeSeconds = 4;

            var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);

            if (response.Messages.Count > 0)
            {
                model.amazonMessage = response.Messages[0].Body;
                await _sqsClient.DeleteMessageAsync(_queueUrl, response.Messages[0].ReceiptHandle);
            }
            return model.amazonMessage;
        }

        public async Task<string> AzureReceiveMsgAsync()
        {
            string _connString = _config.GetConnectionString("AzureConnString");
            string _queueName = _config.GetConnectionString("AzureQueueName");
            var client = new ServiceBusClient(_connString);
            var receiver = client.CreateReceiver(_queueName);

            //var administrationClient = new ServiceBusAdministrationClient(_connString);
            //var props = await administrationClient.GetQueueRuntimePropertiesAsync(_queueName);
            //var messageCount = props.Value.ActiveMessageCount;

            try
            {
                var message = await receiver.ReceiveMessageAsync(_maxWaitTime);
                if (message != null)
                {
                    model.azureMessage = message.Body.ToString();
                    await receiver.CompleteMessageAsync(message);
                }
            }
            finally
            {
                await receiver.DisposeAsync();
                await client.DisposeAsync();
            }
            return model.azureMessage;
        }
    }
}
