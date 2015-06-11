using Newtonsoft.Json;

namespace Cronus.Data.Sync
{
    /// <summary>
    /// Represents a Sync Client Data Entity
    /// </summary>
    public interface ISyncClientEntity : ISyncEntity
    {
        /// <summary>
        /// Indicates the Changes on the Client Side
        /// </summary>
        /// <remarks>Icremented one Time on each Change</remarks>
        [JsonIgnore]
        int _subVersion { get; set; }
    }
}