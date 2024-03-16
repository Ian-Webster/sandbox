using Lit.Test.API.Models;
using Lit.Test.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lit.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetItems()
        {
            return _todoService.GetTodoItems();
        }

        [HttpPost()]
        public ActionResult AddItem([FromBody]TodoAddItem item)
        {
            _todoService.AddTodoItem(item);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        public ActionResult ToggleCompleted(int id)
        {
            _todoService.ToggleCompleted(id);
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
