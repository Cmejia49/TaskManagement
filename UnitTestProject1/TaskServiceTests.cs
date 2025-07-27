using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManagement.Exceptions;
using TaskManagement.Models;
using TaskManagement.Repositories.TaskRepository;
using TaskManagement.Services.TaskService;
using Xunit;

namespace TaskManagement.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _mockRepo = new Mock<ITaskRepository>();
            _service = new TaskService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Task()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Task",
                Description = "test",
                Status = "Pending"
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(task);

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.Is<TaskItem>(t => t.Title == "Task")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Task_Not_Found()
        {
            // Arrange
            var updateTask = new TaskItem
            {
                Id = 999,
                Title = "Doesn't exist",
                Status = "Pending"
            };

            _mockRepo.Setup(r => r.UpdateAsync(updateTask))
                     .ThrowsAsync(new TaskNotFoundException(updateTask.Id));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<TaskNotFoundException>(() => _service.UpdateAsync(updateTask));
            Assert.Equal($"Task with ID {updateTask.Id} was not found.", ex.Message);
        }

        [Fact]
        public async Task GetByStatusAsync_Should_Return_Filtered_Tasks()
        {
            // Arrange
            var status = "Completed";
            var sampleTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "T1", Status = "Completed" },
                new TaskItem { Id = 2, Title = "T2", Status = "Completed" }
            };

            _mockRepo.Setup(r => r.GetByStatusAsync(status)).ReturnsAsync(sampleTasks);

            // Act
            var result = await _service.GetByStatusAsync(status);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.Equal("Completed", t.Status));
        }
    }
}
