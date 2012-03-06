using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {
        private uint _start;
        private uint _end;
        private bool _working;
        private string _address;
        private Queue<Block> _pages;
        private IList<uint> _entries;
        private SemaphoreSlim _semaphore;

        private int _delay = 2000;

        private object _threadLock = new object();

        public Queue<Block> Pages
        {
            get { return _pages; }
        }

        public bool Empty { get { return _pages.Count <= 0; } }

        public Worker()
        {
            _pages = new Queue<Block>();
            _entries = new List<uint>();
            _semaphore = new SemaphoreSlim(10, 10);
        }

        public void SetValue(uint value, string address)
        {
            _start = value;
            _address = address;

            _delay = 2000;
        }

        public void SetValue(uint start, uint end, string address)
        {
            _end = end;
            _start = start;
            _address = address;

            _delay = 10000;
        }

        public void SetValue(IList<uint> entries, string address)
        {
            _entries = entries;
            _address = address;

            _delay = 10000;
        }

        public void Start(ParsingType type)
        {
            _working = true;

            switch (type)
            {
                case ParsingType.TypeSingle:
                    {
                        _semaphore.Wait();
                        Requests request = new Requests(_address, _start);
                        request.Request.BeginGetResponse(RespCallback, request);
                        break;
                    } 
                case ParsingType.TypeMultiple:
                    {
                        for (uint entry = _start; entry <= _end; ++entry)
                        {
                            if (!_working)
                                return;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, entry);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        for (int i = 0; i < _entries.Count; ++i)
                        {
                            if (!_working)
                                return;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, _entries[i]);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
            }

            Thread.Sleep(_delay);
        }

        private void RespCallback(IAsyncResult iar)
        {
            if (!_working)
                return;

            Requests request = (Requests)iar.AsyncState;
            try
            {
                request.Response = (HttpWebResponse)request.Request.EndGetResponse(iar);
            }
            catch { }

            string text = request.ToString();
            if (!string.IsNullOrWhiteSpace(text))
            {
                lock (_threadLock)
                {
                    if (_pages != null)
                        _pages.Enqueue(new Block(text, request.Id));
                }
            }

            request.Dispose();

            lock (_threadLock)
            {
                if (_semaphore != null)
                    _semaphore.Release();
            }

            if (PageDownloaded != null)
                PageDownloaded();
        }

        public void Stop()
        {
            if (_working)
                _working = false;
        }

        public void Finish()
        {
            Stop();
            _pages.Clear();

            if (Finished != null)
                Finished();
        }

        ~Worker()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            Stop();

            _pages.Clear();
            _semaphore.Dispose();

            _pages = null;
            _semaphore = null;
        }

        public delegate void OnPageDownloaded();

        public delegate void OnFinished();

        /// <summary>
        /// Occurs when a page is downloaded.
        /// </summary>
        public event OnPageDownloaded PageDownloaded;

        /// <summary>
        /// Occurs when the working is finished
        /// </summary>
        public event OnFinished Finished; 
    }
}
