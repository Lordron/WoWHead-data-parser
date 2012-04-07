using System;
using System.Collections.Generic;
using System.Text;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public enum SqlQueryType
    {
        None,
        Update,
        Replace,
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

        private static List<object> _keys = new List<object>(64);

        private static List<string> _names = new List<string>(64);

        private static List<List<string>> _listValues = new List<List<string>>(64);

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
            _keys.Clear();
            _names.Clear();
            _listValues.Clear();

            KeyName = string.Empty;
            TableName = string.Empty;
        }

        /// <summary>
        /// Append key value
        /// </summary>
        /// <param name="key">Key value</param>
        public static void AppendKeyValue(object key)
        {
            if (key == null)
                throw new ArgumentNullException();

            _keys.Add(key);
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
                string name = args[i];
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException();

                _names.Add(name);
            }
        }

        /// <summary>
        /// Append fields value
        /// </summary>
        /// <param name="key">key value</param>
        /// <param name="args">fields value array</param>
        public static void AppendFieldsValue(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException();

            List<string> values = new List<string>(64);
            for (int i = 0; i < args.Length; ++i)
            {
                values.Add(args[i]);
            }

            _listValues.Add(values);
        }

        /// <summary>
        /// Build sql query
        /// </summary>
        public static string ToString()
        {
            if (_listValues.Count != _keys.Count)
                throw new ArgumentOutOfRangeException();

            switch (QueryType)
            {
                case SqlQueryType.Update:
                    return BuildUpdateQuery();
                case SqlQueryType.Replace:
                    return BuildReplaceInsertQuery();
                case SqlQueryType.InsertIgnore:
                    return BuildReplaceInsertQuery(false);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string BuildUpdateQuery()
        {
            StringBuilder content = new StringBuilder(1024 * _listValues.Count);

            for (int i = 0; i < _listValues.Count; ++i)
            {
                bool notEmpty = false;

                List<string> values = _listValues[i];

                StringBuilder contentInternal = new StringBuilder(1024);
                {
                    contentInternal.AppendFormat("UPDATE `{0}` SET ", TableName);
                    for (int j = 0; j < values.Count; ++j)
                    {
                        if (string.IsNullOrWhiteSpace(values[j]) && !AllowNullValue)
                            continue;

                        contentInternal.AppendFormat("`{0}` = '{1}', ", _names[j], values[j]);
                        notEmpty = true;
                    }
                    contentInternal.Remove(contentInternal.Length - 2, 2);
                    contentInternal.AppendFormat(" WHERE `{0}` = {1};", KeyName, _keys[i]).AppendLine();

                    if (notEmpty)
                        content.Append(contentInternal.ToString());
                }
            }

            Reset();
            return content.ToString();
        }

        private static string BuildReplaceInsertQuery(bool replace = true)
        {
            StringBuilder content = new StringBuilder(1024);

            if (AppendDeleteQuery)
                content.AppendFormat("DELETE FROM `{0}` WHERE `{1}` = '{2}';", TableName, KeyName, (_keys.Count > 0 ? _keys[0] : 0)).AppendLine();

            if (replace)
                content.AppendFormat("REPLACE INTO `{0}` (`{1}`, ", TableName, KeyName);
            else
                content.AppendFormat("INSERT IGNORE INTO `{0}` (`{1}`, ", TableName, KeyName);

            for (int i = 0; i < _names.Count; ++i)
                content.AppendFormat("`{0}`, ", _names[i]);

            content.Remove(content.Length - 2, 2);
            content.AppendLine(") VALUES");

            bool notEmpty = false;

            for (int i = 0; i < _listValues.Count; ++i)
            {
                List<string> values = _listValues[i];

                content.AppendFormat("('{0}', ", _keys[i]);
                for (int j = 0; j < values.Count; ++j)
                {
                    content.AppendFormat("'{0}', ", values[j]);
                }
                content.Remove(content.Length - 2, 2);
                content.AppendFormat("){0}", i < _listValues.Count - 1 ? "," : ";").AppendLine();

                notEmpty = true;
            }

            Reset();
            return notEmpty ? content.AppendLine().ToString() : string.Empty;
        }
    }
}
