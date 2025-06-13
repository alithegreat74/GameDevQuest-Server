namespace GamedevQuest.Helpers
{
    public class ConnectionHelper
    {
        private readonly string? _host;
        private readonly string? _port;
        private readonly string? _user;
        private readonly string? _password;
        private readonly string? _database;
        public ConnectionHelper()
        {
            _host = Environment.GetEnvironmentVariable("DB_HOST");
            _port = Environment.GetEnvironmentVariable("DB_PORT");
            _user = Environment.GetEnvironmentVariable("DB_USER");
            _password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            _database = Environment.GetEnvironmentVariable("DB_DATABASE");
        }
        public string? GenerateConnectionString()
        {
            return $"Host={_host};Port={_port};Database={_database};Username={_user};Password={_password}";

        }
    }
}
