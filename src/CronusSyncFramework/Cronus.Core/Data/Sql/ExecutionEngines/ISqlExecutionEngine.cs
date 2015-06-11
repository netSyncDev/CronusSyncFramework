using System.Collections.Generic;

namespace Cronus.Data.Sql.ExecutionEngines
{
    public interface ISqlExecutionEngine
    {
        int UpdateEntity(DataEntity entity);

        int InsertNewEntity(DataEntity entity);

        List<T> GetEntites<T>(string sqlCommand);
    }
}