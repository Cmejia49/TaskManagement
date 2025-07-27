using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TaskManagement.Exceptions;
using TaskManagement.Models;
using TaskManagement.Repositories.TaskRepository;

namespace TaskManagement.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly HashSet<string> _validStatuses = new HashSet<string> { "Pending", "InProgress", "Completed" };

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID.");

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                throw new TaskNotFoundException(id);

            return task;
        }

        public async Task CreateAsync(TaskItem task)
        {
            ValidateTask(task);

            task.CreatedAt = DateTime.UtcNow;
            await _repository.CreateAsync(task);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            if (task.Id <= 0)
                throw new ArgumentException("Invalid task ID.");

            ValidateTask(task);

            await _repository.UpdateAsync(task);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID.");

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                throw new TaskNotFoundException(id);

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(string status)
        {
            if (!_validStatuses.Contains(status))
                throw new ArgumentException("Invalid task status.");

            return await _repository.GetByStatusAsync(status);
        }

        private void ValidateTask(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty.");

            if (!_validStatuses.Contains(task.Status))
                throw new ArgumentException($"Status must be one of: {string.Join(", ", _validStatuses)}");
        }
    }
}