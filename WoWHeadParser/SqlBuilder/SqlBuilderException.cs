using System;

namespace Sql
{
    [Serializable]
    internal class InvalidQueryTypeException : Exception
    {
        public InvalidQueryTypeException()
        {
        }

        public InvalidQueryTypeException(SqlQueryType type)
                : base(type.ToString())
        {
        }

        public InvalidQueryTypeException(string message)
                : base(message)
        {
        }

        public InvalidQueryTypeException(string message, params object[] args)
                : base(string.Format(message, args))
        {
        }

        public override string Message { get { return string.Format(@"{0}: Invalid SqlQueryType value! Query type value must be in the interval from SqlQueryType.None to SqlQueryType.Max", Source); } }

        public override string ToString()
        {
            return Message;
        }
    }
}