using System.Collections.Generic;
using System.Linq;
using Cronus.Data.Sql;

namespace Cronus.Data.Sync
{
    public abstract class SyncClient
    {
        private static ISqlStatementExecutionEngine _sqlExecutionEngine;

        public abstract IEnumerable<T> GetChangesForTable<T>();

        public virtual IEnumerable<T> CheckAndExecuteDeletes<T>(IEnumerable<T> entities) where T : DataEntity, ISyncEntity
        {
            for (int i = 0; i < entities.Count(); i++)
            {
                ISyncEntity entity = entities.ElementAt(i);

                if (entity._deleted)
                {
                    _sqlExecutionEngine.ExecuteSqlStatement(((DataEntity) entity).GetDeleteCommand(),
                        SqlBuildOperations.Delete);
                }
            }

            return null;
        }
    }
}
