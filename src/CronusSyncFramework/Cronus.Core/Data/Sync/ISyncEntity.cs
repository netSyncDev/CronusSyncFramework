using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Cronus.Data.Sync
{
    public interface ISyncEntity
    {
        [SuppressMessage("Microsoft.Naming", "CA1709")]
        [SuppressMessage("Microsoft.Naming", "CA1707")]
        [JsonProperty("_mainVersion")]
        int _mainVersion { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709")]
        [SuppressMessage("Microsoft.Naming", "CA1707")]
        [JsonProperty("_syncId")]
        Guid _syncId { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709")]
        [SuppressMessage("Microsoft.Naming", "CA1707")]
        [JsonProperty("_changedAt")]
        DateTime _changedAt { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709")]
        [SuppressMessage("Microsoft.Naming", "CA1707")]
        [JsonProperty("_deleted")]
        bool _deleted { get; set; }
    }
}