using GamedevQuest.Context;
using GamedevQuest.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
string? DefaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GameDevQuestDbContext>(options => options.UseNpgsql(DefaultConnectionString));
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
