using Application.Services;
using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using Microsoft.AspNetCore.Mvc;
using UnitTest.Models.User;

namespace UnitTest.VeiwComponents.User
{
    [ViewComponent(Name = "UserList")]
    public class UserListViewComponent:ViewComponent
    {
        //private readonly IUserService UserSrv;
        private readonly Application.Services.Queries.User.IUserQueryService _queryService;
        public UserListViewComponent(IUserQueryService queryService)
        {
            this._queryService = queryService;
        }
        public async Task <IViewComponentResult> InvokeAsync(UserSearchModel sm)
        {
            //int rc = 0;
            var usrs = await _queryService.SearchUsersAsync(sm);
            UserListAndSearchModel hsm = new UserListAndSearchModel { sm = sm, UserListItems = usrs.Users };
            sm.RecordCount = usrs.RecordCount;
            //sm.RecordCount = rc;
            if (sm.PageSize == 0)
            {
                sm.PageSize = 10;
            }
            if (sm.RecordCount % sm.PageSize == 0)
            {
                sm.PageCount = sm.RecordCount / sm.PageSize;
            }
            else
            {
                sm.PageCount = sm.RecordCount / sm.PageSize + 1;

            }
            return View(hsm);
        }
    }
}
