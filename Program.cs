using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using TOTP_BugTracker.Services.Interfaces;
using TOTP.Services;
using TOTP_BugTracker.Services;
using TOTP_BugTracker.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = DataUtility.GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString,
    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BTUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<BTUserClaimsPrincipalFactory>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Custom Services Below:
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddScoped<IInviteService, InviteService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddMvc();


var app = builder.Build();

var scope = app.Services.CreateScope();
await DataUtility.ManageDataAsync(scope.ServiceProvider);

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
    pattern: "{controller=Home}/{action=Landing}/{id?}");
app.MapRazorPages();

app.Run();
