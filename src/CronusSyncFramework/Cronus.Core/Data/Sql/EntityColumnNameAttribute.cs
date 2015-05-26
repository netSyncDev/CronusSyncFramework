using System;

namespace Cronus.Core.Data.Sql
{
    /// <summary>
    /// Used to have different Names in the Entity Classes. The Property Name does not belong to the DatabaseColumn Name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EntityColumnNameAttribute : Attribute, IEquatable<EntityColumnNameAttribute>
    {
        private string _columnName;

        /// <summary>
        /// Gets the ColumName for this Property in the Database
        /// </summary>
        /// <value>The Name of the Column</value>
        public string ColumnName
        {
            get { return this._columnName; }
        }

        /// <summary>
        /// Initializes a new Instance of the <see cref="EntityColumnNameAttribute"/> - Class
        /// </summary>
        /// <param name="columnName">The Column Name in the Database</param>
        /// <exception cref="ArgumentNullException">If paramter is Null or Empty</exception>
        public EntityColumnNameAttribute(string columnName)
        {
            if(string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException("columnName");

            this._columnName = columnName;
        }

        public bool Equals(EntityColumnNameAttribute other)
        {
            if (other.ColumnName.Equals(this.ColumnName)) return true;
            return false;
        }
    }
}
