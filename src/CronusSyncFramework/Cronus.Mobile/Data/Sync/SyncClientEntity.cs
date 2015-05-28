using Cronus.Data.Sql;

namespace Cronus.Data.Sync
{
    public abstract class SyncClientEntity : SyncEntity, ISyncClientEntity
    {
        public int _subVersion { get; set; }

        /// <summary>
        /// Gets Executed Before an SqlBuild Operation is Executed
        /// </summary>
        /// <param name="buildOperation">The Executed Build Operation</param>
        protected override void OnBeforeStatementBuilded(SqlBuildOperation buildOperation)
        {
            //ToDo: Darüber nachdenken. Evtl mittels INotifyPropertyChanged änderungen feststellen und dann SubVersion inkrementieren
            base.OnBeforeStatementBuilded(buildOperation);

            if (buildOperation == SqlBuildOperation.Update)
                _subVersion ++;
        }
    }
}
