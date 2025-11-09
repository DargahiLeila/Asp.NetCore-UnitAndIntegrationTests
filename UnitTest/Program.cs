using Microsoft.EntityFrameworkCore;
using DataAccess.Services;
using DataAccess.Implements;
using Application.Services;
using Application.Implements;
using DomainModel.Models;
using Application.Implements.User;
using Application.Services.Commands.User;
using Application.Services.Queries.User;
using DataAccess.Implements.User;
using DataAccess.Services.Commands.User;
using DataAccess.Services.Queries;

var builder = WebApplication.CreateBuilder(args);

// اتصال به دیتابیس
var strUnitTest = builder.Configuration["UnitTestConnectionString"];
builder.Services.AddDbContext<db_UnitTestContext>(options =>
    options.UseSqlServer(strUnitTest));

// ثبت سرویس‌ها
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();

// ثبت سرویس‌های CQRS

// DAL
builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();

// BLL
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware ها
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
