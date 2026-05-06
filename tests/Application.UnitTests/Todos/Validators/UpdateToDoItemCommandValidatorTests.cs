using FluentValidation.TestHelper;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Application.Features.Todos.Validators;
using ToDoList.Domain.Shared.Enums;

namespace Application.UnitTests.Todos.Validators
{
    public class UpdateTodoCommandValidatorTests
    {
        private readonly UpdateToDoItemCommandValidator _validator;

        public UpdateTodoCommandValidatorTests()
        {
            _validator = new UpdateToDoItemCommandValidator();
        }

        [Fact]
        public void Should_NotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateToDoItemCommand(
                Guid.NewGuid(),
                "Updated Title",
                "Updated Description",
                DateTime.UtcNow.AddDays(1),
                ToDoItemStatus.Done);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_HaveError_WhenIdIsEmpty()
        {
            // Arrange
            var command = new UpdateToDoItemCommand(
                Guid.Empty,
                "Title",
                null,
                null,
                ToDoItemStatus.ToDo);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("ToDo item ID is required.");
        }

        [Fact]
        public void Should_HaveError_WhenTitleIsTooLong()
        {
            // Arrange
            string longTitle = new string('X', 105);
            var command = new UpdateToDoItemCommand(
                Guid.NewGuid(),
                longTitle,
                null,
                null,
                ToDoItemStatus.ToDo);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Title must not exceed 100 characters.");
        }
    }
}
