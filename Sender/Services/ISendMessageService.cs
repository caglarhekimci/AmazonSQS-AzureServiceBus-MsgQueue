using Sender.Models;

namespace Sender.Services
{
    public interface ISendMessageService
    {
        Task<bool> SendMessageAsync(MessageTypesVM message, bool isAmazon = true, bool isAzure = true);
    }
}
