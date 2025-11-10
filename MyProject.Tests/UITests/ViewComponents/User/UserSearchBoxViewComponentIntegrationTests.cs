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
    public class UserSearchBoxViewComponentIntegrationTests
    {
     
            private readonly db_UnitTestContext _db;
            private readonly UserSearchBoxViewComponent _component;

            public UserSearchBoxViewComponentIntegrationTests()
            {
                var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                _db = new db_UnitTestContext(options);

                // Real repository and service
                var queryRepo = new UserQueryRepository(_db);
                var queryService = new UserQueryService(queryRepo);

                // Real component
                _component = new UserSearchBoxViewComponent(queryService);
            }

            [Fact]
            public void Invoke_ShouldReturnViewWithSearchModel()
            {
                // Arrange
                var searchModel = new UserSearchModel
                {
                    Name = "Ali",
                    PageSize = 5,
                    PageIndex = 1
                };

                // Act
                var result = _component.Invoke(searchModel);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<ViewViewComponentResult>();

                var viewResult = result as ViewViewComponentResult;
                viewResult!.ViewName.Should().BeNull(); // View() called without name
                viewResult.ViewData.Model.Should().BeEquivalentTo(searchModel);
            }
        }
}
