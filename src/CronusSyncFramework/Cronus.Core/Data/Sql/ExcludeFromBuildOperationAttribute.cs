using System;

namespace Cronus.Data.Sql
{
    /// <summary>
    /// Used to Exclude Properties on a <see cref="SqlBuildOperation"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromBuildOperationAttribute : Attribute
    {
        private SqlBuildOperation _excludeFrom;

        internal SqlBuildOperation Excludes
        {
            get { return this._excludeFrom; }
        }
        
        /// <summary>
        /// Initializes a new Instance of the <see cref="ExcludeFromBuildOperationAttribute"/> - Class
        /// </summary>
        /// <param name="buildOperation">Operations where this property should be Excluded from the statment build process</param>
        public ExcludeFromBuildOperationAttribute(SqlBuildOperation buildOperation)
        {
            this._excludeFrom = buildOperation;
        }
    }
}
