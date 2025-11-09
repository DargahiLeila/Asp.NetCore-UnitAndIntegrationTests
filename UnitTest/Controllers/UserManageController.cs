using Application.Services;
using Application.Services.Commands.User;
using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using Microsoft.AspNetCore.Mvc;
using UnitTest.ViewModel.User;

namespace UnitTest.Controllers
{
    public class UserManageController : Controller
    {
       
        private readonly IUserCommandService _commandService;
        private readonly IUserQueryService _queryService;
        public UserManageController(IUserCommandService commandService, IUserQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }



        public IActionResult Index(UserSearchModel sm)
        {
            return View(sm);
        }
        //[HttpGet]
        //public async Task<IActionResult> Index(UserSearchModel sm)
        //{
        //    // اگر داخل View داده‌ای نیاز به لود از دیتابیس داشته باشه، می‌تونی await بزنی
        //    return await Task.FromResult(View(sm));
        //}
        

        [HttpGet]
        public IActionResult ListAction(UserSearchModel sm)
        {
            return  ViewComponent("UserList", sm);
        }
      

        [HttpGet]
        public IActionResult SearchBoxAction(UserSearchModel sm)
        {
            return  ViewComponent("UserSearchBox", sm);
        }
       
        [HttpGet]
        public IActionResult Add()
        {
            return  ViewComponent("RegisterUser");
        }

       

        [HttpPost]
        public async Task<JsonResult> Add(UserAddEditViewModel usr)
        {
            try
            {
                var model = new UserAddModel
                {
                    Id = usr.Id,
                    Name = usr.Name,
                    IsDeleted = false
                };

                var insertedUserId = await _commandService.AddUserAsync(model);

                return Json(new
                {
                    success = true,
                    message = "کاربر با موفقیت ثبت شد",
                    id = insertedUserId
                });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "خطا در ثبت کاربر" });
            }
        }





        [HttpPost]
        public async Task<JsonResult> DeActiveUser(int id)
        {
            var deletedUserId = await _commandService.DeActiveUserAsync(id);

            return Json(new
            {
                success = true,
                message = "کاربر با موفقیت غیرفعال شد",
                id = deletedUserId
            });
        }


        [HttpPost]
        public async Task<JsonResult> ActiveUser(int id)
        {
            var deletedUserId = await _commandService.ActiveUserAsync(id);
            return Json(new { success = true, message = "کاربر با موفقیت فعال شد" /*,id = deletedUserId */});

        }
        [HttpGet]
        public IActionResult Update(int UserID)
        {

            return ViewComponent("UpdateUser", UserID);
        }


        [HttpPost]
        public async Task<JsonResult> Update(UserAddEditViewModel usr)
        {
            try
            {
                var model = new UserUpdateModel
                {
                    Id = usr.Id,
                    Name = usr.Name,
                };

                var updatedUserId = await _commandService.UpdateUserAsync(model);

                return Json(new
                {
                    success = true,
                    message = "کاربر با موفقیت ویرایش شد",
                    id = updatedUserId
                });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "خطا در ویرایش کاربر" });
            }
        }




    }
}
