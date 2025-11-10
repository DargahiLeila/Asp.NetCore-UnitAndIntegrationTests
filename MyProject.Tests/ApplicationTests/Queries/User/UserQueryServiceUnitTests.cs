using Application.Implements.User;
using DataAccess.Services.Queries;
using DomainModel.DTO.UserModel;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Tests.ApplicationTests.Queries.User
{
    public class UserQueryServiceUnitTests
    {

        private readonly Mock<IUserQueryRepository> _repoMock;
        private readonly UserQueryService _service;

        public UserQueryServiceUnitTests()
        {
            _repoMock = new Mock<IUserQueryRepository>();
            _service = new UserQueryService(_repoMock.Object);
        }
        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            var users = new List<UserListItem>
    {
        new UserListItem { Id = 1, Name = "Ali" },
        new UserListItem { Id = 2, Name = "Sara" }
    };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _service.GetAllUsersAsync();

            result.Should().BeEquivalentTo(users);
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task GetUserAsync_ShouldReturnUserById()
        {
            var user = new UserGetModel { Id = 5, Name = "Reza", IsDeleted = false };

            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

            var result = await _service.GetUserAsync(5);

            result.Should().BeEquivalentTo(user);
            _repoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        }
        [Fact]
        public async Task SearchUsersAsync_ShouldReturnUsersAndRecordCount()
        {
            var searchModel = new UserSearchModel { Name = "Ali" };
            var users = new List<UserListItem>
    {
        new UserListItem { Id = 1, Name = "Ali" }
    };
            int recordCount=users.Count();

            _repoMock.Setup(r => r.SearchAsync(searchModel)).ReturnsAsync((users, recordCount));

            var (resultUsers, resultCount) = await _service.SearchUsersAsync(searchModel);

            resultUsers.Should().BeEquivalentTo(users);
            resultCount.Should().Be(1);
            _repoMock.Verify(r => r.SearchAsync(searchModel), Times.Once);
        }

    }
}
