using Microsoft.AspNetCore.SignalR;
using Receiver.Models;

namespace Receiver.Hubs
{
    public class MessageHubcs : Hub
    {
        ListMsgTypesVM model = new ListMsgTypesVM();
        public async Task AzureListener() => await Clients.All.SendAsync("ReceiveMessageAzure", model.azureMessage);
        public async Task AmazonListener() => await Clients.All.SendAsync("ReceiveMessageAmazon", model.amazonMessage);
    }
}
