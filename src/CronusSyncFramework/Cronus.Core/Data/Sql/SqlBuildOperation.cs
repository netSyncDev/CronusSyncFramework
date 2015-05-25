using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronus.Core.Data.Sql
{
    /// <summary>
    /// Represents the Possible Operations which can be Executed on a Entity
    /// </summary>
    [Flags]
    public enum SqlBuildOperation
    {
        /// <summary>
        /// Select Command
        /// </summary>
        Select = 0x00,
        /// <summary>
        /// Insert Command
        /// </summary>
        Insert = 0x01,
        /// <summary>
        /// Update Command
        /// </summary>
        Update = 0x02,
        /// <summary>
        /// Delete Command
        /// </summary>
        Delete = 0x03
    }
}
