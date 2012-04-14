using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public enum SqlQueryType : byte
    {
        None,
        Update,
        Replace,
        Insert,
        InsertIgnore,
    }

    public static class SqlBuilder
    {
        /// <summary>
        /// Gets a sql query type
        /// </summary>
        public static SqlQueryType QueryType { get; private set; }

        /// <summary>
        /// Gets a table name
        /// </summary>
        public static string TableName { get; private set; }

        /// <summary>
        /// Gets a key name
        /// </summary>
        public static string KeyName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to allow null values
        /// </summary>
        public static bool AllowNullValue { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to allow append delete query
        /// </summary>
        public static bool AppendDeleteQuery { get; private set; }

        private static List<string> _fields = new List<string>(64);

        private static List<SqlItem> _items = new List<SqlItem>(64);

        /// <summary>
        /// Initial Sql builder
        /// </summary>
        /// <param name="tableName">Table name (like creature_template, creature etc.)</param>
        /// <param name="keyName">Key name (like entry, id, guid etc.)</param>
        public static void Initial(string tableName, string keyName = "entry")
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(keyName))
                throw new ArgumentNullException();

            TableName = tableName;
            KeyName = keyName;

            AppendDeleteQuery = Settings.Default.AppendDeleteQuery;
            AllowNullValue = Settings.Default.AllowEmptyValues;
            QueryType = (SqlQueryType)Settings.Default.QueryType;

            if (QueryType == SqlQueryType.None)
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Reset Sql builder datas
        /// </summary>
        public static void Reset()
        {
            _fields.Clear();
            _items.Clear();

            KeyName = string.Empty;
            TableName = string.Empty;
        }

        /// <summary>
        /// Append fields name
        /// </summary>
        /// <param name="args">fields name array</param>
        public static void SetFieldsName(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException();

            for (int i = 0; i < args.Length; ++i)
            {
                _fields.Add(args[i]);
            }
        }

        /// <summary>
        /// Append key and fields value 
        /// </summary>
        /// <param name="key">key value</param>
        /// <param name="args">fields value array</param>
        public static void AppendFieldsValue(object key, params string[] args)
        {
            if (key == null)
                throw new ArgumentNullException();

            if (args == null)
                throw new ArgumentNullException();

            List<string> values = new List<string>(args.Length);
            for (int i = 0; i < args.Length; ++i)
            {
                values.Add(args[i]);
            }

            _items.Add(new SqlItem(key, values));
        }

        /// <summary>
        /// Build sql query
        /// </summary>
        public static string ToString()
        {
            switch (QueryType)
            {
                case SqlQueryType.Update:
                    return BuildUpdateQuery();
                case SqlQueryType.Replace:
                case SqlQueryType.Insert:
                case SqlQueryType.InsertIgnore:
                    return BuildReplaceInsertQuery();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string BuildUpdateQuery()
        {
            StringBuilder content = new StringBuilder(1024 * _items.Count);

            for (int i = 0; i < _items.Count; ++i)
            {
                bool notEmpty = false;

                SqlItem item = _items[i];

                StringBuilder contentInternal = new StringBuilder(1024);
                {
                    contentInternal.AppendFormat("UPDATE `{0}` SET ", TableName);
                    for (int j = 0; j < item.Count; ++j)
                    {
                        if (string.IsNullOrWhiteSpace(item[j]) && !AllowNullValue)
                            continue;

                        contentInternal.AppendFormat(NumberFormatInfo.InvariantInfo, "`{0}` = '{1}', ", _fields[j], item[j]);
                        notEmpty = true;
                    }
                    contentInternal.Remove(contentInternal.Length - 2, 2);
                    contentInternal.AppendFormat(" WHERE `{0}` = {1};", KeyName, item.Key).AppendLine();

                    if (notEmpty)
                        content.Append(contentInternal.ToString());
                }
            }

            Reset();
            return content.ToString();
        }

        private static string BuildReplaceInsertQuery()
        {
            StringBuilder content = new StringBuilder(1024);

            if (AppendDeleteQuery)
                content.AppendFormat("DELETE FROM `{0}` WHERE `{1}` = '{2}';", TableName, KeyName, (_items.Count > 0 ? _items[0].Key : 0)).AppendLine();

            switch (QueryType)
            {
                case SqlQueryType.Insert:
                    content.AppendFormat("INSERT INTO `{0}` (`{1}`, ", TableName, KeyName);
                    break;
                case SqlQueryType.InsertIgnore:
                    content.AppendFormat("INSERT IGNORE INTO `{0}` (`{1}`, ", TableName, KeyName);
                    break;
                case SqlQueryType.Replace:
                    content.AppendFormat("REPLACE INTO `{0}` (`{1}`, ", TableName, KeyName);
                    break;
            }

            for (int i = 0; i < _fields.Count; ++i)
                content.AppendFormat("`{0}`, ", _fields[i]);

            content.Remove(content.Length - 2, 2);
            content.AppendLine(") VALUES");

            for (int i = 0; i < _items.Count; ++i)
            {
                SqlItem item = _items[i];

                content.AppendFormat("('{0}', ", item.Key);
                for (int j = 0; j < item.Count; ++j)
                {
                    content.AppendFormat(NumberFormatInfo.InvariantInfo, "'{0}', ", item[j]);
                }
                content.Remove(content.Length - 2, 2);
                content.AppendFormat("){0}", i < _items.Count - 1 ? "," : ";").AppendLine();
            }

            Reset();
            return _items.Count > 0 ? content.AppendLine().ToString() : string.Empty;
        }
    }
}
