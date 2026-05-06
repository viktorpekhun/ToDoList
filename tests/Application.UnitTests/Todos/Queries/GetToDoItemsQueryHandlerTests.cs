using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Features.Todos.Handlers;
using ToDoList.Application.Features.Todos.Queries;
using ToDoList.Domain.Entities;

namespace Application.UnitTests.Todos.Queries
{
    public class GetToDoItemsQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly GetToDoItemsQueryHandler _handler;

        public GetToDoItemsQueryHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new GetToDoItemsQueryHandler(_contextMock.Object);
        }

        private void SetupMockDbWithItems(int count)
        {
            var items = new List<ToDoItem>();
            for (int i = 1; i <= count; i++)
            {
                items.Add(new ToDoItem
                {
                    Id = Guid.NewGuid(),
                    Title = $"Todo Item {i}",
                    CreatedAt = DateTime.UtcNow
                });
            }

            var dbSetMock = items.BuildMockDbSet();
            _contextMock.Setup(x => x.Set<ToDoItem>()).Returns(dbSetMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFirstPage_WhenPageNumberIs1()
        {
            // Arrange
            SetupMockDbWithItems(15);
            var query = new GetToDoItemsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var paginatedList = result.Value;
            paginatedList.Should().NotBeNull();
            paginatedList.Items.Count.Should().Be(10);
            paginatedList.TotalCount.Should().Be(15);
            paginatedList.TotalPages.Should().Be(2);
            paginatedList.PageNumber.Should().Be(1);
        }

        [Fact]
        public async Task Handle_Should_ReturnRemainingItemsOnSecondPage()
        {
            // Arrange
            SetupMockDbWithItems(15);
            var query = new GetToDoItemsQuery { PageNumber = 2, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var paginatedList = result.Value;
            paginatedList.Items.Count.Should().Be(5);
            paginatedList.TotalCount.Should().Be(15);
            paginatedList.TotalPages.Should().Be(2);
            paginatedList.HasNextPage.Should().BeFalse();
            paginatedList.HasPreviousPage.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenPageNumberIsBeyondTotalPages()
        {
            // Arrange
            SetupMockDbWithItems(5);
            var query = new GetToDoItemsQuery { PageNumber = 5, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var paginatedList = result.Value;
            paginatedList.Items.Should().BeEmpty();
            paginatedList.TotalCount.Should().Be(5);
        }
    }
}
