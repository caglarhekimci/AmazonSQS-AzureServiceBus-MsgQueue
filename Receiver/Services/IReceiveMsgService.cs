using Receiver.Models;

namespace Receiver.Services
{
    public interface IReceiveMsgService
    {
        Task<ListMsgTypesVM> ReceiveMessageAsync(bool isAmazon = true, bool isAzure = true);
    }
}
