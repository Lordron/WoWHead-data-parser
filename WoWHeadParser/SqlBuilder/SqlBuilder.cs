using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WoWHeadParser.Properties;

namespace Sql
{
    /// <summary>
    /// Represent a simple SQL Builder
    /// </summary>
    public class SqlBuilder
    {
        /// <summary>
        /// Gets a <see cref="Sql.SqlBuilderSettings"/> that contain settings of the current <see cref="Sql.SqlBuilder"/>
        /// </summary>
        public SqlBuilderSettings BuilderSettings;

        private string _tableName = string.Empty;

        private string _keyName = string.Empty;

        private object _key;

        private SqlItem _sqlItem;

        private int _itemCount = -1;

        private bool _hasPreparedQueries = false;

        private string[] _tableFields;

        private Dictionary<object, List<string>> _querys;

        private Dictionary<object, List<SqlItem>> _items = new Dictionary<object, List<SqlItem>>();

        private Dictionary<object, List<SqlItem>> _sortedDictionary;

        /// <summary>
        /// Initialize <see cref="Sql.SqlBuilder"/> with specific <see cref="Sql.SqlBuilderSettings"/>
        /// </summary>
        /// <param name="settings"><see cref="Sql.SqlBuilderSettings"/> of the current <see cref="Sql.SqlBuilder"/></param>
        public SqlBuilder()
        {
            BuilderSettings = new SqlBuilderSettings(Settings.Default.QueryType, Settings.Default.WithoutHeader, Settings.Default.AllowEmptyValues, Settings.Default.AppendDeleteQuery);;
        }

        /// <summary>
        /// Setup <see cref="Sql.SqlBuilder"/>
        /// </summary>
        /// <param name="tableName">Table name (like creature_template, creature etc.)</param>
        /// <param name="key">Key name from table</param>
        /// <param name="hasPreparedQueries">Value indicating whether to create a query storage/param>
        /// <param name="fields">Fields name</param>
        public void Setup(string tableName, string key, bool hasPreparedQueries, params string[] fields)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (fields == null)
                throw new ArgumentNullException("fields");

            Setup(tableName, key, hasPreparedQueries, fields, fields.Length);
        }

        /// <summary>
        /// Setup <see cref="Sql.SqlBuilder"/>
        /// </summary>
        /// <param name="tableName">Table name (like creature_template, creature etc.)</param>
        /// <param name="key">Key name from table</param>
        /// <param name="fields">Sequance</param>
        /// <param name="hasPreparedQueries">Value indicating whether to create a query storage/param>
        /// <param name="count">A specified mumber of contiguous elements from the start of a sequance</param>
        public void Setup(string tableName, string key, bool hasPreparedQueries, string[] fields, int count)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (fields == null)
                throw new ArgumentNullException("fields");

            if (count == -1)
                throw new ArgumentOutOfRangeException("count");

            _keyName = key;
            _tableName = tableName;

            _tableFields = fields;

            _itemCount = count;
            _sqlItem = new SqlItem(_itemCount);

            if ((_hasPreparedQueries = hasPreparedQueries))
                _querys = new Dictionary<object, List<string>>(1024);
        }

        /// <summary>
        /// Set key
        /// </summary>
        /// <param name="key">Key value</param>
        public void SetKey(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (!_items.ContainsKey(key))
                _items.Add(key, new List<SqlItem>(32));

            if (_hasPreparedQueries)
            {
                if (!_querys.ContainsKey(key))
                    _querys.Add(key, new List<string>(2));
            }

            _key = key;
        }

        /// <summary>
        /// Append value 
        /// </summary>
        /// <param name="value">Value</param>
        public void AppendValue(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _sqlItem.AddValue(value);
        }

        /// <summary>
        /// Append values
        /// </summary>
        /// <param name="values">Values</param>
        public void AppendValues(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException("value");

            _sqlItem.AddValues(values);
        }

        /// <summary>
        /// Append values
        /// </summary>
        /// <param name="values">Values</param>
        public void AppendListValues(IEnumerable<object> values)
        {
            AppendValues(values, values.Count());
        }

        /// <summary>
        /// Append values
        /// </summary>
        /// <param name="values">Values</param>
        /// <param name="count">A specified mumber of contiguous elements from the start of a sequance</param>
        public void AppendValues(IEnumerable<object> values, int count)
        {
            if (values == null)
                throw new ArgumentNullException("value");

            if (count == -1)
                throw new ArgumentOutOfRangeException("count");

            AppendValues(values.Take(count));
        }

        /// <summary>
        /// Append prepared sql query
        /// </summary>
        /// <param name="query"></param>
        public void AppendSqlQuery(object key, string format, params object[] args)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException("query");

            if (args == null)
                throw new ArgumentNullException("args");

            if (!_hasPreparedQueries)
                throw new InvalidOperationException("_hasPreparedQueries");

            string query = string.Format(format, args);
            if (!_querys[key].Contains(query))
                _querys[key].Add(query);
        }

        /// <summary>
        /// Flush <see cref="Sql.SqlBuilder"/>
        /// </summary>
        public void Flush()
        {
            _items[_key].Add(_sqlItem);
            _sqlItem = new SqlItem(_itemCount);
        }

        /// <summary>
        /// Gets the number of elements actually contained in the Dictionary
        /// </summary>
        public int Count { get { return _items.Count; } }

        /// <summary>
        /// Build sql query
        /// </summary>
        public override string ToString()
        {
            if (Count <= 0)
                return string.Empty;

            _sortedDictionary = new Dictionary<object, List<SqlItem>>(_items, new SqlItemComparer(Settings.Default.SortOrder));

            StringBuilder content = new StringBuilder(256 * _items.Count);

            switch (BuilderSettings.QueryType)
            {
                case SqlQueryType.Update:
                    BuildUpdateQuery(content);
                    break;
                case SqlQueryType.Replace:
                case SqlQueryType.Insert:
                case SqlQueryType.InsertIgnore:
                    BuildReplaceInsertQuery(content);
                    break;
            }

            return content.ToString();
        }

        private void BuildUpdateQuery(StringBuilder content)
        {
            foreach (KeyValuePair<object, List<SqlItem>> kvp in _sortedDictionary)
            {
                object key = kvp.Key;
                List<SqlItem> items = kvp.Value;

                AppendQuery(key, content);
                foreach(SqlItem item in items)
                {
                    bool isEmpty = true;

                    StringBuilder contentInternal = new StringBuilder(1024);
                    {
                        contentInternal.AppendFormat("UPDATE `{0}` SET ", _tableName);
                        for (int j = 0; j < _itemCount; ++j)
                        {
                            if (!BuilderSettings.AllowNullValue && string.IsNullOrWhiteSpace(item[j]))
                                continue;

                            contentInternal.AppendFormat(NumberFormatInfo.InvariantInfo, "`{0}` = {1}, ", _tableFields[j], item[j]);
                            isEmpty = false;
                        }
                        contentInternal.Remove(contentInternal.Length - 2, 2);
                        contentInternal.AppendFormat(" WHERE `{0}` = {1};", _keyName, key).AppendLine();

                        if (!isEmpty)
                            content.Append(contentInternal.ToString());
                    }
                }
                content.AppendLine();
            }
        }

        private void BuildReplaceInsertQuery(StringBuilder content)
        {
            int objectCount = 0;

            if (_hasPreparedQueries)
            {
                foreach (object key in _sortedDictionary.Keys)
                {
                    if (!_querys.ContainsKey(key))
                        continue;

                    foreach (string query in _querys[key])
                    {
                        if (!string.IsNullOrEmpty(query))
                            content.AppendLine(query);
                    }
                }
                content.AppendLine();
            }

            if (BuilderSettings.AppendDeleteQuery)
            {
                content.AppendFormat("DELETE FROM `{0}` WHERE `{1}` IN (", _tableName, _keyName);
                foreach (object key in _sortedDictionary.Keys)
                {
                    content.AppendFormat("{0}{1}", key, (objectCount++ < _sortedDictionary.Count - 1) ? ", " : ");");
                }
                content.AppendLine().AppendLine();
            }

            switch (BuilderSettings.QueryType)
            {
                case SqlQueryType.Insert:
                    content.AppendFormat("INSERT INTO `{0}`", _tableName);
                    break;
                case SqlQueryType.InsertIgnore:
                    content.AppendFormat("INSERT IGNORE INTO `{0}`", _tableName);
                    break;
                case SqlQueryType.Replace:
                    content.AppendFormat("REPLACE INTO `{0}`", _tableName);
                    break;
            }

            if (!BuilderSettings.WriteWithoutHeader)
            {
                content.AppendFormat(" (`{0}`, ", _keyName);

                for (int i = 0; i < _itemCount; ++i)
                    content.AppendFormat("`{0}`{1}", _tableFields[i], (i < _itemCount - 1) ? ", " : ")");
            }
            content.AppendLine(" VALUES");

            objectCount = 0;

            foreach (KeyValuePair<object, List<SqlItem>> kvp in _sortedDictionary)
            {
                object key = kvp.Key;
                List<SqlItem> items = kvp.Value;

                for (int i = 0; i < items.Count; ++i)
                {
                    SqlItem item = items[i];

                    content.AppendFormat("({0}, ", key);
                    for (int j = 0; j < _itemCount; ++j)
                    {
                        content.AppendFormat(NumberFormatInfo.InvariantInfo, "{0}{1}", item[j], (j < _itemCount - 1) ? ", " : string.Empty);
                    }

                    content.AppendFormat("){0}", objectCount++ < _sortedDictionary.Count - 1 ? "," : ";").AppendLine();
                }
            }
        }

        private void AppendQuery(object key, StringBuilder content)
        {
            if (!_hasPreparedQueries)
                return;

            if (!_querys.ContainsKey(key))
                return;

            foreach (string query in _querys[key])
            {
                if (!string.IsNullOrEmpty(query))
                    content.AppendLine(query);
            }
        }
    }
}
