using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Cronus.Data.Sql.ExecutionEngines;

namespace Cronus.Data.Sync
{
    public abstract class BaseSyncClient<T> where T : DataEntity, ISyncEntity
    {
        private readonly ISqlExecutionEngine _sqlEngine;

        public virtual List<T> GetChanges()
        {
            DataEntity entityToGetData = Activator.CreateInstance<T>();
            string sqlComd = entityToGetData.GetSelectCommand();

            return _sqlEngine.GetEntites<T>(sqlComd);
        }

        public virtual int InsertNewObject(T entity)
        {
            entity._syncId = Guid.NewGuid();
            entity._mainVersion = -1;
            return _sqlEngine.InsertNewEntity(entity);
        }

        public virtual int UpdateEntity(T entity)
        {
            entity._mainVersion ++;
            return _sqlEngine.UpdateEntity(entity);
        }

        public virtual void DeleteEntity(T entity)
        {
            entity.IgnoreNotify(()=> entity._deleted = true);
        }
    }
}
