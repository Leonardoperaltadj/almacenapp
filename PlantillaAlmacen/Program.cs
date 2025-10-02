using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using PlantillaAlmacen.Models;
using PlantillaAlmacen.Security;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;


FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("App_Data/firebase-adminsdk.json")
});

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AlmacenDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));
builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddDbContext<AlmacenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/Auth/Login";
                    opt.AccessDeniedPath = "/Auth/Login";
                    opt.ExpireTimeSpan = TimeSpan.FromHours(8);
                    opt.SlidingExpiration = true;
                    opt.Cookie.HttpOnly = true;
                    opt.Cookie.SameSite = SameSiteMode.Lax;
                });

builder.Services.AddControllersWithViews(o =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    o.Filters.Add(new AuthorizeFilter(policy));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
