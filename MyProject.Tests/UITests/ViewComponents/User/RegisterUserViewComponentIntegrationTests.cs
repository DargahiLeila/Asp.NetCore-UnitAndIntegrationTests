using Application.Implements.User;
using DataAccess.Implements.User;
using DomainModel.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
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
   
        public class RegisterUserViewComponentIntegrationTests
        {
            private readonly db_UnitTestContext _db;
            private readonly RegisterUserViewComponent _component;

            public RegisterUserViewComponentIntegrationTests()
            {
                var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                _db = new db_UnitTestContext(options);


                // Real repository and service
                var queryRepo = new UserQueryRepository(_db);
                var queryService = new UserQueryService(queryRepo);

                // Real component
                _component = new RegisterUserViewComponent(queryService);
            }

            [Fact]
            public void Invoke_ShouldReturnDefaultView()
            {
                // Act
                var result = _component.Invoke() as ViewViewComponentResult;
                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<ViewViewComponentResult>();

                var viewResult = result as ViewViewComponentResult;
                viewResult!.ViewName.Should().BeNull(); // چون View() بدون پارامتر صدا زده شده
            }
        }
    }

