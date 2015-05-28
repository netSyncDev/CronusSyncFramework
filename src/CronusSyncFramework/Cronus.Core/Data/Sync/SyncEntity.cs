using System;

namespace Cronus.Data.Sync
{
    public abstract class SyncEntity : DataEntity, ISyncEntity
    {
        public int _mainVersion { get; set; }

        public Guid _syncId { get; set; }

        public DateTime _changedAt { get; set; }

        public bool _deleted { get; set; }

        /// <summary>
        /// Gets Executed Before an SqlBuild Operation is Executed
        /// </summary>
        /// <param name="buildOperations">The Executed Build Operation</param>
        protected override void OnBeforeStatementBuild(Sql.SqlBuildOperations buildOperations)
        {
            base.OnBeforeStatementBuild(buildOperations);

            switch (buildOperations)
            {
                case Sql.SqlBuildOperations.Insert:
                    this._mainVersion = -1;
                    this._syncId = Guid.NewGuid();
                    break;
                case Sql.SqlBuildOperations.Update:
                    this._changedAt = DateTime.Now;
                    break;
            }
        }



    }
}
