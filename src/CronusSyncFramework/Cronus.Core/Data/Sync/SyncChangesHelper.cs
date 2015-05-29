using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronus.Data.Sync
{
    public static class SyncChangesHelper
    {
        public static ISyncEntity GetNewestEntity(params ISyncEntity[] entities)
        {
            return entities.Where(x => x._changedAt == entities.Max(y => y._changedAt)).FirstOrDefault();
        }
    }
}
