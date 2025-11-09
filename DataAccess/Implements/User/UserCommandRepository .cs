using DataAccess.Services.Commands.User;
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
    public class UserCommandRepository : IUserCommandRepository
    {
        private readonly db_UnitTestContext db;
        public UserCommandRepository(db_UnitTestContext db)
        {
            this.db = db;
        }
       

public async Task<int> ActiveUserAsync(int id)
    {
        var usr = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (usr == null) return 0;

        usr.IsDeleted = false;
        await db.SaveChangesAsync();
        return usr.Id;
    }

    public async Task<int> AddAsync(UserAddModel model)
    {
        var usr = new TblUser
        {
            // اگر Id در دیتابیس Identity هست، این خط رو حذف کن
            //Id = model.Id,
            Name = model.Name,
            IsDeleted = model.IsDeleted
        };

        await db.AddAsync(usr);
        await db.SaveChangesAsync();
        return usr.Id;
    }

    public async Task<int> DeActiveUserAsync(int id)
    {
        var usr = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (usr == null) return 0;

        usr.IsDeleted = true;
        await db.SaveChangesAsync();
        return usr.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var usr = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (usr == null) return 0;

        usr.IsDeleted = true;
        await db.SaveChangesAsync();
        return usr.Id;
    }

    public async Task<bool> ExistNameAsync(string name)
    {
        return await db.TblUsers.AnyAsync(x => x.Name == name);
    }

    public async Task<bool> ExistNameAsync(string name, int userId)
    {
        return await db.TblUsers.AnyAsync(x => x.Name == name && x.Id != userId);
    }

    public async Task<int> UpdateAsync(UserUpdateModel model)
    {
        var oldUser = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (oldUser == null) return 0;

        oldUser.Name = model.Name;
        db.Update(oldUser);
        await db.SaveChangesAsync();
        return oldUser.Id;
    }
}
}
