using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronus.Data.Sql.ExecutionEngines;

namespace Cronus.Data.SqlExecutionEngines
{
    public sealed class MsSqlExecutionEngine : BaseWindowsExecutionEngine, ISqlExecutionEngine
    {
        public override int UpdateEntity(DataEntity entity)
        {
            throw new NotImplementedException();
        }

        public override int InsertNewEntity(DataEntity entity)
        {
            throw new NotImplementedException();
        }

        public override List<T> GetEntites<T>(string sqlCommand)
        {
            // ToDo: Fill DataTable
            DataTable blubb = null;

            return base.Wrap<T>(blubb);
        }
    }
}
