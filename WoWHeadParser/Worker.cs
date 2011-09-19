using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public delegate void DownloaderProgressHandler(Worker worker, int val);

    public class Worker
    {
        protected uint _rangeStart;
        protected uint _rangeEnd;
        protected object _locale;
        protected uint _threadCount;
        protected Queue<string> _pages;
        protected int _progress;
        protected string _address;

        public event DownloaderProgressHandler OnProgressChanged;

        public Queue<string> Pages
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
            _pages = new Queue<string>();
            _progress = (int)rangeStart;
        }

        public void Start()
        {
            uint petThread = (_rangeEnd - (_rangeStart - 1)) / _threadCount;
            //start = 1000, end = 5000, threadCount = 25
            //perThread = (5000 - 1000) / 25 = 160
            //1000 -> 1160 ( 1000 - 1159)
            //1160 -> 1320  (1160 - 1319)
            //1320 -> 1480 (1320 - 1479
            for (uint i = 0; i < _threadCount; ++i)
            {
                uint start = _rangeStart + (petThread * i);
                uint end = _rangeStart + (petThread * (i + 1));
                Core core = new Core(start, end - 1, this);
                Thread thread = new Thread(core.Start);
                thread.Start();
            }
        }

        public void Progress()
        {
            ++_progress;
            OnProgressChanged(this, _progress);
        }
    }
}
