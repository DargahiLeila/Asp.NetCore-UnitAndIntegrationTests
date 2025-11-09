using Application.Services;
using Application.Services.Queries.User;
using DomainModel.DTO.UserModel;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest.VeiwComponents.User
{
    [ViewComponent(Name = "RegisterUser")]
    public class RegisterUserViewComponent:ViewComponent
    {
        //private readonly IUserService UserSrv;
        private readonly IUserQueryService UserSrv;
        
        public RegisterUserViewComponent(IUserQueryService UserSrv)
        {
            this.UserSrv = UserSrv;
        }

        public IViewComponentResult Invoke()
        {
            return  View();
        }
    }
}
