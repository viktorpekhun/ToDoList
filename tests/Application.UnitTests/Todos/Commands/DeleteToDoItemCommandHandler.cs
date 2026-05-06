using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Application.Features.Todos.Handlers;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared.Enums;

namespace Application.UnitTests.Todos.Commands
{
    public class DeleteToDoItemCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteToDoItemCommandHandler _handler;

        public DeleteToDoItemCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new DeleteToDoItemCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteEntityAndReturnSuccess_WhenTodoExists()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var existingTodo = new ToDoItem
            {
                Id = todoId,
                Title = "To Be Deleted"
            };

            var todosList = new List<ToDoItem> { existingTodo };
            var dbSetMock = todosList.BuildMockDbSet();

            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);

            var command = new DeleteToDoItemCommand(todoId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(todoId);

            _contextMock.Verify(x => x.Set<ToDoItem>().Remove(existingTodo), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenTodoDoesNotExist()
        {
            // Arrange
            var emptyList = new List<ToDoItem>();
            var dbSetMock = emptyList.BuildMockDbSet();
            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);

            var command = new DeleteToDoItemCommand(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);

            _contextMock.Verify(x => x.Set<ToDoItem>().Remove(It.IsAny<ToDoItem>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
