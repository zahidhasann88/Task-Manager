using System;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.DTOs;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("get-tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetById([FromBody] GetByIdDto getByIdDto)
        {
            var task = await _taskService.GetByIdAsync(getByIdDto.Id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost("create-task")]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem taskItem)
        {
            try
            {
                await _taskService.CreateAsync(taskItem);
                return CreatedAtAction(nameof(GetById), new { id = taskItem.Id }, taskItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-task")]
        public async Task<IActionResult> Update([FromBody] UpdateTaskDTO updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskItem = new TaskItem
            {
                Id = updateTaskDto.Id,
                Title = updateTaskDto.Title,
                Description = updateTaskDto.Description,
                DueDate = updateTaskDto.DueDate
            };

            try
            {
                await _taskService.UpdateAsync(taskItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-task")]
        public async Task<IActionResult> Delete([FromBody] DeleteTaskDTO deleteDto)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(deleteDto.Id);
                if (task == null)
                {
                    return NotFound();
                }
                await _taskService.DeleteAsync(task);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
