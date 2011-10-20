using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    public class Worker
    {
        protected int _rangeStart;
        protected int _rangeEnd;
        protected int _entry;
        protected object _threadLock;
        protected string _address;
        protected Queue<Block> _pages;
        protected BackgroundWorker _background;
        protected List<Thread> _threads;
        protected Semaphore _semaphore;
        protected WebClient _client;

        protected const int PercentProgress = 1;

        public Queue<Block> Pages
        {
            get { return _pages; }
        }

        public List<Thread> Threads
        {
            get { return _threads; }
        }

        public Worker(int rangeStart, int rangeEnd, int threadCount, string address, BackgroundWorker background)
        {
            _rangeStart = rangeStart;
            _rangeEnd = rangeEnd;

            _background = background;
            _background.DoWork += new DoWorkEventHandler(DoWorkDownload);
            _threadLock = new object();
            _address = address;
            _pages = new Queue<Block>();
            _threads = new List<Thread>();
            _semaphore = new Semaphore(threadCount, threadCount);
            _client = new WebClient { Encoding = Encoding.UTF8 };
            _client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringDataCompleted);
        }

        public void Start()
        {
            _background.RunWorkerAsync();
        }

        void DoWorkDownload(object sender, DoWorkEventArgs e)
        {
            for (_entry = _rangeStart; _entry < _rangeEnd; ++_entry)
            {
                _semaphore.WaitOne();
                Thread thread = new Thread(DownloadStart);
                _threads.Add(thread);
                thread.Start();
            }
        }

        public void DownloadStart()
        {
            try
            {
                _client.DownloadStringAsync(new Uri(string.Format("{0}{1}", _address, _entry)));
            }
            catch { }
        }

        void DownloadStringDataCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                lock (_threadLock)
                {
                    Block block = new Block(e.Result, (uint)_entry);
                    _pages.Enqueue(block);
                }
            }
            if (_background.IsBusy)
                _background.ReportProgress(PercentProgress);

            _semaphore.Release();
        }
    }
}
