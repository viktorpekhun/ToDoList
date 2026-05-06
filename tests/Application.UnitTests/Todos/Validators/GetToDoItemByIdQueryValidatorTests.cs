using FluentValidation.TestHelper;
using ToDoList.Application.Features.Todos.Queries;
using ToDoList.Application.Features.Todos.Validators;

namespace Application.UnitTests.Todos.Validators
{
    public class GetToDoItemByIdQueryValidatorTests
    {
        private readonly GetToDoItemByIdQueryValidator _validator;

        public GetToDoItemByIdQueryValidatorTests()
        {
            _validator = new GetToDoItemByIdQueryValidator();
        }

        [Fact]
        public void Should_NotHaveError_WhenIdIsValid()
        {
            // Arrange
            var query = new GetToDoItemByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_HaveError_WhenIdIsEmpty()
        {
            // Arrange
            var query = new GetToDoItemByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("ToDo item ID is required.");
        }
    }
}
