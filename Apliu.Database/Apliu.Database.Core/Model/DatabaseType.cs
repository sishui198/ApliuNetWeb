using System;

namespace Apliu.Database.Core
{
    public enum DatabaseType
    {
        Mysql = 0,
        Oracle = 1,
        SqlServer = 2,
        OleDb = 3,
        [Obsolete("暂未支持")]
        MongoDb = 4,
    }
}
