using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronus.Core.Data.Sql
{
    public interface IDataValueToSqlValueFormatter
    {
        string FormatValueToSqlValue(DatabaseType dbType, Type sourcePropertyType, object value);
    }
}
