using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DAL.Helper
{
    public static class MessageConvert
    {
        private static readonly JsonSerializerSettings Settings;

        static MessageConvert()
        {
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static string SerializeObject(this object obj)
        {
            return obj == null ? "" : JsonConvert.SerializeObject(obj, Settings);
        }

        public static T DeserializeObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public static object DeserializeObject(this string json, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, type, Settings);
            }
            catch
            {
                return null;
            }
        }
    }

    public static class CollectionHelper
    {
        public static IList<T> ConvertTo<T>(this DataTable table)
        {
            if (table == null) return null;

            return ConvertTo<T>(table.Rows.Cast<DataRow>().ToList());
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = new List<T>();

            if (rows != null)
            {
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = Activator.CreateInstance<T>();

            if (row != null)
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    if (prop == null || !prop.CanWrite) continue;  // Kiểm tra nếu thuộc tính có thể ghi

                    object value = row[column.ColumnName];
                    if (value != DBNull.Value)
                    {
                        try
                        {
                            // Kiểm tra kiểu dữ liệu và chuyển đổi
                            prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType), null);
                        }
                        catch (InvalidCastException ex)
                        {
                            Console.WriteLine($"Error converting column '{column.ColumnName}': {ex.Message}");
                        }
                    }
                }
            }

            return obj;
        }

        public static DataTable ConvertToDataTable<T>(this IList<T> list)
        {
            DataTable table = new DataTable(typeof(T).Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);  // Kiểm tra kiểu nullable
            }

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;  // Kiểm tra giá trị null
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
