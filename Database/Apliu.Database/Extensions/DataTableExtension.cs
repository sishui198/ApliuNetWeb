using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Apliu.Database.Extensions
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 将DataTable转换为标准的CSV
        /// </summary>
        public static string ToCsv(this DataTable dataTable)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    colum = dataTable.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static T[] ToArray<T>(this DataTable dataTable)
            where T : class, new()
        {
            if (dataTable == null)
                return null;

            var ps = typeof(T).GetProperties().ToDictionary(k => k.Name, v => v);
            var ts = new List<T>();
            foreach (DataRow r in dataTable.Rows)
            {
                var t = Activator.CreateInstance<T>();
                foreach (DataColumn c in dataTable.Columns)
                {
                    PropertyInfo p;
                    if (ps.TryGetValue(c.ColumnName, out p))
                    {
                        var v = r[c.ColumnName];
                        try
                        {
                            p.SetValue(t, v);
                        }
                        catch { }
                    }
                }

            }
            return ts.ToArray();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string tableName = null)
            where T : class
        {
            if (source == null)
                return null;

            DataTable dataTable = new DataTable(tableName);
            var ps = typeof(T).GetProperties();
            foreach (var p in ps)
            {
                dataTable.Columns.Add(p.Name, p.PropertyType);
            }

            foreach (var s in source)
            {
                var r = dataTable.NewRow();
                foreach (var p in ps)
                {
                    r[p.Name] = p.GetValue(s);
                }
                dataTable.Rows.Add(r);
            }

            return null;
        }
    }
}
