using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

public class SqlServerDapperContext : IDbContext
{
    private readonly string _connStr;
    public SqlServerDapperContext(string connStr) => _connStr = connStr;

    public IDbConnection CreateConnection() => new SqlConnection(_connStr);
}
