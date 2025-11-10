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

namespace MyProject.Tests.ApplicationTests.Queries.User
{
    public class UserQueryServiceIntegrationTests
    {

        private readonly db_UnitTestContext _db;
        private readonly UserQueryService _service;

        public UserQueryServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new db_UnitTestContext(options);
            var repo = new UserQueryRepository(_db);
            _service = new UserQueryService(repo);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            _db.TblUsers.AddRange(
                new TblUser { Id = 1, Name = "Ali" },
                new TblUser { Id = 2, Name = "Sara" }
            );
            await _db.SaveChangesAsync();

            var result = await _service.GetAllUsersAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(u => u.Name == "Ali");
            result.Should().Contain(u => u.Name == "Sara");
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUserById()
        {
            _db.TblUsers.Add(new TblUser { Id = 5, Name = "Reza", IsDeleted = false });
            await _db.SaveChangesAsync();

            var result = await _service.GetUserAsync(5);

            result.Should().NotBeNull();
            result!.Id.Should().Be(5);
            result.Name.Should().Be("Reza");
            result.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task SearchUsersAsync_ShouldReturnUsersAndRecordCount()
        {
            _db.TblUsers.AddRange(
                new TblUser { Id = 1, Name = "Ali" },
                new TblUser { Id = 2, Name = "Sara" },
                new TblUser { Id = 3, Name = "Ali" }
            );
            await _db.SaveChangesAsync();

            var searchModel = new UserSearchModel { Name = "Ali" };

            var (resultUsers, resultCount) = await _service.SearchUsersAsync(searchModel);

            resultUsers.Should().HaveCount(2);
            resultUsers.Should().AllSatisfy(u => u.Name.Should().Be("Ali"));
            resultCount.Should().Be(2);
        }
    }
}
