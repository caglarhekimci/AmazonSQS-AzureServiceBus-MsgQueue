using Microsoft.AspNetCore.Mvc;
using Sender.Models;
using Sender.Services;
using System.Diagnostics;
using System.Text.Json;

namespace Sender.Controllers
{
    public class SendMessageController : Controller
    {
        private readonly ISendMessageService _messageService;

        public SendMessageController(ISendMessageService messageService)
        {
            _messageService = messageService;
        }
        public IActionResult MessageSender()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MessageSenderAsync(MessageTypesVM message)
        {
            await _messageService.SendMessageAsync(message);
            return RedirectToAction("MessageSender");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
