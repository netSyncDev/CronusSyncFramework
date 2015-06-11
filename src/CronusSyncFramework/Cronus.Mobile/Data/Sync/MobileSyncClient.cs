using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronus.Data.Sync
{
    public sealed class MobileSyncClient<T> : BaseSyncClient<T> where T : DataEntity, ISyncClientEntity
    {
        public override List<T> GetChanges()
        {
            return this.GetDataForTable().Where(x => x._subVersion != 0 || x._mainVersion == -1).ToList();
        }

        private List<T> GetDataForTable()
        {
            
        }
    }
}
