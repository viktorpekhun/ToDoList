using MediatR;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared;

namespace ToDoList.Application.Features.Todos.Handlers
{
    public class AddToDoItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AddToDoItemCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddToDoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new ToDoItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Set<ToDoItem>().Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(entity.Id);
        }
    }
}
