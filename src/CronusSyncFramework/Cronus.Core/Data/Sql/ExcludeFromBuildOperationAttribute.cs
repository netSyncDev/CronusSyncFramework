using System;
using System.Diagnostics.CodeAnalysis;

namespace Cronus.Data.Sql
{
    /// <summary>
    /// Used to Exclude Properties on a <see cref="SqlBuildOperations"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [SuppressMessage("Microsoft.Design", "CA1019")]
    public sealed class ExcludeFromBuildOperationAttribute : Attribute
    {
        private SqlBuildOperations _excludeFrom;

        internal SqlBuildOperations Excludes
        {
            get { return this._excludeFrom; }
        }
        
        /// <summary>
        /// Initializes a new Instance of the <see cref="ExcludeFromBuildOperationAttribute"/> - Class
        /// </summary>
        /// <param name="buildOperations">Operations where this property should be Excluded from the statment build process</param>
        public ExcludeFromBuildOperationAttribute(SqlBuildOperations buildOperations)
        {
            this._excludeFrom = buildOperations;
        }
    }
}
