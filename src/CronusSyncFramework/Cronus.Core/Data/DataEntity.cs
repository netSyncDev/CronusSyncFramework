using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cronus.Data.Sql;
using Cronus.Data.Sql.DataToSqlValueFormatters;
using Cronus.Properties;
using System.ComponentModel;

namespace Cronus.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataEntity : INotifyPropertyChanged
    {
        private HashSet<string> _changedPropertys;

        private static Dictionary<Type, Func<DatabaseType, object, string>> _formatterFunctions;
        private static IDataValueToSqlValueFormatter _sqlValueFormatter;

        private static DatabaseType _targetDatabaseType;

        private bool _isSupressedNotifyPropertyChanged;

        /// <summary>
        /// Tritt ein, wenn sich ein Eigenschaftswert ändert.
        /// </summary>
        // ReSharper disable InconsistentNaming
        private event PropertyChangedEventHandler _propertyChanged;

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Tritt ein, wenn sich ein Eigenschaftswert ändert.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this._propertyChanged += value; }
            remove { this._propertyChanged -= value; }
        }

        /// <summary>
        /// Gets informatins about this Table (DataEntity)
        /// </summary>
        /// <value>The Informations about the table <see cref="TableAttribute"/></value>
        internal TableAttribute TableInformations
        {
            get { return this.GetTableInformations(); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataEntity()
        {
            _sqlValueFormatter = new DefaultDataToSqlValueFormatter();
            _formatterFunctions = new Dictionary<Type, Func<DatabaseType, object, string>>();
            this._changedPropertys = new HashSet<string>();
        }

        /// <summary>
        /// Registers a own DataValue To SqlValue Format Function
        /// </summary>
        /// <param name="typeToRegister">The Type the Function should be applied to</param>
        /// <param name="formatFunction">The function which formats the .NET Type in the TargetDatabase Sql Value as string</param>
        /// <param name="shouldOverwrite"><c>True</c> if an existing Function should be overwritten, otherwise <c>False</c> in this case an Exception is thrown</param>
        /// <exception cref="ArgumentException">If a ConvertFunction for this Type is already Found and shouldOverwrite = <c>False</c></exception>
        public static void RegisterSqlDataValueFormatFunction(Type typeToRegister,
            Func<DatabaseType, object, string> formatFunction, bool shouldOverwrite)
        {
            if (typeToRegister == null)
                throw new ArgumentNullException("typeToRegister");

            if (formatFunction == null)
                throw new ArgumentNullException("formatFunction");

            if (_formatterFunctions.ContainsKey(typeToRegister) && !shouldOverwrite)
                throw new ArgumentException(
                    string.Format(Resources.KeyFoundInRegisterConvertFunction,
                        typeToRegister.Name));

            if (_formatterFunctions.ContainsKey(typeToRegister) && shouldOverwrite)
            {
                _formatterFunctions.Remove(typeToRegister);
            }

            _formatterFunctions.Add(typeToRegister, formatFunction);
        }

        public static void SetOwnDataValueToSqlValueFormatter(IDataValueToSqlValueFormatter formatter)
        {
            _sqlValueFormatter = formatter;
        }

        public static void SetTargetDatabaseType(DatabaseType dbType)
        {
            _targetDatabaseType = dbType;
        }

        /// <summary>
        /// Gets Executed Before an SqlBuild Operation is Executed
        /// </summary>
        /// <param name="buildOperations">The Executed Build Operation</param>
        protected virtual void OnBeforeStatementBuild(SqlBuildOperations buildOperations)
        {
            
        }

        /// <summary>
        /// Generates a Select Command for an Entity
        /// </summary>
        /// <returns>A Sql Statement which can be Executed</returns>
        public string GetSelectCommand()
        {
            const string selectTemplate = "SELECT {0} FROM {1}";

            List<PropertyInfo> propertiesForOperation = this.GetPropertiesForOperation(SqlBuildOperations.Select);
            string sqlFieldsForOperation = this.GetSqlFieldsForOperation(propertiesForOperation);

            return string.Format(selectTemplate, sqlFieldsForOperation, this.TableInformations);
        }

        /// <summary>
        /// Gets the Insert Command for a Entity
        /// </summary>
        /// <returns>Insert Sql Statement for this Entity</returns>
        public string GetInsertCommand()
        {
            this.OnBeforeStatementBuild(SqlBuildOperations.Insert);
            const string InsertTemplate = "INSERT INTO {0} ({1}) VALUES ({2});";

            List<PropertyInfo> propertiesForOperation = this.GetPropertiesForOperation(SqlBuildOperations.Insert);
            string sqlFieldsForOperation = this.GetSqlFieldsForOperation(propertiesForOperation);

            string insertValues = string.Empty;

            for (int i = 0; i < propertiesForOperation.Count; i++)
            {
                insertValues += this.GetSqlValueForProperty(propertiesForOperation.ElementAt(i), _targetDatabaseType);
                if (i != propertiesForOperation.Count - 1)
                    insertValues += ", ";
            }

            return string.Format(InsertTemplate, this.TableInformations, sqlFieldsForOperation, insertValues);
        }

        public string GetUpdateCommand()
        {
            const string updateTemplate = "UPDATE {0} SET {1} WHERE {2}";

            string whereCondition = this.GenerateWhereCondition();

            string setValues = string.Empty;
            List<PropertyInfo> propertiesForOperation = this.GetPropertiesForOperation(SqlBuildOperations.Update);

            for (int i = 0; i < propertiesForOperation.Count; i++)
            {
                PropertyInfo property = propertiesForOperation.ElementAt(i);
                EntityColumnNameAttribute entityInfo = property.GetCustomAttribute<EntityColumnNameAttribute>();

                setValues += string.Concat(entityInfo.ColumnName, " = ",
                    this.GetSqlValueForProperty(property, _targetDatabaseType));

                if (i != propertiesForOperation.Count - 1)
                    setValues += ", ";
            }

            return string.Format(updateTemplate, this.TableInformations, setValues, whereCondition);
        }

        /// <summary>
        /// Gets the Delete Command for a Entity
        /// </summary>
        /// <returns>A Sql Statement which deletes this Data Entity</returns>
        public string GetDeleteCommand()
        {
            const string deleteTemplate = "DELTE FROM {0} WHERE {1}";

            string whereCondition = GenerateWhereCondition();
            return string.Format(deleteTemplate, this.TableInformations, whereCondition);
        }

        private string GenerateWhereCondition()
        {
            string whereCondition = String.Empty;

            List<PropertyInfo> pkProps =
                this.GetEntityProperties().Where(x => x.IsDefined(typeof (PKAttribute))).ToList();

            if (pkProps.Count > 1)
            {
                string tempWhere = string.Empty;
                for (int i = 0; i < pkProps.Count; i++)
                {
                    EntityColumnNameAttribute entityInfo =
                        pkProps.ElementAt(i).GetCustomAttribute<EntityColumnNameAttribute>();
                    string value = this.GetSqlValueForProperty(pkProps.ElementAt(i), _targetDatabaseType);
                    tempWhere += string.Concat(entityInfo.ColumnName, " = ", value);
                    if (i != (pkProps.Count() - 2))
                        tempWhere += " AND ";
                }
                whereCondition = tempWhere;
            }
            else if (pkProps.Count == 1)
            {
                EntityColumnNameAttribute entityInfo =
                    pkProps.ElementAt(0).GetCustomAttribute<EntityColumnNameAttribute>();
                string value = this.GetSqlValueForProperty(pkProps.ElementAt(0), _targetDatabaseType);
                whereCondition += string.Concat(entityInfo.ColumnName, " = ", value);
            }
            else
            {
                //ToDo: Implment Exception
                //throw new DataEntityException();
            }
            return whereCondition;
        }

        public void IgnoreNotify(Action action)
        {
            this._isSupressedNotifyPropertyChanged = true;
            action();
            this._isSupressedNotifyPropertyChanged = false;
        }

        /// <summary>
        /// Löst das <see cref="INotifyPropertyChanged.PropertyChanged"/> - Ereignis aus.
        /// </summary>
        /// <param name="propertyName">Der Name der Eigenschaft.</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (this._isSupressedNotifyPropertyChanged) return;

            // Debug.Assert(string.IsNullOrEmpty(propertyName) || (this.GetType().GetRuntimeProperty(propertyName) != null), "Überprüfen, ob diese Eigenschaft in dieser Instanz existiert.");
            PropertyChangedEventHandler handler = this._propertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetEntityValue<T>(ref T current, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(current, newValue))
            {
                current = newValue;
                this.OnPropertyChanged(propertyName);
                this._changedPropertys.Add(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all SqlFields for the Statement
        /// </summary>
        /// <param name="properties">The List of properties to obtain the FieldNames</param>
        /// <returns>A string of SqlField Names which can be included in the Statement</returns>
        /// <example>Id, Name, Vorname</example>
        private string GetSqlFieldsForOperation(IEnumerable<PropertyInfo> properties)
        {
            string sqlFields = string.Empty;
            for (int i = 0; i < properties.Count(); i++)
            {
                PropertyInfo property = properties.ElementAt(i);
                EntityColumnNameAttribute columnAttri = property.GetCustomAttribute<EntityColumnNameAttribute>();
                sqlFields += columnAttri.ColumnName;
                
                if (i != properties.Count() - 1)
                    sqlFields += ", ";
            }
            return sqlFields;
        }

        /// <summary>
        /// Gets all Properties for the Statement Build Process
        /// </summary>
        /// <param name="operations">The Sql Build Operation</param>
        /// <returns>A list of Properties which are icluded in the Build Process of the Statement</returns>
        private List<PropertyInfo> GetPropertiesForOperation(SqlBuildOperations operations)
        {
            List<PropertyInfo> @return = new List<PropertyInfo>();
            List<PropertyInfo> entityProperties = this.GetEntityProperties();
            for (int i = 0; i < entityProperties.Count(); i++)
            {
                PropertyInfo property = entityProperties.ElementAt(i);
                if(GetIfPropertyIsExcludedFromBuildOperation(property, operations))
                    continue;

                if (operations == SqlBuildOperations.Update)
                {
                    if (!this._changedPropertys.Contains(property.Name))
                        continue;
                }

                @return.Add(property);
            }
            return @return;
        }

        /// <summary>
        /// Checks if a Property needs to be excluded from the Statement build process
        /// </summary>
        /// <param name="property">The property which needs to be checked</param>
        /// <param name="operations">The Sql Operation which is executed</param>
        /// <returns><c>True</c> if the Property needs to be excluded, otherwise <c>False</c></returns>
        private bool GetIfPropertyIsExcludedFromBuildOperation(PropertyInfo property, SqlBuildOperations operations)
        {
            // Exclude the Property if the PKType is Autoincrement. It´s not needed to put it in the Insert Process
            if (property.IsDefined(typeof (PKAttribute)))
            {
                if (operations == SqlBuildOperations.Update)
                    return true;

                PKAttribute attri = property.GetCustomAttribute<PKAttribute>();
                if (attri.TypeOfPK == PKType.AutoIncrement)
                    return true;
            }

            // Exclude User Defined Properties for a Specific Build Operation
            if (property.IsDefined(typeof (ExcludeFromBuildOperationAttribute)))
            {
                ExcludeFromBuildOperationAttribute attri =
                    property.GetCustomAttribute<ExcludeFromBuildOperationAttribute>();
                if ((attri.Excludes & operations) == operations)
                    return true;
            }
            return false;
        }

        private List<PropertyInfo> GetEntityProperties()
        {
            return this.GetType().GetRuntimeProperties().Where(x => x.IsDefined(typeof (EntityColumnNameAttribute))).ToList();
        }

        private TableAttribute GetTableInformations()
        {
            if (this.GetType().GetTypeInfo().IsDefined(typeof (TableAttribute)))
            {
                TableAttribute tableAttri = this.GetType().GetTypeInfo().GetCustomAttribute<TableAttribute>();
                return tableAttri;
            }

            throw new InvalidOperationException("The class has no Table Attribute");
        }

        /// <summary>
        /// Gets the SqlValue for a Property
        /// </summary>
        /// <param name="property">The Property to get the SqlFormatted Value</param>
        /// <param name="dbType">The Database Type the SqlValue should be Formatted to</param>
        /// <returns>A Formatted SqlValue which can be Inserted in the Statement</returns>
        private string GetSqlValueForProperty(PropertyInfo property, DatabaseType dbType)
        {
            if(_formatterFunctions.ContainsKey(property.PropertyType))
                return _formatterFunctions[property.PropertyType].Invoke(dbType, property.GetValue(this));

            return _sqlValueFormatter.FormatValueToSqlValue(dbType, property.PropertyType, property.GetValue(this));
        }




    }
}
