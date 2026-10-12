using Microsoft.EntityFrameworkCore;
using mvc_Dapper.Models.Conn;
using mvc_Dapper.Models.Repo;
using mvc_Dapper.Models.Repository;
using mvc_Dapper.Models.Service;
using mvc_Dapper.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<mvcstoreContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("mvcstore"));
    options.EnableDetailedErrors(true);
});

builder.Services.AddScoped<IZSysProgramRepo,ZSysProgramRepo>();
builder.Services.AddScoped<IZSysProgramService, ZSysProgramService>();
builder.Services.AddScoped<IZOrgUserRepo,ZOrgUserRepo>();
builder.Services.AddScoped<IZSysGroupRepo, ZSysGroupRepo>();
builder.Services.AddScoped<IZSysSecurityRepo, ZSysSecurityRepo>();
builder.Services.AddScoped<IPoinRepository,PoinRepository>();
builder.Services.AddScoped<IZDemoCustomerRepo, ZDemoCustomerRepo>();
builder.Services.AddScoped<IZDemoVendorRepo, ZDemoVendorRepo>();
builder.Services.AddScoped<IZDemoDepartmentRepo, ZDemoDepartmentRepo>();
builder.Services.AddScoped<IZDemoTitleRepo, ZDemoTitleRepo>();
builder.Services.AddScoped<IZDemoEmployeeRepo, ZDemoEmployeeRepo>();
builder.Services.AddScoped<IZDemoEmployeeSchoolRepo, ZDemoEmployeeSchoolRepo>();
builder.Services.AddScoped<IZDemoEmployeeExperienceRepo, ZDemoEmployeeExperienceRepo>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
