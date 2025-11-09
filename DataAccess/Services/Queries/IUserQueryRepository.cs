using DomainModel.DTO.UserModel;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Queries
{
    public interface IUserQueryRepository
    {

        Task<List<UserListItem>> GetAllAsync();
        Task<(List<UserListItem> Users, int RecordCount)> SearchAsync(UserSearchModel sm);
        Task<UserGetModel?> GetByIdAsync(int id);
        Task<TblUser?> GetAsync(int id);
    }
}
