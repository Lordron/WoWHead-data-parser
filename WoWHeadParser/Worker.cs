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
        private int _start;
        private int _end;
        private int _entry;
        private object _threadLock;
        private string _address;
        private Queue<Block> _pages;
        private BackgroundWorker _background;
        private Semaphore _semaphore;
        private WebClient _client;

        private const int PercentProgress = 1;

        public Queue<Block> Pages
        {
            get { return _pages; }
        }

        public Semaphore Semaphore
        {
            get { return _semaphore; }
        }

        public Worker(int start, int end, int threadCount, string address, BackgroundWorker background)
        {
            _end = end;
            _start = start;
            _address = address;
            _background = background;
            _threadLock = new object();
            _pages = new Queue<Block>();
            _semaphore = new Semaphore(threadCount, threadCount);
            _client = new WebClient { Encoding = Encoding.UTF8 };
            _background.DoWork += new DoWorkEventHandler(DownloadInitial);
            _client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadPageAsyncCompleted);
        }

        public void Start()
        {
            //Starting work on a different thread to prevent MainForm freezing
            _background.RunWorkerAsync();
        }

        void DownloadInitial(object sender, DoWorkEventArgs e)
        {
            for (_entry = _start; _entry < _end; ++_entry)
            {
                _semaphore.WaitOne();
                Thread thread = new Thread(DownloadPage);
                thread.Start();
            }
        }

        public void DownloadPage()
        {
            try
            {
                _client.DownloadStringAsync(new Uri(string.Format("{0}{1}", _address, _entry)));
            }
            catch { }
        }

        void DownloadPageAsyncCompleted(object sender, DownloadStringCompletedEventArgs e)
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
