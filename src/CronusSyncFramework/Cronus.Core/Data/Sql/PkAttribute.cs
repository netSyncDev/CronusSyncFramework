using System;

namespace Cronus.Data.Sql
{
    /// <summary>
    /// Represents the Primary key for a Entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PKAttribute : Attribute
    {
        private PKType _typeOfPK;

        /// <summary>
        /// Gets the Type of the Primerty Key
        /// </summary>
        /// <value>Type of the Pk <see cref="PKType"/></value>
        public PKType TypeOfPK
        {
            get { return this._typeOfPK; }
        }

        /// <summary>
        /// Initializes a new Instance of the <see cref="PKAttribute"/> - Class
        /// Initializes a Normal Pk for AutoIncrement use other Constructor
        /// </summary>
        public PKAttribute() : this(PKType.Normal)
        {
            
        }

        /// <summary>
        /// Initializes a new Instance of hte <see cref="PKAttribute"/> - Class
        /// </summary>
        /// <param name="typeOfPK">The Type of the Primary key</param>
        public PKAttribute(PKType typeOfPK)
        {
            this._typeOfPK = typeOfPK;
        }
    }
}
