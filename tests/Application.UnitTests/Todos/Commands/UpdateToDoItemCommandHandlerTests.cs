using Moq;
using MockQueryable.Moq;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Handlers;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared.Enums;
using FluentAssertions;
using ToDoList.Application.Features.Todos.Commands;

namespace Application.UnitTests.Todos.Commands
{
    public class UpdateTodoCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateToDoItemCommandHandler _handler;

        public UpdateTodoCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new UpdateToDoItemCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateEntityAndReturnSuccess_WhenTodoExists()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var existingTodo = new ToDoItem
            {
                Id = todoId,
                Title = "Old Title",
                Description = "Old Description",
                Status = ToDoItemStatus.ToDo,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var todosList = new List<ToDoItem> { existingTodo };
            var dbSetMock = todosList.BuildMockDbSet();

            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);

            var command = new UpdateToDoItemCommand(
                todoId,
                "New Title",
                "New Description",
                DateTime.UtcNow.AddDays(5),
                ToDoItemStatus.InProgress);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(todoId);
            existingTodo.Title.Should().Be(command.Title);
            existingTodo.Description.Should().Be(command.Description);
            existingTodo.Status.Should().Be(command.Status);
            existingTodo.DueDate.Should().Be(command.DueDate);
            existingTodo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            existingTodo.CreatedAt.Should().BeBefore(existingTodo.UpdatedAt);

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenTodoDoesNotExist()
        {
            // Arrange
            var emptyList = new List<ToDoItem>();
            var dbSetMock = emptyList.BuildMockDbSet();
            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);

            var command = new UpdateToDoItemCommand(
                Guid.NewGuid(),
                "Title",
                "Description",
                null,
                ToDoItemStatus.ToDo);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
