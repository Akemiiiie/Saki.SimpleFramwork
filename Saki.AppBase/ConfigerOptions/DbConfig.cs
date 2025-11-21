namespace Saki.Framework.AppBase.ConfigerOptions
{
    public enum DbType
    {
        SQLite,
        SqlServer
    }

    public enum OrmType
    {
        EFCore,
        Dapper
    }

    // 可以放在 App.config 或用户选择
    public static class AppConfig
    {
        public static DbType CurrentDbType = DbType.SQLite;
    }
}
