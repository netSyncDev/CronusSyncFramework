using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronus.Data.Sql.ExecutionEngines;

namespace Cronus.Data.SqlExecutionEngines
{
    public abstract class BaseWindowsExecutionEngine : BaseSqlExecutionEngine
    {
        protected virtual List<T> Wrap<T>(DataTable data)
        {
            return null;;
        }
    }
}
