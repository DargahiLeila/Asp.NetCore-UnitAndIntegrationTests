using Application.Implements.User;
using DataAccess.Services.Commands.User;
using DomainModel.DTO.UserModel;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Tests.ApplicationTests.Commands.User
{
    public class UserCommandServiceUnitTests
    {

        private readonly Mock<IUserCommandRepository> _repoMock;
        private readonly UserCommandService _service;

        public UserCommandServiceUnitTests()
        {
            _repoMock = new Mock<IUserCommandRepository>();
            _service = new UserCommandService(_repoMock.Object);
        }
        [Fact]
        public async Task AddUserAsync_WhenNameIsUnique_ShouldAddUser()
        {
            var model = new UserAddModel { Name = "Ali" };
            _repoMock.Setup(r => r.ExistNameAsync("Ali")).ReturnsAsync(false);
            _repoMock.Setup(r => r.AddAsync(model)).ReturnsAsync(1);

            var result = await _service.AddUserAsync(model);

            result.Should().Be(1);
        }

        [Fact]
        public async Task AddUserAsync_WhenNameExists_ShouldThrowArgumentException()
        {
            var model = new UserAddModel { Name = "Ali" };
            _repoMock.Setup(r => r.ExistNameAsync("Ali")).ReturnsAsync(true);

            var act = async () => await _service.AddUserAsync(model);

            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("نام کاربر تکراری است");
        }
        [Fact]
        public async Task UpdateUserAsync_WhenNameIsUnique_ShouldUpdateUser()
        {
            var model = new UserUpdateModel { Id = 5, Name = "Sara" };
            _repoMock.Setup(r => r.ExistNameAsync("Sara", 5)).ReturnsAsync(false);
            _repoMock.Setup(r => r.UpdateAsync(model)).ReturnsAsync(5);

            var result = await _service.UpdateUserAsync(model);

            result.Should().Be(5);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenNameExists_ShouldThrowArgumentException()
        {
            var model = new UserUpdateModel { Id = 5, Name = "Sara" };
            _repoMock.Setup(r => r.ExistNameAsync("Sara", 5)).ReturnsAsync(true);

            var act = async () => await _service.UpdateUserAsync(model);

            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("نام کاربر تکراری است");
        }
        [Fact]
        public async Task ActiveUserAsync_ShouldCallRepository()
        {
            _repoMock.Setup(r => r.ActiveUserAsync(1)).ReturnsAsync(1);

            var result = await _service.ActiveUserAsync(1);

            result.Should().Be(1);
        }

        [Fact]
        public async Task DeActiveUserAsync_ShouldCallRepository()
        {
            _repoMock.Setup(r => r.DeActiveUserAsync(2)).ReturnsAsync(2);

            var result = await _service.DeActiveUserAsync(2);

            result.Should().Be(2);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldCallRepository()
        {
            _repoMock.Setup(r => r.DeleteAsync(3)).ReturnsAsync(3);

            var result = await _service.DeleteUserAsync(3);

            result.Should().Be(3);
        }

    }
}
