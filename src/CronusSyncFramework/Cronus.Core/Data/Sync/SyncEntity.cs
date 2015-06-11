using System;
using System.ComponentModel;

namespace Cronus.Data.Sync
{
    public abstract class SyncEntity : DataEntity, ISyncEntity
    {
        public int _mainVersion { get; set; }

        public Guid _syncId { get; set; }

        public DateTime _changedAt { get; set; }

        public bool _deleted { get; set; }

        /// <summary>
        /// Löst das <see cref="INotifyPropertyChanged.PropertyChanged"/> - Ereignis aus.
        /// </summary>
        /// <param name="propertyName">Der Name der Eigenschaft.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            this.IgnoreNotify(() =>
            {
                this._changedAt = DateTime.Now;
            });
        }

        /// <summary>
        /// Gets Executed Before an SqlBuild Operation is Executed
        /// </summary>
        /// <param name="buildOperations">The Executed Build Operation</param>
        protected override void OnBeforeStatementBuild(Sql.SqlBuildOperations buildOperations)
        {
            //ToDo: Darüber nachdenken. Evtl mittels INotifyPropertyChanged änderungen feststellen und dann SubVersion inkrementieren
            //base.OnBeforeStatementBuild(buildOperations);

            //switch (buildOperations)
            //{
            //    case Sql.SqlBuildOperations.Insert:
            //        this.IgnoreNotify(() =>
            //        {
            //            this._mainVersion = -1;
            //            this._syncId = Guid.NewGuid();
            //        });
            //        break;
            //}
        }
    }
}
