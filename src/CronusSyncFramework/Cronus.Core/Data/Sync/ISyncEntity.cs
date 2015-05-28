using System;
using Newtonsoft.Json;

namespace Cronus.Data.Sync
{
    public interface ISyncEntity
    {
        [JsonProperty("_mainVersion")]
        int _mainVersion { get; set; }

        [JsonProperty("_syncId")]
        Guid _syncId { get; set; }

        [JsonProperty("_changedAt")]
        DateTime _changedAt { get; set; }

        [JsonProperty("_deleted")]
        bool _deleted { get; set; }
    }
}