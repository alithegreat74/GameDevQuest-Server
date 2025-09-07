using GamedevQuest.Helpers.DatabaseHelpers;
using GamedevQuest.Repositories;
using GamedevQuest.Services;

namespace GamedevQuest.Helpers
{
    public class DependencyInjectionHelper
    {
        private readonly WebApplicationBuilder _builder;
        public DependencyInjectionHelper(WebApplicationBuilder builder)
        {
            _builder = builder;
        }
        public void AddInjections()
        {
            _builder.Services.AddScoped<UnitOfWork>();

            _builder.Services.AddScoped<UserRepository>();
            _builder.Services.AddScoped<LessonRepository>();
            _builder.Services.AddScoped<TestRepository>();
            _builder.Services.AddScoped<RefreshTokenRepository>();

            _builder.Services.AddScoped<UserSignupService>();
            _builder.Services.AddScoped<UserLoginService>();
            _builder.Services.AddScoped<LessonService>();
            _builder.Services.AddScoped<TestService>();
            _builder.Services.AddScoped<RefreshTokenService>();
            _builder.Services.AddScoped<UserService>();

            _builder.Services.AddScoped<TokenHelper>();
            _builder.Services.AddScoped<AuthorizationHelper>();
            _builder.Services.AddScoped<PasswordHelper>();

        }
    }
}
