using DataAccess.Services.Queries;
using DomainModel.DTO.UserModel;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Implements.User
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly db_UnitTestContext db;
        public UserQueryRepository(db_UnitTestContext db)
        {
            this.db = db;
        }


        public async Task<TblUser?> GetAsync(int id)
        {
            return await db.TblUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UserListItem>> GetAllAsync()
        {
            return await db.TblUsers
                .Where(x => !x.IsDeleted)
                .Select(x => new UserListItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsDeleted = x.IsDeleted
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<UserGetModel?> GetByIdAsync(int id)
        {
            var usr = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (usr == null) return null;

            return new UserGetModel
            {
                Id = usr.Id,
                Name = usr.Name,
                IsDeleted = usr.IsDeleted
            };
        }
        public List<UserListItem> Search(UserSearchModel sm, out int RecordCount)
        {
            if (sm.PageSize == 0)
            {
                sm.PageSize = 10;
            }
            var q = from item in db.TblUsers select item;
            
            if (!string.IsNullOrEmpty(sm.Name))
            {
                q = q.Where(x => x.Name.StartsWith(sm.Name));
            }
            
            RecordCount = q.Count();

            return q.Select(x => new UserListItem
            {
                Id = x.Id,
                Name = x.Name,
            }).OrderByDescending(x => x.Id).Skip(sm.PageIndex * sm.PageSize).Take(sm.PageSize).ToList();
        }

        
        public async Task<(List<UserListItem> Users, int RecordCount)> SearchAsync(UserSearchModel sm)
        {
            if (sm.PageSize == 0)
                sm.PageSize = 10;

            var q = db.TblUsers
                     /* .Where(x => !x.IsDeleted) */
                      .AsQueryable();

            if (!string.IsNullOrEmpty(sm.Name))
                q = q.Where(x => x.Name.StartsWith(sm.Name));

            var recordCount = await q.CountAsync();

            var users = await q.Select(x => new UserListItem
            {
                Id = x.Id,
                Name = x.Name,
                IsDeleted = x.IsDeleted
            })
            .OrderByDescending(x => x.Id)
            .Skip(sm.PageIndex * sm.PageSize)
            .Take(sm.PageSize)
            .ToListAsync();

            return (users, recordCount);
        }
    }

}

