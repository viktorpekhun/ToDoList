using FluentValidation;
using ToDoList.Application.Features.Todos.Queries;

namespace ToDoList.Application.Features.Todos.Validators
{
    public class GetToDoItemsQueryValidator : AbstractValidator<GetToDoItemsQuery>
    {
        public GetToDoItemsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
                .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
        }
    }
}
