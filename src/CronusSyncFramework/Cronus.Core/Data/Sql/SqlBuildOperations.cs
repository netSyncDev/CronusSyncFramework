using System;

namespace Cronus.Data.Sql
{
    /// <summary>
    /// Represents the Possible Operations which can be Executed on a Entity
    /// </summary>
    [Flags]
    public enum SqlBuildOperations
    {
        /// <summary>
        /// Select Command
        /// </summary>
        Select = 0x01,
        /// <summary>
        /// Insert Command
        /// </summary>
        Insert = 0x02,
        /// <summary>
        /// Update Command
        /// </summary>
        Update = 0x03,
        /// <summary>
        /// Delete Command
        /// </summary>
        Delete = 0x04
    }
}
