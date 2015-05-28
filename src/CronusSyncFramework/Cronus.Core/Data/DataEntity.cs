using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cronus.Data.Sql;
using Cronus.Data.Sql.DataToSqlValueFormatters;

namespace Cronus.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataEntity
    {
        private static Dictionary<Type, Func<DatabaseType, object, string>> _formatterFunctions;
        private static IDataValueToSqlValueFormatter _sqlValueFormatter;

        private static DatabaseType _targetDatabaseType;

        /// <summary>
        /// 
        /// </summary>
        protected DataEntity()
        {
            _sqlValueFormatter = new DefaultDataToSqlValueFormatter();
            _formatterFunctions = new Dictionary<Type, Func<DatabaseType, object, string>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToRegister"></param>
        /// <param name="formatFunction"></param>
        /// <param name="shouldOverwrite"></param>
        /// <exception cref="ArgumentException">If a ConvertFunction for this Type is already Found and shouldOverwrite = <c>False</c></exception>
        public static void RegisterSqlDataValueFormatFunction(Type typeToRegister,
            Func<DatabaseType, object, string> formatFunction, bool shouldOverwrite)
        {
            if (_formatterFunctions.ContainsKey(typeToRegister) && !shouldOverwrite)
                throw new ArgumentException(
                    string.Format("The Key {0} was already found. Use shouldOverwrite to Overwrite the Existing Key",
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
        /// <param name="buildOperation">The Executed Build Operation</param>
        protected virtual void OnBeforeStatementBuilded(SqlBuildOperation buildOperation)
        {
            
        }

        /// <summary>
        /// Generates a Select Command for an Entity
        /// </summary>
        /// <returns>A Sql Statement which can be Executed</returns>
        public string GetSelectCommand()
        {
            TableAttribute tableInfo = GetTableInformations();
            const string selectTemplate = "SELECT {0} FROM {1}";

            List<PropertyInfo> propertiesForOperation = this.GetPropertiesForOperation(SqlBuildOperation.Select);
            string sqlFieldsForOperation = this.GetSqlFieldsForOperation(propertiesForOperation);

            return string.Format(selectTemplate, sqlFieldsForOperation, tableInfo.ToString());
        }

        /// <summary>
        /// Gets the Insert Command for a Entity
        /// </summary>
        /// <returns>Insert Sql Statement for this Entity</returns>
        public string GetInsertCommand()
        {
            this.OnBeforeStatementBuilded(SqlBuildOperation.Insert);
            TableAttribute tableInfo = GetTableInformations();
            const string InsertTemplate = "INSERT INTO {0} ({1}) VALUES ({2});";

            List<PropertyInfo> propertiesForOperation = this.GetPropertiesForOperation(SqlBuildOperation.Insert);
            string sqlFieldsForOperation = this.GetSqlFieldsForOperation(propertiesForOperation);

            string insertValues = string.Empty;

            for (int i = 0; i < propertiesForOperation.Count; i++)
            {
                insertValues += this.GetSqlValueForProperty(propertiesForOperation.ElementAt(i), _targetDatabaseType);
                if (i != propertiesForOperation.Count - 1)
                    insertValues += ", ";
            }

            return string.Format(InsertTemplate, tableInfo, sqlFieldsForOperation, insertValues);
        }

        /// <summary>
        /// Gets the Delete Command for a Entity
        /// </summary>
        /// <returns>A Sql Statement which deletes this Data Entity</returns>
        public string GetDeleteCommand()
        {
            TableAttribute tableInfo = GetTableInformations();
            const string deleteTemplate = "DELTE FROM {0} WHERE {1}";

            string whereCondition = String.Empty;

            List<PropertyInfo> pkProps =
                this.GetEntityProperties().Where(x => x.IsDefined(typeof(PkAttribute))).ToList();

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
                //ToDo Implment Exception
                //throw new DataEntityException();
            }
            return string.Format(deleteTemplate, tableInfo, whereCondition);
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
        /// <param name="operation">The Sql Build Operation</param>
        /// <returns>A list of Properties which are icluded in the Build Process of the Statement</returns>
        private List<PropertyInfo> GetPropertiesForOperation(SqlBuildOperation operation)
        {
            List<PropertyInfo> @return = new List<PropertyInfo>();
            List<PropertyInfo> entityProperties = this.GetEntityProperties();
            for (int i = 0; i < entityProperties.Count(); i++)
            {
                PropertyInfo property = entityProperties.ElementAt(i);
                if(GetIfPropertyIsExcludedFromBuildOperation(property, operation))
                    continue;
                @return.Add(property);
            }
            return @return;
        }

        /// <summary>
        /// Checks if a Property needs to be excluded from the Statement build process
        /// </summary>
        /// <param name="property">The property which needs to be checked</param>
        /// <param name="operation">The Sql Operation which is executed</param>
        /// <returns><c>True</c> if the Property needs to be excluded, otherwise <c>False</c></returns>
        private bool GetIfPropertyIsExcludedFromBuildOperation(PropertyInfo property, SqlBuildOperation operation)
        {
            // Exclude the Property if the PkType is Autoincrement. It´s not needed to put it in the Insert Process
            if (property.IsDefined(typeof (PkAttribute)))
            {
                PkAttribute attri = property.GetCustomAttribute<PkAttribute>();
                if (attri.PkType == PkAttributeType.AutoIncrement)
                    return true;
            }

            // Exclude User Defined Properties for a Specific Build Operation
            if (property.IsDefined(typeof (ExcludeFromBuildOperationAttribute)))
            {
                ExcludeFromBuildOperationAttribute attri =
                    property.GetCustomAttribute<ExcludeFromBuildOperationAttribute>();
                if ((attri.Excludes & operation) == operation)
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

            throw new InvalidOperationException("The class has no TableAttribute");
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
