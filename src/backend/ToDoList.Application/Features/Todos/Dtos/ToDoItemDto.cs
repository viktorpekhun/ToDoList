
using ToDoList.Domain.Shared.Enums;

namespace ToDoList.Application.Features.Todos.Dtos
{
    public class ToDoItemDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public DateTime UpdatedAt { get; init; }
        public DateTime? DueDate { get; init; }
        public ToDoItemStatus Status { get; init; }
    }
}
