
namespace ToDoList.Infrastructure.Options
{
    public class DatabaseOptions
    {
        public const string SectionName = "Database";

        public string Server { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string User { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public bool TrustServerCertificate { get; init; }

        public string ToConnectionString()
        {
            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = Server,
                InitialCatalog = Name,
                UserID = User,
                Password = Password,
                TrustServerCertificate = TrustServerCertificate
            };
            return builder.ConnectionString;
        }
    }
}
