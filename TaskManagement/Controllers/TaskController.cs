using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManagement.Dtos;
using TaskManagement.Exceptions;
using TaskManagement.Helper;
using TaskManagement.Models;
using TaskManagement.Services.TaskService;

namespace TaskManagement.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            var dtos = TaskMapper.ToDtoList(tasks);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                return Ok(TaskMapper.ToDto(task));
            }
            catch (TaskNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("status/{status}")]
        public async Task<IHttpActionResult> GetByStatus(string status)
        {
            try
            {
                var tasks = await _taskService.GetByStatusAsync(status);
                return Ok(TaskMapper.ToDtoList(tasks));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (dto == null)
                return BadRequest("Task data is required.");

            try
            {
                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Status = dto.Status
                };

                await _taskService.CreateAsync(task);
                return StatusCode(HttpStatusCode.Created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            if (dto == null)
                return BadRequest("Task is invalid or ID mismatch.");

            try
            {
                var task = new TaskItem
                {
                    Id = id,
                    Title = dto.Title,
                    Description = dto.Description,
                    Status = dto.Status
                };

                await _taskService.UpdateAsync(task);
                return Ok();
            }
            catch (TaskNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                await _taskService.DeleteAsync(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (TaskNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
