using DomainModel.DTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Queries.User
{
    public interface IUserQueryService
    {

        Task<List<UserListItem>> GetAllUsersAsync();
        Task<(List<UserListItem> Users, int RecordCount)> SearchUsersAsync(UserSearchModel sm);
        Task<UserGetModel?> GetUserAsync(int id);
    }
}
