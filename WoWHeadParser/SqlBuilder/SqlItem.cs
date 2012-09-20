using System;

namespace Sql
{
    public struct SqlItem
    {
        private int _pos;
        private object[] _values;

        public SqlItem(int count)
        {
            if (count == -1)
                throw new ArgumentOutOfRangeException("count");

            _pos = 0;
            _values = new object[count];
        }

        public void AddValue(object value)
        {
            _values[_pos++] = value;
        }

        public void AddValues(params object[] values)
        {
            for (int i = 0; i < values.Length; ++i)
            {
                _values[_pos++] = values[i];
            }
        }

        public string this[int x]
        {
            get
            {
                if (x < 0 || x >= _pos)
                    throw new IndexOutOfRangeException("x");

                object obj = _values[x];
                if (obj == null)
                    throw new ArgumentNullException("obj");

                string str = obj.ToString();
                if ((obj is char) || (obj is string) || (obj is float) || (obj is double))
                    return string.Format("{0}{1}{0}", @"'", str);

                return str;
            }
        }
    }
}