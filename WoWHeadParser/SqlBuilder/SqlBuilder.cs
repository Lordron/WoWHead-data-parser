using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WoWHeadParser.Properties;

namespace Sql
{
    public enum SqlQueryType : byte
    {
        Update,
        Replace,
        Insert,
        InsertIgnore,
    }

    public class SqlBuilder
    {
        /// <summary>
        /// Gets a sql query type
        /// </summary>
        public SqlQueryType QueryType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to allow null values
        /// </summary>
        public bool AllowNullValue { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to allow append delete query
        /// </summary>
        public bool AppendDeleteQuery { get; private set; }

        /// <summary>
        /// Gets a value indication whether to allow append header to insert and replace query
        /// </summary>
        public bool WriteWithoutHeader { get; private set; }

        private string _tableName = string.Empty;

        private string _keyName = string.Empty;

        private List<string> _fields = new List<string>(64);

        private List<SqlItem> _items = new List<SqlItem>(64);

        private StringBuilder _content = new StringBuilder(8196);

        /// <summary>
        /// Initial Sql builder
        /// </summary>
        /// <param name="tableName">Table name (like creature_template, creature etc.)</param>
        /// <param name="keyName">Key name (like entry, id, guid etc.)</param>
        public SqlBuilder(string tableName, string keyName)
        {
            _tableName = tableName;
            _keyName = keyName;

            WriteWithoutHeader = Settings.Default.WithoutHeader;
            AppendDeleteQuery = Settings.Default.AppendDeleteQuery;
            AllowNullValue = Settings.Default.AllowEmptyValues;
            QueryType = Settings.Default.QueryType;
        }

        /// <summary>
        /// Initial Sql builder
        /// <param name="tableName">Table name (like creature_template, creature etc.)</param>
        /// </summary>
        public SqlBuilder(string tableName)
                : this(tableName, "entry")
        {
        }

        /// <summary>
        /// Append fields names
        /// </summary>
        /// <param name="args">fields name array</param>
        public void SetFieldsNames(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException();

            _fields.AddRange(args);
        }

        /// <summary>
        /// Append fields names
        /// </summary>
        /// <param name="args">Sequance</param>
        /// <param name="count">A specified mumber of contiguous elements from the start of a sequance</param>
        public void SetFieldsNames(IEnumerable<string> args, int count)
        {
            if (args == null)
                throw new ArgumentNullException();

            if (count == -1)
                throw new ArgumentOutOfRangeException();

            _fields.AddRange(args.Take(count));
        }

        /// <summary>
        /// Append fields names
        /// </summary>
        /// <param name="args">fields name array</param>
        public void SetFieldsName(string format, params object[] args)
        {
            if (args == null || string.IsNullOrEmpty(format))
                throw new ArgumentNullException();

            string field = string.Format(format, args);
            _fields.Add(field);
        }

        /// <summary>
        /// Append key and fields value 
        /// </summary>
        /// <param name="key">key value</param>
        /// <param name="args">string fields values array</param>
        public void AppendFieldsValue(object key, params string[] args)
        {
            if (key == null || args == null)
                throw new ArgumentNullException();

            List<string> values = new List<string>(args);
            _items.Add(new SqlItem(key, values));
        }

        /// <summary>
        /// Append key and fields value 
        /// </summary>
        /// <param name="key">key value</param>
        /// <param name="args">object fields values array</param>
        public void AppendFieldsValue(object key, params object[] args)
        {
            if (key == null || args == null)
                throw new ArgumentNullException();

            List<string> values = new List<string>(args.Length);
            for (int i = 0; i < args.Length; ++i)
            {
                values.Add(args[i].ToString());
            }

            _items.Add(new SqlItem(key, values));
        }

        /// <summary>
        /// Append key and fields value 
        /// </summary>
        /// <param name="key">key value</param>
        /// <param name="args">object fields values array</param>
        /// <param name="count">A specified mumber of contiguous elements from the start of a sequance</param>
        public void AppendFieldsValue(object key, IEnumerable<string> args, int count)
        {
            if (key == null || args == null)
                throw new ArgumentNullException();

            if (count == -1)
                throw new ArgumentOutOfRangeException();

            List<string> values = new List<string>(args.Take(count));

            _items.Add(new SqlItem(key, values));
        }

        /// <summary>
        /// Append sql query
        /// </summary>
        /// <param name="query"></param>
        public void AppendSqlQuery(string query, params object[] args)
        {
            if (args == null || string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException();

            _content.AppendLine(string.Format(query, args));
        }

        public bool Empty
        {
            get { return _items.Count <= 0; }
        }

        /// <summary>
        /// Build sql query
        /// </summary>
        public override string ToString()
        {
            if (Empty)
                return string.Empty;

            _content.Capacity = 2048 * _items.Count;

            switch (QueryType)
            {
                case SqlQueryType.Update:
                    return BuildUpdateQuery();
                case SqlQueryType.Replace:
                case SqlQueryType.Insert:
                case SqlQueryType.InsertIgnore:
                    return BuildReplaceInsertQuery();
                default:
                    return string.Empty;
            }
        }

        private string BuildUpdateQuery()
        {
            for (int i = 0; i < _items.Count; ++i)
            {
                bool notEmpty = false;

                SqlItem item = _items[i];

                StringBuilder contentInternal = new StringBuilder(1024);
                {
                    contentInternal.AppendFormat("UPDATE `{0}` SET ", _tableName);
                    for (int j = 0; j < item.Count; ++j)
                    {
                        if (!AllowNullValue && string.IsNullOrWhiteSpace(item[j]))
                            continue;

                        contentInternal.AppendFormat(NumberFormatInfo.InvariantInfo, "`{0}` = '{1}', ", _fields[j], item[j]);
                        notEmpty = true;
                    }
                    contentInternal.Remove(contentInternal.Length - 2, 2);
                    contentInternal.AppendFormat(" WHERE `{0}` = {1};", _keyName, item.Key).AppendLine();

                    if (notEmpty)
                        _content.Append(contentInternal.ToString());
                }
            }

            return _content.ToString();
        }

        private string BuildReplaceInsertQuery()
        {
            if (AppendDeleteQuery)
                _content.AppendFormat("DELETE FROM `{0}` WHERE `{1}` = '{2}';", _tableName, _keyName, _items[0].Key).AppendLine();

            switch (QueryType)
            {
                case SqlQueryType.Insert:
                    _content.AppendFormat("INSERT INTO `{0}`", _tableName);
                    break;
                case SqlQueryType.InsertIgnore:
                    _content.AppendFormat("INSERT IGNORE INTO `{0}`", _tableName);
                    break;
                case SqlQueryType.Replace:
                    _content.AppendFormat("REPLACE INTO `{0}`", _tableName);
                    break;
            }

            if (!WriteWithoutHeader)
            {
                _content.AppendFormat(" (`{0}`, ", _keyName);

                for (int i = 0; i < _fields.Count; ++i)
                    _content.AppendFormat("`{0}`, ", _fields[i]);

                _content.Remove(_content.Length - 2, 2);
                _content.Append(")");
            }
            _content.AppendLine(" VALUES");

            for (int i = 0; i < _items.Count; ++i)
            {
                SqlItem item = _items[i];

                _content.AppendFormat("('{0}', ", item.Key);
                for (int j = 0; j < item.Count; ++j)
                {
                    _content.AppendFormat(NumberFormatInfo.InvariantInfo, "'{0}', ", item[j]);
                }
                _content.Remove(_content.Length - 2, 2);
                _content.AppendFormat("){0}", i < _items.Count - 1 ? "," : ";").AppendLine();
            }

            return _content + Environment.NewLine;
        }
    }
}