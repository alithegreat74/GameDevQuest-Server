using Microsoft.Extensions.Configuration;

namespace GamedevQuest.Helpers
{
    public class CorsHelper
    {
        private class CorsOrigin
        {
            public string Name {  get; set; }
            public string Url { get; set; }
        }
        private readonly WebApplicationBuilder _builder;
        private List<CorsOrigin>? _allowedOrigins;
        public CorsHelper(WebApplicationBuilder builder)
        {
            _builder = builder;
            _allowedOrigins = new List<CorsOrigin>();
        }
        public void SetUpCorsPolicy()
        {
            _allowedOrigins = _builder.Configuration.GetSection("Cors:AllowedOrigins").Get<List<CorsOrigin>>();
            if (_allowedOrigins == null || _allowedOrigins.Count < 0)
                return;
            _builder.Services.AddCors(options =>
            {
                foreach (CorsOrigin origin in _allowedOrigins)
                {
                    if (string.IsNullOrEmpty(origin.Url) || string.IsNullOrEmpty(origin.Name))
                        continue;
                    options.AddPolicy(origin.Name, policy =>
                    {
                        policy.WithOrigins(origin.Url)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                }
            });
        }
        public string GetDefaultCorsName()
        {
            if(_allowedOrigins==null || _allowedOrigins.Count < 0 || string.IsNullOrEmpty(_allowedOrigins[0].Name))
                return string.Empty;

            return _allowedOrigins[0].Name;
        }
    }
}
