using System.Collections.Generic;
using System.Threading;

namespace WoWHeadParser
{
    public class Worker
    {
        protected uint _rangeStart;
        protected uint _rangeEnd;
        protected object _locale;
        protected uint _threadCount;
        protected Queue<Block> _pages;
        protected string _address;

        public event WoWHeadParser.WoWHeadParserForm.DownloaderProgressHandler OnProgressChanged;

        public Queue<Block> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }

        public object Locale
        {
            get { return _locale; }
        }

        public string Address
        {
            get { return _address; }
        }

        public Worker(uint rangeStart, uint rangeEnd, object locale, string address, uint threadCount)
        {
            _address = address;
            _rangeStart = rangeStart;
            _rangeEnd = rangeEnd;
            _locale = locale;
            _threadCount = threadCount;
            _pages = new Queue<Block>();
        }

        public void Start()
        {
            if (_threadCount > 1)
            {
                uint petThread = (_rangeEnd - _rangeStart) / _threadCount;
                for (uint i = 0; i < _threadCount; ++i)
                {
                    uint start = _rangeStart + (petThread * i);
                    uint end = _rangeStart + (petThread * (i + 1));
                    Core core = new Core(start, end - 1, this);
                    Thread thread = new Thread(core.Start);
                    thread.Start();
                }
            }
            else
            {
                Core core = new Core(_rangeStart, _rangeEnd, this);
                Thread thread = new Thread(core.Start);
                thread.Start();
            }
        }

        public void Progress()
        {
            OnProgressChanged(this);
        }
    }
}
