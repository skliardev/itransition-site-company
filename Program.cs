using Company.Service;
using Company.Domain;
using Company.Domain.Entities;
using Company.Domain.Repositories.Abstract;
using Company.Domain.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

//Include data config from appsetting.json
builder.Configuration.Bind("ProjectDevelopers", new Config());

//Connect func app as service
builder.Services.AddTransient<IRepository<User>, EFUserRepository>();
builder.Services.AddTransient<IRepository<Message>, EFMessageRepository>();
builder.Services.AddTransient<DataManager>();

//Connect to context DB
builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(
    builder.Configuration.GetConnectionString("localdb")
));

//Configure Identity систему
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    //opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 1;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//Configure authentication cookie
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.Cookie.Name = Config.CompanyName + "Auth";
    opts.Cookie.HttpOnly = true;
    opts.LoginPath = "/Account/Login";
    opts.LogoutPath = "/Account/Logout";
    opts.AccessDeniedPath = "/Account/Accessdenide";
    //opts.ExpireTimeSpan = TimeSpan.Zero;
});

//Configure policy authorization
builder.Services.AddAuthorization(opts =>
{
    opts.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{name?}");
});

app.Run();
