using Cronus.Data.Sql;

namespace Cronus.Data.Sync
{
    public abstract class SyncClientEntity : SyncEntity, ISyncClientEntity
    {
        public int _subVersion { get; set; }

        /// <summary>
        /// Gets Executed Before an SqlBuild Operation is Executed
        /// </summary>
        /// <param name="buildOperations">The Executed Build Operation</param>
        protected override void OnBeforeStatementBuild(SqlBuildOperations buildOperations)
        {
            //ToDo: Darüber nachdenken. Evtl mittels INotifyPropertyChanged änderungen feststellen und dann SubVersion inkrementieren
            base.OnBeforeStatementBuild(buildOperations);

            if (buildOperations == SqlBuildOperations.Update)
                _subVersion ++;
        }
    }
}
