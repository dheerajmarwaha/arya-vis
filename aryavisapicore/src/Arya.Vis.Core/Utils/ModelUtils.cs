using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Utils
{
    public class ModelUtils<T> where T : class
    {
        public static T CreateObject(DataTable table)
        {
            List<T> lst = CreateObjects(table);
            return lst.Count > 0 ? lst[0] : null;
        }

        public static List<T> CreateObjects(DataTable table)
        {
            List<T> lstObjects = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                lstObjects.Add(CreateObject(row));
            }
            return lstObjects;
        }
        public static T CreateObject(DataRow row)
        {
            try
            {
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                DataTable table = row.Table;
                T obj = (T)Activator.CreateInstance(type);

                foreach (PropertyInfo p in properties)
                {
                    try
                    {
                        string dbColName = p.Name;
                        if (!table.Columns.Contains(dbColName))
                        {
                            continue;
                        }
                        object val = row[dbColName] == DBNull.Value ? null : row[dbColName];
                        p.SetValue(obj, val);
                    }
                    catch
                    {
                        throw;
                    }
                }
                return obj;
            }
            catch
            {
                throw;
            }
        }
    }
}
