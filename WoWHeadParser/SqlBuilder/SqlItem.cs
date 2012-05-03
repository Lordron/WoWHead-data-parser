using System;
using System.Collections.Generic;

namespace Sql
{
    public class SqlItem
    {
        private List<string> _values = new List<string>(64);

        public object Key { get; private set; }

        /// <summary>
        /// Gets the number of elements actually contained in the list
        /// </summary>
        public int Count { get { return _values.Count; } }

        public SqlItem(object key, List<string> values)
        {
            Key = key;
            _values = values;
        }

        public string this[int x]
        {
            get
            {
                if (x >= Count)
                    throw new IndexOutOfRangeException();

                return _values[x];
            }
        }
    }
}
