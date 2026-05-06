using MediatR;
using ToDoList.Application.Common.Models;
using ToDoList.Application.Features.Todos.Dtos;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Queries
{
    public record GetToDoItemsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PaginatedList<ToDoItemDto>>>;
}
