namespace Project1.Contexts.DbConnection
{
    public class AspClassDbConnection
    {
        public static string ConnectionString = string.Empty;
        public static void SetConnectionString(IConfiguration? _config)
        {
            ConnectionString = _config.GetConnectionString("AspClass");
        }
    }
}
