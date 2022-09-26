using Microsoft.AspNetCore.Mvc;
using Receiver.Models;
using Receiver.Services;
using System.Diagnostics;

namespace Receiver.Controllers
{
    public class ReceiveMessagesController : Controller
    {
        private readonly IReceiveMsgService _messageService;
        public ReceiveMessagesController(IReceiveMsgService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> MessageReceiver()
        {
            ListMsgTypesVM model = await _messageService.ReceiveMessageAsync(false, false);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
