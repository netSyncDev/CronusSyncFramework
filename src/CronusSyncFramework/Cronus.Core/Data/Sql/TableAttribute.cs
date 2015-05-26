using System;

namespace Cronus.Core.Data.Sql
{
    /// <summary>
    /// Represents a Table in the Database
    /// Used to use a different Name for the Entity as in the DatabaseServer. The Name of the Entity don´t has to Match the Table in the Server
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute, IEquatable<TableAttribute>
    {
        private string _tableName;

        /// <summary>
        /// Gets the TableName for this Entity
        /// </summary>
        /// <value>The Name of the Table in the Database</value>
        public string TableName
        {
            get { return this._tableName; }
        }

        /// <summary>
        /// Initializes a new Instance of the <see cref="TableAttribute"/> - Class
        /// </summary>
        /// <param name="tableName">The name of the Entity in the Database</param>
        public TableAttribute(string tableName)
        {
            if(string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            this._tableName = tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _tableName;
        }

        public bool Equals(TableAttribute other)
        {
            return other.TableName.Equals(this.TableName);
        }
    }
}
