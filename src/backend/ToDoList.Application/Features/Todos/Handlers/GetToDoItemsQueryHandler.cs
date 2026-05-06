using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Common.Models;
using ToDoList.Application.Features.Todos.Dtos;
using ToDoList.Application.Features.Todos.Queries;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Handlers
{
    public class GetToDoItemsQueryHandler
        : IRequestHandler<GetToDoItemsQuery, Result<PaginatedList<ToDoItemDto>>>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetToDoItemsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<PaginatedList<ToDoItemDto>>> Handle(GetToDoItemsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<ToDoItem>()
                .AsNoTracking()
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => new ToDoItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    UpdatedAt = t.UpdatedAt,
                    Status = t.Status
                });

            var paginatedList = await PaginatedList<ToDoItemDto>.CreateAsync(
                query,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            return Result.Success(paginatedList);
        }
    }
}
