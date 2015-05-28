using System;

namespace Cronus.Data.Sql
{
    public interface IDataValueToSqlValueFormatter
    {
        string FormatValueToSqlValue(DatabaseType dbType, Type sourcePropertyType, object value);
    }
}
