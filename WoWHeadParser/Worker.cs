using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {
        private uint _start;
        private uint _end;
        private uint _entry;
        private bool _working;
        private string _address;
        private Queue<Block> _pages;
        private IList<uint> _entries;
        private SemaphoreSlim _semaphore;

        private object _threadLock = new object();

        public Queue<Block> Pages
        {
            get { return _pages; }
        }

        public Worker(uint value, string address)
        {
            _entry = value;
            _address = address;
            _pages = new Queue<Block>(1);
            _semaphore = new SemaphoreSlim(1, 10);
        }

        public Worker(uint start, uint end, string address)
        {
            _end = end;
            _start = start;
            _address = address;

            int max = (int)(_end - start);
            _pages = new Queue<Block>(max);
            _semaphore = new SemaphoreSlim(1, 10);

        }

        public Worker(IList<uint> entries, string address)
        {
            _entries = entries;
            _address = address;
            _semaphore = new SemaphoreSlim(1, 10);
            _pages = new Queue<Block>(entries.Count);
        }

        public void Start(ParsingType type)
        {
            _working = true;

            switch (type)
            {
                case ParsingType.TypeSingle:
                    {
                        _semaphore.Wait();
                        Requests request = new Requests(_address, _entry);
                        request.Request.BeginGetResponse(RespCallback, request);
                        break;
                    }
                case ParsingType.TypeMultiple:
                    {
                        for (_entry = _start; _entry < _end; ++_entry)
                        {
                            if (!_working)
                                break;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, _entry);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        for (int i = 0; i < _entries.Count; ++i)
                        {
                            if (!_working)
                                break;

                            _semaphore.Wait();

                            _entry = _entries[i];
                            Requests request = new Requests(_address, _entry);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
            }
        }

        private void RespCallback(IAsyncResult iar)
        {
            Requests request = (Requests)iar.AsyncState;
            try
            {
                request.Response = (HttpWebResponse)request.Request.EndGetResponse(iar);
            }
            finally
            {
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
            }

            lock (_threadLock)
            {
                if (_semaphore != null)
                    _semaphore.Release();

                if (PageDownloaded != null)
                    PageDownloaded();
            }
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

            if (_working)
                _working = false;

            if (_semaphore != null)
            {
                _semaphore.Dispose();
                _semaphore = null;
            }

            if (_pages != null)
            {
                _pages.Clear();
                _pages = null;
            }

            if (_entries != null)
            {
                _entries.Clear();
                _entries = null;
            }

            if (Disposed != null)
                Disposed();
        }

        public delegate void OnPageDownloaded();

        public delegate void OnDisposed();

        /// <summary>
        /// Occurs when a page is downloaded.
        /// </summary>
        public event OnPageDownloaded PageDownloaded;

        /// <summary>
        /// Occurs when the component is disposed
        /// </summary>
        public event OnDisposed Disposed; 
    }
}
