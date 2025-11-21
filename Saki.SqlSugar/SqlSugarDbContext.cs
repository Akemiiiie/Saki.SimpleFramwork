using Microsoft.AspNetCore.Http;
using Saki.BaseTemplate.ConfigerOptions;
using SqlSugar;
using StackExchange.Profiling;
using System.Reflection;

namespace Saki.RepositoryTemplate.DBClients;

public class Repository<T> : SimpleClient<T> where T : class, new()
{
    public Repository(ISqlSugarClient db)
    {
        base.Context = db;
    }
}