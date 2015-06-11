using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronus.Data.Sql.ExecutionEngines
{
    public abstract class BaseSqlExecutionEngine
    {
        public abstract int UpdateEntity(DataEntity entity);

        public abstract int InsertNewEntity(DataEntity entity);

        public abstract List<T> GetEntites<T>(string sqlCommand);
    }
}
