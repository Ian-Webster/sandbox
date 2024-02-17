using Messaging.Outbox.Consumer2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Outbox.Consumer2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _messageService.GetMessages();
            if (!result.Any())
                return NoContent();

            return Ok(result);
        }
    }
}
