using FluentValidation.TestHelper;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Application.Features.Todos.Validators;
using ToDoList.Domain.Shared.Enums;

namespace Application.UnitTests.Todos.Validators
{
    public class AddToDoItemCommandValidatorTests
    {
        private readonly AddToDoItemCommandValidator _validator;

        public AddToDoItemCommandValidatorTests()
        {
            _validator = new AddToDoItemCommandValidator();
        }

        [Fact]
        public void Should_NotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var command = new AddToDoItemCommand(
                "Valid Title",
                "Valid Description",
                ToDoItemStatus.ToDo,
                DateTime.UtcNow.AddDays(1));

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_WhenTitleIsNullOrWhitespace(string invalidTitle)
        {
            // Arrange
            var command = new AddToDoItemCommand(
                invalidTitle,
                "Description",
                ToDoItemStatus.ToDo,
                null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Title is required.");
        }

        [Fact]
        public void Should_HaveError_WhenTitleExceedsMaxLength()
        {
            // Arrange
            string longTitle = new string('A', 101);
            var command = new AddToDoItemCommand(longTitle, null, ToDoItemStatus.ToDo, null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Title must not exceed 100 characters.");
        }

        [Fact]
        public void Should_HaveError_WhenDueDateIsInThePast()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddDays(-1);
            var command = new AddToDoItemCommand("Title", null, ToDoItemStatus.ToDo, pastDate);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DueDate)
                  .WithErrorMessage("Due date cannot be in the past.");
        }
    }
}
