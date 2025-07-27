using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagement.Dtos;
using TaskManagement.Models;

namespace TaskManagement.Helper
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(TaskItem task)
        {
            if (task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            };
        }

        public static IEnumerable<TaskDto> ToDtoList(IEnumerable<TaskItem> tasks)
        {
            if (tasks == null) yield break;

            foreach (var task in tasks)
                yield return ToDto(task);
        }
    }
}