using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {
        private uint _start;
        private uint _end;
        private bool _working;
        private string _address;
        private IList<uint> _entries;
        private SemaphoreSlim _semaphore;

        private object _threadLock = new object();

        public List<Block> Pages { get; private set; }

        public bool Empty { get { return Pages.Count <= 0; } }

        private const int SemaphoreCount = 10;

        public Worker()
        {
            Pages = new List<Block>();
            _entries = new List<uint>();
            _semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
        }

        public void SetValue(uint value, string address)
        {
            _start = value;
            _address = address;
        }

        public void SetValue(uint start, uint end, string address)
        {
            _end = end;
            _start = start;
            _address = address;
        }

        public void SetValue(IList<uint> entries, string address)
        {
            _entries = entries;
            _address = address;
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
                                break;

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
                                break;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, _entries[i]);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
                case ParsingType.TypeWoWHead:
                    {
                        for (uint entry = 0; entry <= _start; ++entry)
                        {
                            if (!_working)
                                break;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, (entry * 200), ((entry + 1) * 200));
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
            }

            while (_semaphore.CurrentCount != SemaphoreCount)
            {
                continue;
            }

            Pages.Sort(new BlockComparer(Settings.Default.SortDown));
        }

        private void RespCallback(IAsyncResult iar)
        {
            Requests request = (Requests)iar.AsyncState;
            try
            {
                request.Response = (HttpWebResponse)request.Request.EndGetResponse(iar);
            }
            catch
            {
                Console.WriteLine("Cannot get response from {0}", request.Uri);
            }

            string text = request.ToString();
            if (!string.IsNullOrWhiteSpace(text))
            {
                lock(_threadLock)
                {
                    if (Pages != null)
                        Pages.Add(new Block(text, request.Id));
                }
            }

            request.Dispose();

            lock(_threadLock)
            {
                if (_semaphore != null)
                    _semaphore.Release();
            }

            if (PageDownloadingComplete != null)
                PageDownloadingComplete();
        }

        public void Stop()
        {
            if (_working)
                _working = false;
        }

        public void Finish()
        {
            Stop();
            Pages.Clear();

            if (RunWorkerCompleted != null)
                RunWorkerCompleted();
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

            Pages.Clear();
            _semaphore.Dispose();

            Pages = null;
            _semaphore = null;
        }

        public delegate void OnPageDownloadingComplete();

        public delegate void OnRunWorkerCompleted();

        /// <summary>
        /// Occurs when a page is downloaded.
        /// </summary>
        public event OnPageDownloadingComplete PageDownloadingComplete;

        /// <summary>
        /// Occurs when the working is completed or has been canceled
        /// </summary>
        public event OnRunWorkerCompleted RunWorkerCompleted;
    }
}