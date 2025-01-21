using System.Reflection;

namespace INTERNAL_SOURCE_LOAD.Services
{
    public static class SqlInsertGenerator
    {
        public static string GenerateInsertQuery<T>(string tableName, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var columns = new List<string>();
            var values = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(data);

                // Skip complex types (e.g., collections or nested objects)
                if (value != null && IsSimpleType(property.PropertyType))
                {
                    columns.Add(property.Name);
                    values.Add(ConvertToSqlValue(value));
                }
            }

            var columnsPart = string.Join(", ", columns);
            var valuesPart = string.Join(", ", values);

            return $"INSERT INTO {tableName} ({columnsPart}) VALUES ({valuesPart});";
        }

        public static string GenerateInsertQueries<T>(string tableName, IEnumerable<T> dataList)
        {
            if (dataList == null || !dataList.Any())
                throw new ArgumentException("The data list is null or empty.", nameof(dataList));

            var queries = dataList.Select(data => GenerateInsertQuery(tableName, data));
            return string.Join("\n", queries);
        }

        private static string ConvertToSqlValue(object value)
        {
            return value switch
            {
                string str => $"'{str.Replace("'", "''")}'", // Escape single quotes for strings
                DateTime dateTime => $"'{dateTime:yyyy-MM-dd HH:mm:ss}'",
                null => "NULL",
                _ => value.ToString() // Default handling for other types
            };
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive ||
                   type.IsEnum ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(Guid);
        }
    }

}
