using System;

namespace TaskManagement.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int id)
            : base($"Task with ID {id} was not found.")
        {
        }
    }
}
