
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Handlers
{
    public class UpdateToDoItemCommandHandler :
        IRequestHandler<UpdateToDoItemCommand, Result<Guid>>
    {
        private readonly IApplicationDbContext _dbContext;
        public UpdateToDoItemCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<Guid>> Handle(UpdateToDoItemCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Set<ToDoItem>().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (todo is null)
                return Result.Failure<Guid>(Error.NotFound("NotFound", $"ToDo item with id {request.Id} not found."));

            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.DueDate = request.DueDate;
            todo.Status = request.Status;

            todo.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(todo.Id);
        }
    }
}
