using DataAccess.Implements.User;
using DomainModel.DTO.UserModel;
using DomainModel.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Tests.DataAccessTests.Queries.User
{
    public class UserQueryRepositoryIntegrationTests
    {
        private readonly db_UnitTestContext db;
        private readonly UserQueryRepository repo;

        public UserQueryRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<db_UnitTestContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            db = new db_UnitTestContext(options);
            repo = new UserQueryRepository(db);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnUserById()
        {
          
            var user = new TblUser { Name = "Ali", IsDeleted = false };
            db.TblUsers.Add(user);
            await db.SaveChangesAsync();

            
            var result = await repo.GetAsync(user.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Ali");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveUsers()
        {
          
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali", IsDeleted = false },
                new TblUser { Name = "Sara", IsDeleted = true }
            );
            await db.SaveChangesAsync();

        
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Ali");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedModel()
        {
           
            var user = new TblUser { Name = "Reza", IsDeleted = false };
            db.TblUsers.Add(user);
            await db.SaveChangesAsync();

        
            var result = await repo.GetByIdAsync(user.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Reza");
            result.IsDeleted.Should().BeFalse();
        }
        //این تست دقت صفحه بندی رو بررسی میکنه
        [Fact]
        public void Search_ShouldReturnPagedAndFilteredResults()
        {
           
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "AliReza" },
                new TblUser { Name = "Sara" }
            );
            db.SaveChanges();

           
            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 1 };

            var result = repo.Search(sm, out int count);//خروجی این متد یک لیست و یک outint count هست

            count.Should().Be(2);//چون دو تا رکورد با علی شروع شده پس کانت باید بشه 2
            result.Should().HaveCount(1);//چون سایز صفحه رو گفتیم یک باشه پس باید یک رکورد از دو رکورد پیدا شده نمایش داده بشه
            result[0].Name.Should().StartWith("Ali");
        }
        //این تست می‌خواد بررسی کنه که فیلتر کردن درست انجام شده یا نه
        [Fact]
        public async Task SearchAsync_ShouldReturnPagedAndFilteredResults()
        {
           
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "AliReza" },
                new TblUser { Name = "Sara" }
            );
            await db.SaveChangesAsync();

            
            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 2 };

            var (users, count) = await repo.SearchAsync(sm);// خروجی این متد یک تاپل هستش که لیست کاربران و تعدادشون رو برمگردونه

            count.Should().Be(2);
            users.Should().HaveCount(2);//چون سایز صفحه رو گفتیم 2 باشه پس دو تا رکورد پیدا شده رو نشون میده
            users.All(u => u.Name.StartsWith("Ali")).Should().BeTrue();
        }
        //تست برای صفحه دوم یعنی pageIndex=1
        [Fact]
        public void Search_ShouldReturnSecondPage()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali1" },
                new TblUser { Name = "Ali2" },
                new TblUser { Name = "Ali3" }
            );
            db.SaveChanges();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 1, PageSize = 2 };

            var result = repo.Search(sm, out int count);

            count.Should().Be(3);
            result.Should().HaveCount(1); // چون صفحه دوم فقط یک آیتم باقی مونده
            result[0].Name.Should().StartWith("Ali");
        }
        //تست: PageSize = 0 (باید پیش‌فرض 10 شود)
        [Fact]
        public async Task SearchAsync_ShouldDefaultPageSizeTo10()
        {
            db.TblUsers.AddRange(
                Enumerable.Range(1, 15).Select(i => new TblUser { Name = $"Ali{i}" })
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 0 };

            var (users, count) = await repo.SearchAsync(sm);

            count.Should().Be(15);
            users.Should().HaveCount(10); // چون PageSize صفر بود و باید 10 شود
        }
        //تست: Name = null (بدون فیلتر)
        [Fact]
        public async Task SearchAsync_ShouldReturnAllWhenNameIsNull()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "Sara" }
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = null, PageIndex = 0, PageSize = 10 };

            var (users, count) = await repo.SearchAsync(sm);

            count.Should().Be(2);
            users.Should().HaveCount(2);
        }
        //تست: Name = "" (رشته خالی)
        [Fact]
        public void Search_ShouldReturnAllWhenNameIsEmpty()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "Sara" }
            );
            db.SaveChanges();

            var sm = new UserSearchModel { Name = "", PageIndex = 0, PageSize = 10 };

            var result = repo.Search(sm, out int count);

            count.Should().Be(2);
            result.Should().HaveCount(2);
        }
        //تست: وقتی هیچ کاربری پیدا نمی‌شود
        [Fact]
        public void Search_ShouldReturnEmptyList_WhenNoMatchFound()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Sara" },
                new TblUser { Name = "Reza" }
            );
            db.SaveChanges();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 10 };

            var result = repo.Search(sm, out int count);

            count.Should().Be(0);
            result.Should().BeEmpty();
        }
        //تست: SearchAsync وقتی هیچ نتیجه‌ای نیست
        [Fact]
        public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatchFound()
        {
            //Arrange
            db.TblUsers.AddRange(
                new TblUser { Name = "Sara" },
                new TblUser { Name = "Reza" }
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 10 };
            //Act
            var (users, count) = await repo.SearchAsync(sm);
            //Assert
            count.Should().Be(0);
            users.Should().BeEmpty();
        }
        //تست: ورودی نامعتبر (PageIndex منفی)
        [Fact]
        public void Search_ShouldHandleNegativePageIndex()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "AliReza" }
            );
            db.SaveChanges();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = -1, PageSize = 1 };

            var result = repo.Search(sm, out int count);

            count.Should().Be(2);
            result.Should().HaveCount(1); // چون Skip(-1 * 1) = Skip(0)
        }

        //تست: ورودی نامعتبر (PageSize منفی)
        [Fact]
        public async Task SearchAsync_ShouldDefaultTo10_WhenPageSizeIsNegative()
        {
            db.TblUsers.AddRange(
                Enumerable.Range(1, 12).Select(i => new TblUser { Name = $"Ali{i}" })
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = -5 };

            var (users, count) = await repo.SearchAsync(sm);

            count.Should().Be(12);
            users.Should().HaveCount(10); // چون PageSize منفی بوده و باید به 10 تنظیم بشه
        }

        //تست: Name = "ali" (بررسی حساسیت به حروف بزرگ و کوچک)
        [Fact]
        public async Task SearchAsync_ShouldBeCaseSensitive()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "ali" }
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 10 };

            var (users, count) = await repo.SearchAsync(sm);

            count.Should().Be(1);
            users[0].Name.Should().Be("Ali");
        }

        //تست: PageIndex خارج از محدوده (صفحه‌ای که داده ندارد)
        [Fact]
        public async Task SearchAsync_ShouldReturnEmpty_WhenPageIndexTooHigh()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "AliReza" }
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 5, PageSize = 2 };

            var (users, count) = await repo.SearchAsync(sm);

            count.Should().Be(2);
            users.Should().BeEmpty(); // چون صفحه پنجم هیچ داده‌ای نداره
        }
        //تست: PageSize خیلی بزرگ (بیشتر از تعداد رکوردها)
        [Fact]
        public void Search_ShouldReturnAll_WhenPageSizeTooLarge()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "AliReza" }
            );
            db.SaveChanges();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = 0, PageSize = 100 };

            var result = repo.Search(sm, out int count);

            count.Should().Be(2);
            result.Should().HaveCount(2); // چون PageSize بزرگ‌تر از تعداد واقعی رکوردهاست
        }
        //تست: PageSize و PageIndex هر دو نامعتبر
        [Fact]
        public async Task SearchAsync_ShouldHandleNegativeValuesGracefully()
        {
            db.TblUsers.AddRange(
                new TblUser { Name = "Ali" },
                new TblUser { Name = "AliReza" }
            );
            await db.SaveChangesAsync();

            var sm = new UserSearchModel { Name = "Ali", PageIndex = -2, PageSize = -10 };

            var (users, count) = await repo.SearchAsync(sm);

            count.Should().Be(2);
            users.Should().HaveCount(2); // چون PageSize منفی بوده و به 10 تنظیم شده، و PageIndex منفی به 0 تبدیل شده
        }


    }
}
