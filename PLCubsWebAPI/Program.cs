using AuthPermissions;
using AuthPermissions.AspNetCore;
using AuthPermissions.AspNetCore.Services;
using AuthPermissions.AspNetCore.StartupServices;
using AuthPermissions.BaseCode;
using AuthPermissions.BaseCode.SetupCode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PLCubsWebAPI.Data;
using PLCubsWebAPI.Models;
using PLCubsWebAPI.PermissionsCode;
using RunMethodsSequentially;
using System.Text;
using PLCubsWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");
var lockFolder = builder.Environment.WebRootPath;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClubWebApiWithToken.IndividualAccounts", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(
        options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();



// Configure Authentication using JWT token with refresh capability
var jwtData = new JwtSetupData();
builder.Configuration.Bind("JwtData", jwtData);
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
   {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtData.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtData.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtData.SigningKey)),
            ClockSkew = TimeSpan.Zero //The default is 5 minutes, but we want a quick expires for JTW refresh
        };

        //This code came from https://www.blinkingcaret.com/2018/05/30/refresh-tokens-in-asp-net-core-web-api/
        //It returns a useful header if the JWT Token has expired
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
   });


builder.Services.RegisterAuthPermissions<TenantUserPermission>(options =>
{
    options.TenantType = TenantTypes.SingleLevel;
    //options.AppConnectionString = connectionString;

    //This tells AuthP that you don't have multiple instances of your app running,
    //so it can run the startup services without a global lock
    options.UseLocksToUpdateGlobalResources = false;

    //This sets up the JWT Token. The config is suitable for using the Refresh Token with your JWT Token
    options.ConfigureAuthPJwtToken = new AuthPJwtConfiguration
    {
        Issuer = jwtData.Issuer,
        Audience = jwtData.Audience,
        SigningKey = jwtData.SigningKey,
        TokenExpires = new TimeSpan(0, 5, 0), //Quick Token expiration because we use a refresh token
        RefreshTokenExpires = new TimeSpan(1, 0, 0, 0) //Refresh token is valid for one day
    };
 })
                .UsingEfCoreSqlServer(connectionString) //NOTE: This uses the same database as the individual accounts DB
                .IndividualAccountsAuthentication()
                .RegisterAddClaimToUser<AddTenantNameClaim>()
                //.AddSuperUserToIndividualAccounts()
                .RegisterFindUserInfoService<IndividualAccountUserLookup>()
                .AddRolesPermissionsIfEmpty(AppAuthSetupData.RolesDefinition)
                .AddTenantsIfEmpty(AppAuthSetupData.TenantDefinition)
                .AddAuthUsersIfEmpty(AppAuthSetupData.UsersRolesDefinition)
                //.RegisterTenantChangeService<ClubTenantChangeService>()
                .SetupAspNetCoreAndDatabase(options =>
                {
                    //Migrate individual account database
                    options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<ApplicationDbContext>>();
                    //Add demo users to the database
                    options.RegisterServiceToRunInJob<StartupServicesIndividualAccountsAddDemoUsers>();
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
