using FluentValidation.TestHelper;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Application.Features.Todos.Validators;

namespace Application.UnitTests.Todos.Validators
{
    public class DeleteToDoItemCommandValidatorTests
    {
        private readonly DeleteToDoItemCommandValidator _validator;

        public DeleteToDoItemCommandValidatorTests()
        {
            _validator = new DeleteToDoItemCommandValidator();
        }

        [Fact]
        public void Should_NotHaveError_WhenIdIsValid()
        {
            // Arrange
            var command = new DeleteToDoItemCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_HaveError_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteToDoItemCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("ToDo item ID is required.");
        }
    }
}
