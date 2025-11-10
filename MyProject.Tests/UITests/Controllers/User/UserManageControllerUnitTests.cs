using Application.Services.Commands.User;
using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Controllers;
using UnitTest.DTOS;
using UnitTest.ViewModel.User;

namespace MyProject.Tests.UITests.Controllers.User
{
    public class UserManageControllerUnitTests
    {
        private readonly Mock<IUserCommandService> _commandMock;
        private readonly Mock<IUserQueryService> _queryMock;
        private readonly UserManageController _controller;

        public UserManageControllerUnitTests()
        {
            _commandMock = new Mock<IUserCommandService>();
            _queryMock = new Mock<IUserQueryService>();
            _controller = new UserManageController(_commandMock.Object, _queryMock.Object);
        }

        [Fact]
        public void Index_ShouldReturnViewWithModel()
        {
            var sm = new UserSearchModel { Name = "Ali" };

            var result = _controller.Index(sm) as ViewResult;

            result.Should().NotBeNull();
            result!.Model.Should().Be(sm);
        }

        [Fact]
        public void ListAction_ShouldReturnViewComponent()
        {
            var sm = new UserSearchModel();

            var result = _controller.ListAction(sm) as ViewComponentResult;

            result.Should().NotBeNull();
            result!.ViewComponentName.Should().Be("UserList");
            result.Arguments.Should().Be(sm);
        }

        [Fact]
        public void SearchBoxAction_ShouldReturnViewComponent()
        {
            var sm = new UserSearchModel();

            var result = _controller.SearchBoxAction(sm) as ViewComponentResult;

            result.Should().NotBeNull();
            result!.ViewComponentName.Should().Be("UserSearchBox");
            result.Arguments.Should().Be(sm);
        }


        [Fact]
        public void Add_Get_ShouldReturnRegisterUserViewComponent()
        {
            var result = _controller.Add() as ViewComponentResult;

            result.Should().NotBeNull();
            result!.ViewComponentName.Should().Be("RegisterUser");
        }

        [Fact]
        public async Task Add_Post_WhenValid_ShouldReturnSuccessJson()
        {
            var vm = new UserAddEditViewModel { Name = "Ali" };
            _commandMock.Setup(c => c.AddUserAsync(It.IsAny<UserAddModel>())).ReturnsAsync(1);

            var result = await _controller.Add(vm) as JsonResult;

            var data = result!.Value as OperationResult;
            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(1);
            data.message.Should().Be("کاربر با موفقیت ثبت شد");
        }

        [Fact]
        public async Task Add_Post_WhenDuplicateName_ShouldReturnErrorJson()
        {
            var vm = new UserAddEditViewModel { Name = "Ali" };
            _commandMock.Setup(c => c.AddUserAsync(It.IsAny<UserAddModel>()))
                        .ThrowsAsync(new ArgumentException("نام کاربر تکراری است"));

            var result = await _controller.Add(vm) as JsonResult;

            var data = result!.Value as OperationResult;
            data.Should().NotBeNull();
            data!.success.Should().BeFalse();
            data.message.Should().Be("نام کاربر تکراری است");
        }

        [Fact]
        public async Task DeActiveUser_WhenValid_ShouldReturnSuccessJson()
        {
            _commandMock.Setup(c => c.DeActiveUserAsync(10)).ReturnsAsync(10);

            var result = await _controller.DeActiveUser(10) as JsonResult;

            var data = result!.Value as OperationResult;
            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(10);
        }

        [Fact]
        public async Task ActiveUser_WhenValid_ShouldReturnSuccessJson()
        {
            _commandMock.Setup(c => c.ActiveUserAsync(20)).ReturnsAsync(20);

            var result = await _controller.ActiveUser(20) as JsonResult;

            var data = result!.Value as OperationResult;
            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(20);
            data.message.Should().Be("کاربر با موفقیت فعال شد");
        }

        [Fact]
        public void Update_Get_ShouldReturnUpdateUserViewComponent()
        {
            var result = _controller.Update(5) as ViewComponentResult;

            result.Should().NotBeNull();
            result!.ViewComponentName.Should().Be("UpdateUser");
            result.Arguments.Should().Be(5);
        }

        [Fact]
        public async Task Update_Post_WhenValid_ShouldReturnSuccessJson()
        {
            var vm = new UserAddEditViewModel { Id = 5, Name = "Sara" };
            _commandMock.Setup(c => c.UpdateUserAsync(It.IsAny<UserUpdateModel>())).ReturnsAsync(5);

            var result = await _controller.Update(vm) as JsonResult;

            var data = result!.Value as OperationResult;
            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(5);
            data.message.Should().Be("کاربر با موفقیت ویرایش شد");
        }

        [Fact]
        public async Task Update_Post_WhenDuplicateName_ShouldReturnErrorJson()
        {
            var vm = new UserAddEditViewModel { Id = 5, Name = "Sara" };
            _commandMock.Setup(c => c.UpdateUserAsync(It.IsAny<UserUpdateModel>()))
                        .ThrowsAsync(new ArgumentException("نام کاربر تکراری است"));

            var result = await _controller.Update(vm) as JsonResult;

            var data = result!.Value as OperationResult;
            data.Should().NotBeNull();
            data!.success.Should().BeFalse();
            data.message.Should().Be("نام کاربر تکراری است");
        }
    }
}
