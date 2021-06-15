using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppLibrary.Database
{
    public static class DBHelper
    {
        public static List<T> ToObjectList<T>(this DataTable dataTable) where T : new()
        {
            List<T> objList = new List<T>();
            try
            {
                List<PropertyInfo> _propertyInfos = typeof(T).GetProperties().ToList();
                List<DataColumn> _columns = dataTable.Columns.Cast<DataColumn>().ToList();

                foreach (DataRow row in dataTable.Rows)
                {
                    T item = new T();
                    bool hasValue = false;
                    foreach (PropertyInfo property in _propertyInfos)
                    {
                        DataColumn column;
                        if ((column = _columns.Find(f => f.ColumnName.ToUpper() == property.Name.ToUpper())) != null)
                        {
                            hasValue = hasValue || true;
                            if (row[column.ColumnName] != DBNull.Value)
                                property.SetValue(item, row[column.ColumnName], null);
                        }
                    }
                    if (hasValue)
                        objList.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objList;
        }
    }
}
