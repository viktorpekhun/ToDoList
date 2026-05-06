using MediatR;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Commands
{
    public record DeleteToDoItemCommand(Guid Id) : IRequest<Result<Guid>>;
}
