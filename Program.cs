
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<mvcStore.Services.NavigationRepository>();

builder.Services.AddAuthentication("StoreCookie").AddCookie("StoreCookie", option =>
{
    option.LoginPath = "/Login";
    option.Cookie.Name = "mvcStore.Auth";
    option.ExpireTimeSpan = TimeSpan.FromDays(30);
    option.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
});



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"
);


app.Run();
