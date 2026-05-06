
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Dtos;
using ToDoList.Application.Features.Todos.Queries;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Handlers
{
    public class GetToDoItemByIdQueryHandler
        : IRequestHandler<GetToDoItemByIdQuery, Result<ToDoItemDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetToDoItemByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<ToDoItemDto>> Handle(GetToDoItemByIdQuery request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Set<ToDoItem>().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (todo is null)
                return Result.Failure<ToDoItemDto>(Error.NotFound("NotFound", $"ToDo item with id {request.Id} not found."));

            var dto = new ToDoItemDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                UpdatedAt = todo.UpdatedAt,
                DueDate = todo.DueDate,
                Status = todo.Status
            };
            return Result.Success(dto);
        }
    }
}
