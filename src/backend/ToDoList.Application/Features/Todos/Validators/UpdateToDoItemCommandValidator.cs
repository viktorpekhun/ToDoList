using FluentValidation;
using ToDoList.Application.Features.Todos.Commands;

namespace ToDoList.Application.Features.Todos.Validators
{
    public class UpdateToDoItemCommandValidator : AbstractValidator<UpdateToDoItemCommand>
    {
        public UpdateToDoItemCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("ToDo item ID is required.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(v => v.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date cannot be in the past.")
                .When(v => v.DueDate.HasValue);

            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Invalid status specified.");
        }
    }
}
