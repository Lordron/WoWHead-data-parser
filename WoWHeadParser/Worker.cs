using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {
        private int _rangeStart;
        private int _rangeEnd;
        private int _entry;
        private object _threadLock;
        private string _address;
        private Queue<Block> _pages;
        private BackgroundWorker _background;
        private List<Thread> _threads;
        private Semaphore _semaphore;
        private WebClient _client;

        private const int PercentProgress = 1;

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
            _client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringAsyncCompleted);
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
                Thread thread = new Thread(Download);
                _threads.Add(thread);
                thread.Start();
            }
        }

        public void Download()
        {
            try
            {
                _client.DownloadStringAsync(new Uri(string.Format("{0}{1}", _address, _entry)));
            }
            catch { }
        }

        void DownloadStringAsyncCompleted(object sender, DownloadStringCompletedEventArgs e)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_semaphore != null)
                    _semaphore.Dispose();
                if (_client != null)
                    _client.Dispose();
            }
        }
    }
}
