namespace TunicoAniversarioAdmissao.Controllers
{
    public class Conexao
    {
        public static string banco = GetConnectionString();

        private static string GetConnectionString()
        {
            string server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "";
            string database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "";
            string userId = Environment.GetEnvironmentVariable("DB_USER") ?? "";
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
            string trustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_CERT") ?? "true";

            return $"Server={server};Database={database};User Id={userId};Password={password};TrustServerCertificate={trustServerCertificate};";
        }
    }
}
