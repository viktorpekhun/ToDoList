
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Handlers
{
    public class DeleteToDoItemCommandHandler 
        : IRequestHandler<DeleteToDoItemCommand, Result<Guid>>
    {
        private readonly IApplicationDbContext _dbContext;
        public DeleteToDoItemCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Handle(DeleteToDoItemCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Set<ToDoItem>().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (todo is null)
                return Result.Failure<Guid>(Error.NotFound("NotFound", $"ToDo item with id {request.Id} not found."));

            _dbContext.Set<ToDoItem>().Remove(todo);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(todo.Id);
        }
    }
}
