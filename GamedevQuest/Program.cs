using GamedevQuest.Context;
using GamedevQuest.Helpers;
using GamedevQuest.Helpers.DatabaseHelpers;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();
string? DefaultConnectionString = new ConnectionHelper().GenerateConnectionString();
builder.Services.AddDbContext<GameDevQuestDbContext>(options =>
    options.UseNpgsql(DefaultConnectionString));

var dependencyInjectionHelper = new DependencyInjectionHelper(builder);
dependencyInjectionHelper.AddInjections();
var corsHelper = new CorsHelper(builder);
corsHelper.SetUpCorsPolicy();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
        };
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context => {
                if (context.Request.Cookies.ContainsKey(AuthorizationHelper.JwtAuthorizationKey))
                {
                    context.Token = context.Request.Cookies[AuthorizationHelper.JwtAuthorizationKey];
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHealthChecks("/health");
string defaultCorsPolicyName = corsHelper.GetDefaultCorsName();
if(!string.IsNullOrEmpty(defaultCorsPolicyName))
    app.UseCors(defaultCorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
