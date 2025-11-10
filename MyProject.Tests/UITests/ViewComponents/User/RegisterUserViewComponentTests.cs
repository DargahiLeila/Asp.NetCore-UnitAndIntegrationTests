using Application.Services.Commands.User;
using Application.Services.Queries.User;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.VeiwComponents.User;

namespace MyProject.Tests.UITests.ViewComponents.User
{
    public class RegisterUserViewComponentTests
    {
        [Fact]
        public void Invoke_ShouldReturnDefaultView()
        {
            // Arrange
            var queryServiceMock = new Mock<IUserQueryService>();
            var component = new RegisterUserViewComponent(queryServiceMock.Object);

            // Act
            var result = component.Invoke();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewViewComponentResult>();

            var viewResult = result as ViewViewComponentResult;
            viewResult!.ViewName.Should().BeNull(); // چون View() بدون پارامتر صدا زده شده
        }


        //example--> if DropDown exist in View
        //[Fact]
        //public async Task InvokeAsync_ShouldSetRoleListInViewBag()
        //{
        //    // Arrange
        //    var mockQuery = new Mock<IUserQueryService>();
        //    var roles = new List<RoleDto>
        //{
        //    new RoleDto { Id = 1, Title = "Admin" },
        //    new RoleDto { Id = 2, Title = "User" }
        //};

        //    mockQuery.Setup(q => q.GetRolesAsync()).ReturnsAsync(roles);

        //    var component = new RegisterUserViewComponent(mockQuery.Object);

        //    // Act
        //    var result = await component.InvokeAsync() as ViewViewComponentResult;

        //    // Assert
        //    result.Should().NotBeNull();
        //    component.ViewBag.RoleList.Should().NotBeNull();
        //    var selectList = component.ViewBag.RoleList as SelectList;
        //    selectList.Should().NotBeNull();
        //    selectList!.Count().Should().Be(2);
        //}
    }
}
