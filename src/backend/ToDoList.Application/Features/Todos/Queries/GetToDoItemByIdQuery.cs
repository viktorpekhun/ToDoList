using MediatR;
using ToDoList.Application.Features.Todos.Dtos;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Queries
{
    public record GetToDoItemByIdQuery(Guid Id) : IRequest<Result<ToDoItemDto>>;
}
