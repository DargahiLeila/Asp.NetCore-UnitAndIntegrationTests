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
    public class UpdateUserViewComponentUnitTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldReturnViewWithUserModel()
        {
            // Arrange
            var mockQueryService = new Mock<IUserQueryService>();
            var expectedUser = new UserGetModel
            {
                Id = 5,
                Name = "Sara",
                IsDeleted = false
            };

            mockQueryService.Setup(q => q.GetUserAsync(5)).ReturnsAsync(expectedUser);

            var component = new UpdateUserViewComponent(mockQueryService.Object);

            // Act
            var result = await component.InvokeAsync(5) as ViewViewComponentResult;

            // Assert
            result.Should().NotBeNull();
            result!.ViewData.Model.Should().BeEquivalentTo(expectedUser);
        }
    }
}
