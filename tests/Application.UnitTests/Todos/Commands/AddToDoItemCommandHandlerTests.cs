using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Application.Features.Todos.Handlers;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Shared.Enums;

namespace Application.UnitTests.Todos.Commands
{
    public class AddToDoItemCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<ToDoItem>> _dbSetMock;
        private readonly AddToDoItemCommandHandler _handler;

        public AddToDoItemCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _dbSetMock = new Mock<DbSet<ToDoItem>>();
            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(_dbSetMock.Object);
            _handler = new AddToDoItemCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResultWithGuid_WhenCommandIsValid()
        {
            // Arrange
            var command = new AddToDoItemCommand(
                "Test Title",
                "Test Description",
                ToDoItemStatus.ToDo,
                DateTime.UtcNow.AddDays(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_MapCommandToEntityCorrectly()
        {
            // Arrange
            var command = new AddToDoItemCommand(
                "Learn Unit Testing",
                "Write tests for handlers",
                ToDoItemStatus.InProgress,
                DateTime.UtcNow.AddDays(2));

            ToDoItem? capturedEntity = null;

            _dbSetMock.Setup(x => x.Add(It.IsAny<ToDoItem>()))
                      .Callback<ToDoItem>(entity => capturedEntity = entity);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity!.Title.Should().Be(command.Title);
            capturedEntity.Description.Should().Be(command.Description);
            capturedEntity.DueDate.Should().Be(command.DueDate);
            capturedEntity.Status.Should().Be(command.Status);

            capturedEntity.Id.Should().NotBeEmpty();
            capturedEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            capturedEntity.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }
    }
}
