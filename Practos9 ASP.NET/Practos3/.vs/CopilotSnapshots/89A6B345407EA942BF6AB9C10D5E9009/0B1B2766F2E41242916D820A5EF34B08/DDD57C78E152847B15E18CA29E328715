using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Services;
var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopAdminConnection")));

// Persist data protection keys to keep cookie validation after app restart
var keysFolder = System.IO.Path.Combine(env.ContentRootPath, "DataProtection-Keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(keysFolder));

// Authentication and authorization
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "WebShopAuth";
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and default admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<WebShopDbContext>();
        // Ensure DB is created
        db.Database.EnsureCreated();

        // Seed roles if missing
        var adminRole = db.Roles.FirstOrDefault(r => r.RoleName == "Admin");
        if (adminRole == null)
        {
            adminRole = new Practos3.Models.AdminRole { RoleName = "Admin" };
            db.Roles.Add(adminRole);
        }
        if (!db.Roles.Any(r => r.RoleName == "User"))
        {
            db.Roles.Add(new Practos3.Models.AdminRole { RoleName = "User" });
        }
        db.SaveChanges();

        // Ensure admin user exists and has admin role
        var adminUser = db.Users.FirstOrDefault(u => u.Login == "admin");
        if (adminUser == null)
        {
            adminUser = new Practos3.Models.AdminUser
            {
                Login = "admin",
                PasswordHash = AuthHelper.HashPassword("admin123"),
                RoleID = adminRole.RoleID
            };
            db.Users.Add(adminUser);
            db.SaveChanges();
        }
        else
        {
            var changed = false;
            if (adminUser.RoleID != adminRole.RoleID)
            {
                adminUser.RoleID = adminRole.RoleID;
                changed = true;
            }

            // Ensure password is hashed; if not, set default password
            bool hasValidHash = false;
            try
            {
                var bytes = Convert.FromBase64String(adminUser.PasswordHash ?? "");
                if (bytes.Length == 1 + 16 + 32 && bytes[0] == 0x01) hasValidHash = true;
            }
            catch { }

            if (!hasValidHash)
            {
                adminUser.PasswordHash = AuthHelper.HashPassword("admin123");
                changed = true;
            }

            if (changed)
            {
                db.Users.Update(adminUser);
                db.SaveChanges();
            }
        }
    }
    catch
    {
        // ignore seeding errors to not break app startup in dev
    }
}

// Configure the HTTP request pipeline.
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

// Map short routes for login/register/access denied
app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Account", action = "Register" });

app.MapControllerRoute(
    name: "accessdenied",
    pattern: "accessdenied",
    defaults: new { controller = "Account", action = "AccessDenied" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



