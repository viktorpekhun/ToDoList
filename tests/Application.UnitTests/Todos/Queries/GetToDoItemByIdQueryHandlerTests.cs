
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Handlers;
using ToDoList.Application.Features.Todos.Queries;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared.Enums;

namespace Application.UnitTests.Todos.Queries
{
    public class GetToDoItemByIdQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly GetToDoItemByIdQueryHandler _handler;

        public GetToDoItemByIdQueryHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new GetToDoItemByIdQueryHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResultWithDto_WhenTodoExists()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var existingTodo = new ToDoItem
            {
                Id = todoId,
                Title = "Read Clean Architecture",
                Description = "By Robert C. Martin",
                DueDate = DateTime.UtcNow.AddDays(7),
                Status = ToDoItemStatus.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var todosList = new List<ToDoItem> { existingTodo };
            var dbSetMock = todosList.BuildMockDbSet();

            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);

            var query = new GetToDoItemByIdQuery(todoId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(existingTodo.Id);
            result.Value.Title.Should().Be(existingTodo.Title);
            result.Value.Description.Should().Be(existingTodo.Description);
            result.Value.Status.Should().Be(existingTodo.Status);
            result.Value.DueDate.Should().Be(existingTodo.DueDate);
            result.Value.UpdatedAt.Should().Be(existingTodo.UpdatedAt);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenTodoDoesNotExist()
        {
            // Arrange
            var emptyList = new List<ToDoItem>();
            var dbSetMock = emptyList.BuildMockDbSet();

            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);

            var query = new GetToDoItemByIdQuery(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
        }
    }
}
