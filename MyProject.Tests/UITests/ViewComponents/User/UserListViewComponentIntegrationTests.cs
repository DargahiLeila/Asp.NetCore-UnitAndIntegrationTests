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
using UnitTest.Models.User;
using UnitTest.VeiwComponents.User;

namespace MyProject.Tests.UITests.ViewComponents.User
{
    public class UserListViewComponentIntegrationTests
    {
 
            private readonly db_UnitTestContext _db;
            private readonly UserListViewComponent _component;

            public UserListViewComponentIntegrationTests()
            {
                var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique test DB
                    .Options;

                _db = new db_UnitTestContext(options);

                // Seed test data
                _db.TblUsers.AddRange(
                    new TblUser { Id = 1, Name = "Ali", IsDeleted = false },
                    new TblUser { Id = 2, Name = "Sara", IsDeleted = false }
                );
                _db.SaveChanges();

                // Real repository and service
                var queryRepo = new UserQueryRepository(_db);
                var queryService = new UserQueryService(queryRepo);

                // Real component
                _component = new UserListViewComponent(queryService);
            }

            [Fact]
            public async Task InvokeAsync_ShouldReturnViewWithUserListAndPaging()
            {
                // Arrange
                var searchModel = new UserSearchModel { PageSize = 0 };

                // Act
                var result = await _component.InvokeAsync(searchModel) as ViewViewComponentResult;

                // Assert
                result.Should().NotBeNull();
                var model = result!.ViewData.Model as UserListAndSearchModel;

                model.Should().NotBeNull();
                model!.UserListItems.Should().HaveCount(2);

                // Sort result to ensure consistent order
                var sorted = model.UserListItems.OrderBy(u => u.Id).ToList();

                sorted[0].Name.Should().Be("Ali");
                sorted[1].Name.Should().Be("Sara");

                searchModel.PageSize.Should().Be(10); // default set in component
                searchModel.RecordCount.Should().Be(2);
                searchModel.PageCount.Should().Be(1); // 2 / 10 = 0.2 → PageCount = 1
            }
        }
}
