using MediatR;
using ToDoList.Domain.Shared;
using ToDoList.Domain.Shared.Enums;

namespace ToDoList.Application.Features.Todos.Commands
{
    public record UpdateToDoItemCommand(
        Guid Id,
        string Title,
        string? Description,
        DateTime? DueDate,
        ToDoItemStatus Status
    ) : IRequest<Result<Guid>>;
}
