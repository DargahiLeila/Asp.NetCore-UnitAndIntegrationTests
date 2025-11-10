using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using FluentAssertions;
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
    public class UserSearchBoxViewComponentUnitTests
    {
        [Fact]
        public void Invoke_WhenCalled_ShouldReturnViewWithSearchModel()
        {
            // Arrange
            
            var queryServiceMock = new Mock<IUserQueryService>();

            var searchModel = new UserSearchModel
            {
                Name = "Ali",
                PageSize = 5,
                PageIndex = 1
            };

            var component = new UserSearchBoxViewComponent(queryServiceMock.Object);

            // Act
            var result = component.Invoke(searchModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewViewComponentResult>();

            var viewResult = result as ViewViewComponentResult;
            viewResult!.ViewName.Should().BeNull(); // چون View() بدون اسم صدا زده شده
            viewResult.ViewData.Model.Should().BeEquivalentTo(searchModel);
        }
    }
}
