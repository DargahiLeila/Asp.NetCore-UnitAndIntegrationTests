using Application.Services;
using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnitTest.VeiwComponents.User
{
    [ViewComponent(Name = "UpdateUser")]
    public class UpdateUserViewComponent:ViewComponent
    {
        //private readonly IUserService _srv;
        private readonly IUserQueryService _queryService;
        public UpdateUserViewComponent(IUserQueryService queryService)
        {
            this._queryService = queryService;  
        }
        public async Task< IViewComponentResult> InvokeAsync( int UserId)
        {
            var usr = await _queryService.GetUserAsync(UserId);
            return View(usr);
        }
    }
}
