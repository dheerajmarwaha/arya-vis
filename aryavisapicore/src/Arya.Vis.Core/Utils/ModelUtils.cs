using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
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

                        if (p.PropertyType == typeof(Guid))
                        {
                            val = row[dbColName] == DBNull.Value ? Guid.Empty : new Guid(row.Field<String>(dbColName));
                        }
                        else if(p.PropertyType == typeof(Boolean))
                        {
                            val = Convert.ToBoolean(val);
                        }
                        {

                        }
                       
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
