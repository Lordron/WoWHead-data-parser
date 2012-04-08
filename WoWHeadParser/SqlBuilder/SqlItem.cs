using System;
using System.Collections.Generic;

namespace WoWHeadParser
{
    public class SqlItem
    {
        private object _key = new object();
        private List<string> _values = new List<string>(64);

        public SqlItem(object key, List<string> values)
        {
            _key = key;
            _values = values;
        }

        public string this[int x]
        {
            get
            {
                if (x >= _values.Count)
                    throw new IndexOutOfRangeException();

                return _values[x];
            }
        }

        /// <summary>
        /// Gets a key
        /// </summary>
        public object Key { get { return _key; } }

        /// <summary>
        /// Gets the number of elements actually contained in the list
        /// </summary>
        public int Count { get { return _values.Count; } }
    }
}
