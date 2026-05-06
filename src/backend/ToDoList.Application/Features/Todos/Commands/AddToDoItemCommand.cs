using MediatR;
using ToDoList.Domain.Shared;
using ToDoList.Domain.Shared.Enums;

namespace ToDoList.Application.Features.Todos.Commands
{
    public record AddToDoItemCommand(
        string Title, 
        string? Description, 
        ToDoItemStatus Status, 
        DateTime? DueDate
    ) : IRequest<Result<Guid>>;
}
