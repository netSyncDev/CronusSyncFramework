using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronus.Core.Data.Sql
{
    /// <summary>
    /// Represents the Type of the Primary Key for a Entity
    /// </summary>
    public enum PkAttributeType
    {
        /// <summary>
        /// Defines the Normal Primary Key for a Entity.
        /// </summary>
        Normal,
        /// <summary>
        /// Defines that the Pk of the Entity is AutoIncremented
        /// </summary>
        AutoIncrement
    }
}
