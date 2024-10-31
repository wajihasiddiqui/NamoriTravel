using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using System.Globalization;
using NamoriTravel.Common;
using ServiceLayer;
using DomainLayer;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:SecretKey").Get<string>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

// Database Connection
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = new[] { new CultureInfo("en-US") };
    options.SupportedUICultures = new[] { new CultureInfo("en-US") };
});

var ConnectionString = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Get<string>();
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<NamoriTrvl_dbContext>(options =>
    options.UseSqlServer(ConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    }));

// Add Repositories
builder.Services.AddScoped<DbContext, NamoriTrvl_dbContext>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
AppSettings.Initialize(builder.Configuration);

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecifOrigin", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();

    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("Views/Shared/Error404.cshtml");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowSpecifOrigin");
app.UseRouting();

// Add session middleware
app.UseSession();
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("jwtToken");

    if (!string.IsNullOrEmpty(token))
        context.Request.Headers.Append("Authorization", "Bearer " + token);

    await next.Invoke();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NamoriTravels}/{action=Index}/{id?}");

app.MapGet("/", context =>
{
    context.Response.Redirect("/NamoriTravels/Index");
    return Task.CompletedTask;
});

app.Run();

