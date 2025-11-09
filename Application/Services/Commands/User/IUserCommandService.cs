using DomainModel.DTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Commands.User
{
    public interface IUserCommandService
    {
        Task<int> AddUserAsync(UserAddModel model);
        Task<int> UpdateUserAsync(UserUpdateModel model);
        Task<int> DeleteUserAsync(int id);
        Task<int> DeActiveUserAsync(int id);
        Task<int> ActiveUserAsync(int id);
    }
}
