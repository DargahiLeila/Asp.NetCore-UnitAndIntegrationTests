using DomainModel.DTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Commands.User
{
    public interface IUserCommandRepository
    {
        Task<int> AddAsync(UserAddModel model);
        Task<int> UpdateAsync(UserUpdateModel model);
        Task<int> DeleteAsync(int id);
        Task<int> DeActiveUserAsync(int id);
        Task<int> ActiveUserAsync(int id);
        Task<bool> ExistNameAsync(string name);
        Task<bool> ExistNameAsync(string name, int userId);
    }
}
