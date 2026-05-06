using FluentValidation;
using ToDoList.Application.Features.Todos.Commands;

namespace ToDoList.Application.Features.Todos.Validators
{
    public class DeleteToDoItemCommandValidator : AbstractValidator<DeleteToDoItemCommand>
    {
        public DeleteToDoItemCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("ToDo item ID is required.");
        }
    }
}
