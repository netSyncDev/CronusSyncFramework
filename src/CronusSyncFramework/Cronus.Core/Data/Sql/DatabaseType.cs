using System.Diagnostics.CodeAnalysis;

namespace Cronus.Data.Sql
{
    public enum DatabaseType
    {
        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709")]
        MsSql,
        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709")]
        MySql,
        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709")]
        SqLite
    }
}