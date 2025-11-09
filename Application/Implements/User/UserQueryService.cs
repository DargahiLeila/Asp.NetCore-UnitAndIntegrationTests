using Application.Services.Queries.User;
using DataAccess.Services;
using DataAccess.Services.Queries;
using DomainModel.DTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.User
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserQueryRepository repo;
        public UserQueryService(IUserQueryRepository repo)
        {
            this.repo = repo;
        }
        public async Task<List<UserListItem>> GetAllUsersAsync()
        {
            return await repo.GetAllAsync();
        }

        public async Task<UserGetModel?> GetUserAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(List<UserListItem> Users, int RecordCount)> SearchUsersAsync(UserSearchModel sm)
        {
            return await repo.SearchAsync(sm);
        }
    }
}
