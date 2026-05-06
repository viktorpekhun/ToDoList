using ToDoList.Domain.Shared.Enums;

namespace ToDoList.Domain.Entities
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public ToDoItemStatus Status { get; set; }
    }
}
