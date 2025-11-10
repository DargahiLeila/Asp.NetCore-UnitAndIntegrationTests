using Application.Implements.User;
using Application.Services.Commands.User;
using Application.Services.Queries.User;
using DataAccess.Implements.User;
using DomainModel.DTO.UserModel;
using DomainModel.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class UserManageControllerIntegrationTests
    {

        private readonly db_UnitTestContext _db;
        private readonly UserManageController _controller;

        public UserManageControllerIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // دیتابیس تستی منحصر به‌فرد
                .Options;

            _db = new db_UnitTestContext(options);

            // ساخت ریپازیتوری و سرویس واقعی
            var commandRepo = new UserCommandRepository(_db);
            var queryRepo = new UserQueryRepository(_db);

            var commandService = new UserCommandService(commandRepo);
            var queryService = new UserQueryService(queryRepo);

            // ساخت کنترلر با سرویس‌های واقعی
            _controller = new UserManageController(commandService, queryService);
        }
        [Fact]
        public void Index_ShouldReturnViewWithModel()
        {
            var sm = new UserSearchModel { Name = "Ali" };

            var result = _controller.Index(sm) as ViewResult;

            result.Should().NotBeNull();
            result!.Model.Should().BeOfType<UserSearchModel>();
            var model = result.Model as UserSearchModel;
            model!.Name.Should().Be("Ali");
        }
        [Fact]
        public void ListAction_ShouldReturnViewComponent()
        {
            var sm = new UserSearchModel { Name = "Ali" };

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

            var result = await _controller.Add(vm) as JsonResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult;

            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.message.Should().Be("کاربر با موفقیت ثبت شد");
            data.id.Should().BeGreaterThan(0);
            var allUsers = _db.TblUsers.ToList();
            var userInDb = await _db.TblUsers.FindAsync(data.id);
            userInDb.Should().NotBeNull();
            userInDb!.Name.Should().Be("Ali");
        }

        [Fact]
        public async Task Add_Post_WhenDuplicateName_ShouldReturnErrorJson()
        {
            // ثبت اولیه کاربر با نام "Ali"
            _db.TblUsers.Add(new TblUser { Name = "Ali" });
            await _db.SaveChangesAsync();

            var vm = new UserAddEditViewModel { Name = "Ali" };

            var result = await _controller.Add(vm) as JsonResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult;

            data.Should().NotBeNull();
            data!.success.Should().BeFalse();
            data.message.Should().Be("نام کاربر تکراری است");
        }

        [Fact]
        public async Task DeActiveUser_WhenValid_ShouldReturnSuccessJson()
        {
            _db.TblUsers.Add(new TblUser { Id = 10, Name = "Ali", IsDeleted = false });
            await _db.SaveChangesAsync();

            var result = await _controller.DeActiveUser(10) as JsonResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult;

            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(10);

            var user = await _db.TblUsers.FindAsync(10);
            user!.IsDeleted.Should().BeTrue();
        }
        [Fact]
        public async Task ActiveUser_WhenValid_ShouldReturnSuccessJson()
        {
            _db.TblUsers.Add(new TblUser { Id = 20, Name = "Sara", IsDeleted = true });
            await _db.SaveChangesAsync();

            var result = await _controller.ActiveUser(20) as JsonResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult;

            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(20);
            data.message.Should().Be("کاربر با موفقیت فعال شد");

            var user = await _db.TblUsers.FindAsync(20);
            user!.IsDeleted.Should().BeFalse();
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
            _db.TblUsers.Add(new TblUser { Id = 5, Name = "OldName", IsDeleted = false });
            await _db.SaveChangesAsync();

            var vm = new UserAddEditViewModel { Id = 5, Name = "Sara" };

            var result = await _controller.Update(vm) as JsonResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult;

            data.Should().NotBeNull();
            data!.success.Should().BeTrue();
            data.id.Should().Be(5);
            data.message.Should().Be("کاربر با موفقیت ویرایش شد");

            var user = await _db.TblUsers.FindAsync(5);
            user!.Name.Should().Be("Sara");
        }
        [Fact]
        public async Task Update_Post_WhenDuplicateName_ShouldReturnErrorJson()
        {
            _db.TblUsers.AddRange(
                new TblUser { Id = 5, Name = "Sara" },
                new TblUser { Id = 6, Name = "Ali" }
            );
            await _db.SaveChangesAsync();

            var vm = new UserAddEditViewModel { Id = 5, Name = "Ali" }; // تلاش برای تغییر نام به نام تکراری

            var result = await _controller.Update(vm) as JsonResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult;

            data.Should().NotBeNull();
            data!.success.Should().BeFalse();
            data.message.Should().Be("نام کاربر تکراری است");
        }
    }


}

