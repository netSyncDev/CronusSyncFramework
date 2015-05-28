using System;

namespace Cronus.Data.Sql.DataToSqlValueFormatters
{
    internal sealed class DefaultDataToSqlValueFormatter : IDataValueToSqlValueFormatter
    {
        public string FormatValueToSqlValue(DatabaseType dbType, Type sourcePropertyType, object value)
        {
            // Build the Sqlvalue 
            // Example 3.414 oder 'Alex'
            if (sourcePropertyType == typeof(float))
            {
                // Needed becaouse maybe you get Expo in the String
                float floatVal = (float) value;
                string floatString = floatVal.ToString("F");
                floatString = floatString.Replace(',', '.');
                return floatString;
            }

            if (sourcePropertyType == typeof(float?))
            {
                float? floatVal = (float?)value;
                if (floatVal.HasValue)
                {
                    string floatString = floatVal.Value.ToString("F");
                    floatString = floatString.Replace(',', '.');
                    return floatString;
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(int) || sourcePropertyType == typeof(double) || sourcePropertyType == typeof(Int64) || sourcePropertyType == typeof(Int16))
            {
                return value.ToString();
            }

            if (sourcePropertyType == typeof(Int64?))
            {
                Int64? tempValue = (Int64?)value;
                if (tempValue.HasValue)
                {
                    return tempValue.Value.ToString();
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(Int16?))
            {
                Int16? tempValue = (Int16)value;
                if (tempValue.HasValue)
                {
                    return tempValue.Value.ToString();
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(int?))
            {
                int? tempValue = (int?)value;
                if (tempValue.HasValue)
                {
                    return tempValue.Value.ToString();
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(double?))
            {
                double? tempValue = (double?)value;
                if (tempValue.HasValue)
                {
                    return tempValue.Value.ToString();
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(long?))
            {
                long? tempValue = (long?)value;
                if (tempValue.HasValue)
                {
                    return tempValue.Value.ToString();
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(string))
            {
                var tempValue = value;
                if (tempValue == null)
                    return "NULL";
                if (dbType == DatabaseType.MySql)
                    return "\"" + value.ToString() + "\"";

                // Escape the Single Quote Values for Sqlite and Mssql
                if (dbType == DatabaseType.Sqlite || dbType == DatabaseType.MsSql)
                {
                    if (value.ToString().Contains("'"))
                    {
                        value = value.ToString().Replace("'", "''");
                    }
                }
                return "'" + value.ToString() + "'";
            }


            if (sourcePropertyType == typeof(Guid))
            {
                Guid tempValue = (Guid)value;
                if (tempValue == Guid.Empty)
                    return "NULL";

                if (dbType == DatabaseType.MySql)
                    return "\"" + value.ToString() + "\"";

                return "'" + value.ToString() + "'";
            }
            if (sourcePropertyType == typeof(Guid?))
            {
                Guid? tempValue = (Guid?)value;
                if (tempValue.HasValue)
                {
                    if (dbType == DatabaseType.MySql)
                        return "\"" + tempValue.Value.ToString() + "\"";

                    return "'" + tempValue.Value.ToString() + "'";
                }
                return "NULL";
            }

            if (sourcePropertyType == typeof(DateTime))
            {
                string tempValue = "";
                DateTime date = (DateTime)value;

                // Format the Date for inserting into MSSQL Server
                if (dbType == DatabaseType.MsSql)
                {
                    tempValue = String.Format("{0:yyyy-MM-dd HH:mm:ss.ffffff}", date);
                    return "'" + tempValue + "'";
                }
                tempValue = String.Format("{0:yyyy-MM-dd HH:mm:ss.ffffff}", date);


                return "\"" + tempValue + "\"";
            }
            if (sourcePropertyType == typeof(DateTime?))
            {
                DateTime? date = (DateTime?)value;
                if (date.HasValue)
                {
                    string tempValue = "";
                    if (dbType == DatabaseType.MsSql)
                    {
                        tempValue = String.Format("{0:yyyy-MM-dd HH:mm:ss.ffffff}", date);
                        return "'" + tempValue + "'";
                    }
                    value = String.Format("{0:yyyy-MM-dd HH:mm:ss.ffffff}", date);
                    return "\"" + tempValue + "\"";
                }
                return "NULL";
            }
            if (sourcePropertyType == typeof(byte))
            {
                throw new NotImplementedException();
            }
            if (sourcePropertyType == typeof(byte?))
            {
                byte? tempValue = (byte?)value;
                if (tempValue.HasValue)
                {
                    return tempValue.ToString();
                }
                return "NULL";
            }
            if (sourcePropertyType == typeof(bool))
            {
                bool tempValue = (bool)value;
                if (tempValue == true)
                    return "1";
                else
                    return "0";
            }
            if (sourcePropertyType == typeof(bool?))
            {
                bool? tempValue = (bool?)value;
                if (tempValue.HasValue)
                {
                    if (tempValue.Value)
                        return "1";
                    else
                        return "0";
                }
                else
                    return "NULL";
            }
//            if (sourcePropertyType == typeof(XmlDocument))
//            {
//                XmlDocument value = (XmlDocument)this.GetType().GetProperty(prop.Name).GetValue(this);
//                using (StringWriter stringWriter = new StringWriter())
//                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
//                {
//                    value.WriteTo(xmlTextWriter);
//                    xmlTextWriter.Flush();
//                    string xmlString = stringWriter.GetStringBuilder().ToString();

//                    // Escape the Single Quote Values for Sqlite and Mssql
//#if WINDOWS
//                    if (dbType == DatabaseType.Sqlite || dbType == DatabaseType.Mssql)
//#else
//                    if (dbType == DatabaseType.Sqlite)
//#endif
//                    {
//                        if (xmlString.Contains("'"))
//                        {
//                            xmlString = xmlString.Replace("'", "''");
//                        }
//                    }
//                    return string.Format("'{0}'", xmlString);
//                }
//            }

            return "";
        }
    }
}
