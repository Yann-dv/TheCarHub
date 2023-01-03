using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using theCarHub.Data;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load(); //env var loads

var azureConnectionString = Environment.GetEnvironmentVariable("TheCarHub_AzureConnection");
string connectionString;

// Add services to the container.
if (azureConnectionString != null) {
    connectionString = azureConnectionString ?? throw new InvalidOperationException("Connection string 'AzureConnection' not found.");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        // User settings.
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 9;

        // Lockout settings.
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

if (builder.Configuration["Authentication:Facebook:AppId"] == null && Environment.GetEnvironmentVariable("Facebook_AppId") != null)
{
    builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = Environment.GetEnvironmentVariable("Facebook_AppId");
        facebookOptions.AppSecret = Environment.GetEnvironmentVariable("FACEBOOK_PROVIDER_AUTHENTICATION_SECRET");
    });
}

else if (builder.Configuration["Authentication:Facebook:AppId"] != null && Environment.GetEnvironmentVariable("Facebook_AppId") == null)
{
    builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });
}
/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

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

