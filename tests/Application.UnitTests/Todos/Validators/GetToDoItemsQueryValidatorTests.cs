using FluentValidation.TestHelper;
using ToDoList.Application.Features.Todos.Queries;
using ToDoList.Application.Features.Todos.Validators;

namespace Application.UnitTests.Todos.Validators
{
    public class GetToDoItemsQueryValidatorTests
    {
        private readonly GetToDoItemsQueryValidator _validator;

        public GetToDoItemsQueryValidatorTests()
        {
            _validator = new GetToDoItemsQueryValidator();
        }

        [Fact]
        public void Should_NotHaveError_WhenQueryIsValid()
        {
            // Arrange
            var query = new GetToDoItemsQuery { PageNumber = 1, PageSize = 50 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveError_WhenPageNumberIsLessThanOne(int invalidPageNumber)
        {
            // Arrange
            var query = new GetToDoItemsQuery { PageNumber = invalidPageNumber, PageSize = 10 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageNumber)
                  .WithErrorMessage("Page number must be at least 1.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveError_WhenPageSizeIsLessThanOne(int invalidPageSize)
        {
            // Arrange
            var query = new GetToDoItemsQuery { PageNumber = 1, PageSize = invalidPageSize };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageSize)
                  .WithErrorMessage("Page size must be at least 1.");
        }

        [Fact]
        public void Should_HaveError_WhenPageSizeExceedsMaximum()
        {
            // Arrange
            var query = new GetToDoItemsQuery { PageNumber = 1, PageSize = 101 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageSize)
                  .WithErrorMessage("Page size must not exceed 100.");
        }
    }
}
