using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWebAPI.Models;
using ToDoListWebAPI.Services;

namespace ToDoListWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(ITodoService todoService) : ControllerBase
    {
        private readonly ITodoService _todoService = todoService;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_todoService.GetAll());
        }

        [HttpPost]
        public IActionResult Post(In_TodoItem input)
        {
            if (string.IsNullOrWhiteSpace(input.Title))
                return BadRequest("Title is required");

            int todo = _todoService.Add(input);
            return Ok(todo);
        }

        [HttpPut]
        public IActionResult Put(In_TodoItem input)
        {
            if (string.IsNullOrWhiteSpace(input.Title))
                return BadRequest("Title is required");

            _todoService.Update(input);
            return Ok(input);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _todoService.Delete(id);
            return NoContent();
        }
    }
}
