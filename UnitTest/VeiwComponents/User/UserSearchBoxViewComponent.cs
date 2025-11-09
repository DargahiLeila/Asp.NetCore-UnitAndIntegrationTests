using Application.Services;
using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnitTest.VeiwComponents.User
{
    [ViewComponent(Name = "UserSearchBox")]
    public class UserSearchBoxViewComponent: ViewComponent
    {
        //private readonly IUserService UserSrv;
        private readonly IUserQueryService _queryService;

        public UserSearchBoxViewComponent(IUserQueryService queryService)
        {
            this._queryService = queryService;
        }
        
        public IViewComponentResult Invoke(UserSearchModel sm)
        {
            return View(sm);
        }
    }
}
