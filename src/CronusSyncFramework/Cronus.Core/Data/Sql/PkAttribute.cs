using System;

namespace Cronus.Data.Sql
{
    /// <summary>
    /// Represents the Primary key for a Entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PkAttribute : Attribute
    {
        private PkAttributeType _pkType;

        /// <summary>
        /// Gets the Type of the Primerty Key
        /// </summary>
        /// <value>Type of the Pk <see cref="PkAttributeType"/></value>
        public PkAttributeType PkType
        {
            get { return this._pkType; }
        }

        /// <summary>
        /// Initializes a new Instance of the <see cref="PkAttribute"/> - Class
        /// Initializes a Normal Pk for AutoIncrement use other Constructor
        /// </summary>
        public PkAttribute() : this(PkAttributeType.Normal)
        {
            
        }

        /// <summary>
        /// Initializes a new Instance of hte <see cref="PkAttribute"/> - Class
        /// </summary>
        /// <param name="pkType">The Type of the Primary key</param>
        public PkAttribute(PkAttributeType pkType)
        {
            this._pkType = pkType;
        }
    }
}
