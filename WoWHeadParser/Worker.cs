using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace WoWHeadParser
{
    public class Worker
    {
        protected int _rangeStart;
        protected int _rangeEnd;
        protected int _threadCount;
        protected string _address;
        protected Queue<Block> _pages;
        protected BackgroundWorker _background;

        public BackgroundWorker Background
        {
            get { return _background; }
        }
        public Queue<Block> Pages
        {
            get { return _pages; }
        }

        public string Address
        {
            get { return _address; }
        }

        public Worker(int rangeStart, int rangeEnd, int threadCount, string address, BackgroundWorker background)
        {
            _background = background;
            _background.DoWork += new DoWorkEventHandler(DoWorkDownload);
            _address = address;
            _rangeStart = rangeStart;
            _rangeEnd = rangeEnd;
            _threadCount = threadCount;
            _pages = new Queue<Block>();
        }

        public void Start()
        {
            if (_threadCount > 1)
            {
                int petThread = (_rangeEnd - _rangeStart) / _threadCount;
                for (uint i = 0; i < _threadCount; ++i)
                {
                    int start = (int)(_rangeStart + (petThread * i));
                    int end = (int)(_rangeStart + (petThread * (i + 1)));
                    Core core = new Core(start, end, this);
                    Thread thread = new Thread(core.Start);
                    thread.Start();
                }
            }
            else
            {
                Core core = new Core(_rangeStart, _rangeEnd, this);
                _background.RunWorkerAsync(core);
            }
        }

        void DoWorkDownload(object sender, DoWorkEventArgs e)
        {
            Core core = (Core)e.Argument;
            core.Start();
        }
    }
}
