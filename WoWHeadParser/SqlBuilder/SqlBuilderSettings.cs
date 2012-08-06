
namespace Sql
{
    public struct SqlBuilderSettings
    {
        /// <summary>
        /// Gets a <see cref="Sql.SqlQueryType"/> of the current <see cref="Sql.SqlBuilderSettings"/>
        /// </summary>
        public SqlQueryType QueryType;

        /// <summary>
        /// Gets a value indicating whether to allow print null values
        /// </summary>
        public bool AllowNullValue;

        /// <summary>
        /// Gets a value indicating whether to allow append delete query
        /// </summary>
        public bool AppendDeleteQuery;

        /// <summary>
        /// Gets a value indication whether to allow append header for insert and replace query
        /// </summary>
        public bool WriteWithoutHeader;

        public SqlBuilderSettings(SqlQueryType queryType, bool withoutHeader, bool allowNullValue, bool appendDeleteQuery)
        {
            QueryType = queryType;
            AllowNullValue = allowNullValue;
            WriteWithoutHeader = withoutHeader;
            AppendDeleteQuery = appendDeleteQuery;
        }
    }
}
