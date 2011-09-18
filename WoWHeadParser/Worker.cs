using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    public class Worker
    {
        protected Parser _parser;
        protected uint _rangeStart;
        protected uint _rangeEnd;
        protected object _locale;
        protected uint _threadCount;
        protected Queue<string> _pages;

        public Queue<string> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }

        public object Locale
        {
            get { return _locale; }
        }

        public Parser Parser
        {
            get { return _parser; }
        }

        public Worker(Parser parser, uint rangeStart, uint rangeEnd, object locale, uint threadCount)
        {
            _parser = parser;
            _rangeStart = rangeStart;
            _rangeEnd = rangeEnd;
            _locale = locale;
            _threadCount = threadCount;
            _pages = new Queue<string>();
        }

        public void Start()
        {
            uint petThread = (_rangeEnd - _rangeStart) / _threadCount;
            for (uint i = 0; i < _threadCount; ++i)
            {
                uint start = _rangeStart + (petThread * i);
                uint end = _rangeStart + (petThread * (i + 1));
                Core core = new Core(start, end, this);
                Thread thread = new Thread(core.Start);
                thread.Start();
            }
        }
    }
}
