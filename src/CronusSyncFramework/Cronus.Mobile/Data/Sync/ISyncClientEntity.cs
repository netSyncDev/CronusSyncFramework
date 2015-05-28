using Newtonsoft.Json;

namespace Cronus.Data.Sync
{
    /// <summary>
    /// Represents a Sync Client Data Entity
    /// </summary>
    internal interface ISyncClientEntity : ISyncEntity
    {
        /// <summary>
        /// Indicates the Changes on the Client Side
        /// </summary>
        /// <remarks>Icremented one Time on each Change</remarks>
        [JsonProperty("_subVersion")]
        int _subVersion { get; set; }
    }
}