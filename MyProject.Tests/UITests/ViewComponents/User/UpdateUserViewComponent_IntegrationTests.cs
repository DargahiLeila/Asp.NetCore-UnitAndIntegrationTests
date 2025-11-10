using Application.Implements.User;
using DataAccess.Implements.User;
using DomainModel.DTO.UserModel;
using DomainModel.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.VeiwComponents.User;

namespace MyProject.Tests.UITests.ViewComponents.User
{
    public class UpdateUserViewComponent_IntegrationTests
    {
        public class UpdateUserViewComponentIntegrationTests
        {
            private readonly db_UnitTestContext _db;
            private readonly UpdateUserViewComponent _component;

            public UpdateUserViewComponentIntegrationTests()
            {
                var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique test DB
                    .Options;

                _db = new db_UnitTestContext(options);

                // Seed test data
                _db.Add(new TblUser
                {
                    Id = 5,
                    Name = "Sara",
                    IsDeleted = false
                });
                _db.SaveChanges();

                // Real repository and service
                var queryRepo = new UserQueryRepository(_db);
                var queryService = new UserQueryService(queryRepo);

                // Real component
                _component = new UpdateUserViewComponent(queryService);
            }

            [Fact]
            public async Task InvokeAsync_ShouldReturnViewWithUserModel()
            {
                // Act
                var result = await _component.InvokeAsync(5) as ViewViewComponentResult;

                // Assert
                result.Should().NotBeNull();
                result!.ViewData.Model.Should().BeOfType<UserGetModel>();
                var model = result.ViewData.Model as UserGetModel;
                model!.Id.Should().Be(5);
                model.Name.Should().Be("Sara");
                model.IsDeleted.Should().BeFalse();
            }
        }
    }
        
}
