using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimiiformesWebApplication.Data;
using SimiiformesWebApplication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Identity config, megerõsített felhasználónak kell lennie (RewuireConfirmedAccount),
//hozzáadja a role-okat, RoleManager-t (funkciókat biztosít a roleok kezelésére), és az adatbázis kontextust az ApplicationDbContext jelöli
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)    
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    // Create database and apply all migrations
    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    if (context != null && context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    CreateRoles(serviceScope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}

else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Roles creation
static void CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    Task<IdentityResult> roleResult;

    //Check that there is an Administrator role and create if not
    foreach (Role role in Enum.GetValues(typeof(Role)))
    {
        Task<bool> hasRole = roleManager.RoleExistsAsync(role.ToString());
        hasRole.Wait();

        if (!hasRole.Result)
        {
            roleResult = roleManager.CreateAsync(new IdentityRole(role.ToString()));
            roleResult.Wait();
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();