using GamedevQuest.Context;
using GamedevQuest.Helpers;
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

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<JwtTokenGenerator>();

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
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
