using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cronus.Core.Data.Sql;

namespace Cronus.Core.Data
{
    public abstract class DataEntity
    {
        public string GetSelectCommand()
        {
            TableAttribute tableInfo = GetTableInformations();
            const string selectTemplate = "SELECT {0} FROM {1}";

            List<PropertyInfo> propertiesForOperation = this.GetPropertiesForOperation(SqlBuildOperation.Select);
            string sqlFieldsForOperation = this.GetSqlFieldsForOperation(propertiesForOperation);

            return string.Format(selectTemplate, sqlFieldsForOperation, tableInfo.ToString());
        }

        public string GetInsertCommand()
        {
            throw new NotImplementedException();
        }

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
                    string value = string.Empty;
                    tempWhere += string.Concat(entityInfo.ColumnName, " = ", value);
                    if (i != pkProps.Count() - 2)
                        tempWhere += " AND ";
                }
                whereCondition = tempWhere;
            }
            else if (pkProps.Count == 1)
            {
                EntityColumnNameAttribute entityInfo =
                    pkProps.ElementAt(0).GetCustomAttribute<EntityColumnNameAttribute>();
                string value = string.Empty;
                whereCondition += string.Concat(entityInfo.ColumnName, " = ", value);
            }
            else
            {
                //ToDo Implment Exception
                throw new DataEntityException();
            }


            return string.Format(deleteTemplate, tableInfo, whereCondition);
        }

        private string GetSqlFieldsForOperation(IEnumerable<PropertyInfo> properties)
        {
            string sqlFields = string.Empty;
            for (int i = 0; i < properties.Count(); i++)
            {
                PropertyInfo property = properties.ElementAt(i);
                EntityColumnNameAttribute columnAttri = property.GetCustomAttribute<EntityColumnNameAttribute>();
                sqlFields += columnAttri.ColumnName;
                
                if (i != properties.Count() - 2)
                    sqlFields += ", ";
            }
            return sqlFields;
        }

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

        private bool GetIfPropertyIsExcludedFromBuildOperation(PropertyInfo property, SqlBuildOperation operation)
        {
            if (property.IsDefined(typeof (ExcludeFromBuildOperationAttribute)))
            {
                ExcludeFromBuildOperationAttribute attri =
                    property.GetCustomAttribute<ExcludeFromBuildOperationAttribute>();
                if ((attri.Excludes & operation) == operation)
                    return true;
            }
            return true;
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
    }
}
