using System.ComponentModel;

namespace Cronus.Data.Sync
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SyncClientEntity : SyncEntity, ISyncClientEntity
    {
        public int _subVersion { get; set; }

        /// <summary>
        /// Gets called when the <see cref="INotifyPropertyChanged.PropertyChanged"/> - event is triggered
        /// </summary>
        /// <param name="propertyName">Der Name der Eigenschaft.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            // Increment the SubVersion of this Entity to detect a change on the Dataset
            this._subVersion++;
        }
    }
}
