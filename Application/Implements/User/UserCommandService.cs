using Application.Services.Commands.User;
using DataAccess.Services.Commands.User;
using DomainModel.DTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.User
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserCommandRepository repo;

        public UserCommandService(IUserCommandRepository repo)
        {
           this. repo = repo;
        }
        public async Task< int> ActiveUserAsync(int id)
        {
            return await repo.ActiveUserAsync(id);
        }

        public async Task<int> AddUserAsync(UserAddModel model)
        {
            if (await repo.ExistNameAsync(model.Name))
                throw new ArgumentException("نام کاربر تکراری است");
            return await repo.AddAsync(model);
        }

        public async Task<int> DeActiveUserAsync(int id)
        {
            return await repo.DeActiveUserAsync(id);
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            return await repo.DeleteAsync(id);
        }

        public async Task<int> UpdateUserAsync(UserUpdateModel model)
        {

            if ( await repo.ExistNameAsync(model.Name, model.Id))
                throw new ArgumentException("نام کاربر تکراری است");
            return await repo.UpdateAsync(model);
        }
    }
}
