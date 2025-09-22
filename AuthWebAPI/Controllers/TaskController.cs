using AuthWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using AuthWebAPI.Entities;
using AuthWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly MyDbContext _context;

        public TaskController(MyDbContext context)
        {
            _context = context;
        }

        // ✅ Create Task
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body cannot be empty." });

            // Map DTO to Entity
            var task = new Entities.Task
            {
                Name = dto.Name,
                Email = dto.Email,
                Description = dto.Description
                // Id is auto-generated (Guid.NewGuid()) in the entity
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Task created successfully",
                task.Id,      // Return generated GUID
                task.Name
            });
        }

        // ✅ Get All Tasks
        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }
        // ✅ Get Task by Id
        [HttpGet("tasks/{id:guid}")]
        public async Task<IActionResult> GetTaskById([FromRoute] Guid id)
        {
            Console.WriteLine($"🔎 Looking for task with Id: {id}");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                Console.WriteLine("⚠ Task not found in DB");
                return NotFound(new { message = "Task not found." });
            }

            Console.WriteLine($"✅ Found task: {task.Name}");
            return Ok(task);
        }

        //Update Task
        [HttpPut("tasks/{id:guid}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] TaskDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body cannot be empty." });

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return NotFound(new { message = "Task not found." });

            // Update fields
            task.Name = dto.Name;
            task.Email = dto.Email;
            task.Description = dto.Description;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Task updated successfully",
                task.Id,
                task.Name
            });
        }

        // Delete Task
        [HttpDelete("tasks/{id:guid}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return NotFound(new { message = "Task not found." });

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task deleted successfully", task.Id });
        }

    }
}
