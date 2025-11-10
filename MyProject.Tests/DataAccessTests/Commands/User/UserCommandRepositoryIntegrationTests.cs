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

namespace MyProject.Tests.DataAccessTests.Commands.User
{
    public class UserCommandRepositoryIntegrationTests
    {

        private readonly db_UnitTestContext db;
        private readonly UserCommandRepository repo;

        public UserCommandRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            db = new db_UnitTestContext(options);
            repo = new UserCommandRepository(db);
        }
        

        [Fact]
        public async Task AddAsync_ShouldAddUserAndReturnId()
        {
            var model = new UserAddModel { Name = "Ali", IsDeleted = false };

            var id = await repo.AddAsync(model);

            id.Should().BeGreaterThan(0);
            var user = await db.TblUsers.FindAsync(id);
            user.Should().NotBeNull();
            user!.Name.Should().Be("Ali");
        }

        [Fact]
        public async Task ActiveUserAsync_ShouldSetIsDeletedFalse()
        {
            
            var user = new TblUser { Name = "Sara", IsDeleted = true };
            db.TblUsers.Add(user);
            await db.SaveChangesAsync();

            
            var result = await repo.ActiveUserAsync(user.Id);

            result.Should().Be(user.Id);
            var updated = await db.TblUsers.FindAsync(user.Id);
            updated!.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeActiveUserAsync_ShouldSetIsDeletedTrue()
        {
          
            var user = new TblUser { Name = "Reza", IsDeleted = false };
            db.TblUsers.Add(user);
            await db.SaveChangesAsync();

           
            var result = await repo.DeActiveUserAsync(user.Id);

            result.Should().Be(user.Id);
            var updated = await db.TblUsers.FindAsync(user.Id);
            updated!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task ExistNameAsync_ShouldReturnTrueIfExists()
        {
          
            db.TblUsers.Add(new TblUser { Name = "Ali" });
            await db.SaveChangesAsync();

          
            var exists = await repo.ExistNameAsync("Ali");

            exists.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUserName()
        {
           
            var user = new TblUser { Name = "OldName" };
            db.TblUsers.Add(user);
            await db.SaveChangesAsync();

           
            var model = new UserUpdateModel { Id = user.Id, Name = "NewName" };

            var result = await repo.UpdateAsync(model);

            result.Should().Be(user.Id);
            var updated = await db.TblUsers.FindAsync(user.Id);
            updated!.Name.Should().Be("NewName");
        }
    }
}
