using Application.Implements.User;
using DataAccess.Implements.User;
using DomainModel.DTO.UserModel;
using DomainModel.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Tests.ApplicationTests.Commands.User
{
    public class UserCommandServiceIntegrationTests
    {
        
        private readonly db_UnitTestContext _db;
        private readonly UserCommandService _service;

        public UserCommandServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new db_UnitTestContext(options);
            var repo = new UserCommandRepository(_db);
            _service = new UserCommandService(repo);
        }

        [Fact]
        public async Task AddUserAsync_WhenNameIsUnique_ShouldAddUser()
        {
            var model = new UserAddModel { Name = "Ali" };

            var result = await _service.AddUserAsync(model);

            result.Should().BeGreaterThan(0);

            var user = await _db.TblUsers.FindAsync(result);
            user.Should().NotBeNull();
            user!.Name.Should().Be("Ali");
        }

        [Fact]
        public async Task AddUserAsync_WhenNameExists_ShouldThrowArgumentException()
        {
            _db.TblUsers.Add(new TblUser { Name = "Ali" });
            await _db.SaveChangesAsync();

            var model = new UserAddModel { Name = "Ali" };

            var act = async () => await _service.AddUserAsync(model);

            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("نام کاربر تکراری است");
        }

        [Fact]
        public async Task UpdateUserAsync_WhenNameIsUnique_ShouldUpdateUser()
        {
            _db.TblUsers.Add(new TblUser { Id = 5, Name = "OldName" });
            await _db.SaveChangesAsync();

            var model = new UserUpdateModel { Id = 5, Name = "Sara" };

            var result = await _service.UpdateUserAsync(model);

            result.Should().Be(5);

            var user = await _db.TblUsers.FindAsync(5);
            user!.Name.Should().Be("Sara");
        }

        [Fact]
        public async Task UpdateUserAsync_WhenNameExists_ShouldThrowArgumentException()
        {
            _db.TblUsers.AddRange(
                new TblUser { Id = 5, Name = "Sara" },
                new TblUser { Id = 6, Name = "Ali" }
            );
            await _db.SaveChangesAsync();

            var model = new UserUpdateModel { Id = 5, Name = "Ali" };

            var act = async () => await _service.UpdateUserAsync(model);

            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("نام کاربر تکراری است");
        }

        [Fact]
        public async Task ActiveUserAsync_ShouldSetIsDeletedFalse()
        {
            _db.TblUsers.Add(new TblUser { Id = 1, Name = "Ali", IsDeleted = true });
            await _db.SaveChangesAsync();

            var result = await _service.ActiveUserAsync(1);

            result.Should().Be(1);

            var user = await _db.TblUsers.FindAsync(1);
            user!.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeActiveUserAsync_ShouldSetIsDeletedTrue()
        {
            _db.TblUsers.Add(new TblUser { Id = 2, Name = "Sara", IsDeleted = false });
            await _db.SaveChangesAsync();

            var result = await _service.DeActiveUserAsync(2);

            result.Should().Be(2);

            var user = await _db.TblUsers.FindAsync(2);
            user!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUser()
        {
            _db.TblUsers.Add(new TblUser { Id = 3, Name = "Reza" });
            await _db.SaveChangesAsync();

            var result = await _service.DeleteUserAsync(3);

            result.Should().Be(3);

            var user = await _db.TblUsers.FindAsync(3);
            user.Should().BeNull();
        }
    }
}
