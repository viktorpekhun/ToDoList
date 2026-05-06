using FluentValidation;
using ToDoList.Application.Features.Todos.Queries;

namespace ToDoList.Application.Features.Todos.Validators
{
    public class GetToDoItemByIdQueryValidator : AbstractValidator<GetToDoItemByIdQuery>
    {
        public GetToDoItemByIdQueryValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("ToDo item ID is required.");
        }
    }
}
