using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace WoWHeadParser
{
    public class Worker
    {
        private uint _start;
        private uint _end;
        private uint _entry;
        private string _address;
        private ParsingType _type;
        private object _threadLock;
        private Queue<Block> _pages;
        private List<uint> _entries;
        private Semaphore _semaphore;
        private List<Requests> _requests;
        private BackgroundWorker _background;

        private const int PercentProgress = 1;

        public Queue<Block> Pages
        {
            get { return _pages; }
        }

        public Worker(uint start, uint end, string address, BackgroundWorker background)
        {
            _end = end;
            _start = start;
            _address = address;
            _background = background;
            _threadLock = new object();
            _pages = new Queue<Block>();
            _type = ParsingType.TypeMultiple;
            _requests = new List<Requests>();
            _semaphore = new Semaphore(1, 1);
            _background.DoWork += DownloadInitial;
        }

        public Worker(uint value, string address, BackgroundWorker background)
        {
            _entry = value;
            _address = address;
            _background = background;
            _threadLock = new object();
            _pages = new Queue<Block>();
            _type = ParsingType.TypeSingle;
            _requests = new List<Requests>();
            _background.DoWork += DownloadInitial;
        }

        public Worker(List<uint> entries, string address, BackgroundWorker background)
        {
            _entries = entries;
            _address = address;
            _background = background;
            _threadLock = new object();
            _pages = new Queue<Block>();
            _type = ParsingType.TypeList;
            _requests = new List<Requests>();
            _semaphore = new Semaphore(1, 1);
            _background.DoWork += DownloadInitial;
        }

        public void Start()
        {
            //Starting work on a different thread to prevent MainForm freezing
            _background.RunWorkerAsync();
        }

        public void DownloadInitial(object sender, DoWorkEventArgs e)
        {
            switch (_type)
            {
                case ParsingType.TypeSingle:
                    {
                        Requests request = new Requests(new Uri(string.Format("{0}{1}", _address, _entry)), _entry);
                        request.Request.BeginGetResponse(RespCallback, request);
                        break;
                    }
                case ParsingType.TypeMultiple:
                    {
                        for (_entry = _start; _entry < _end; ++_entry)
                        {
                            _semaphore.WaitOne();

                            Requests request = new Requests(new Uri(string.Format("{0}{1}", _address, _entry)), _entry);
                            {
                                _requests.Add(request);
                                request.Request.BeginGetResponse(RespCallback, request);
                            }
                        }
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        for (int i = 0; i < _entries.Count; ++i)
                        {
                            _semaphore.WaitOne();

                            _entry = _entries[i];
                            Requests request = new Requests(new Uri(string.Format("{0}{1}", _address, _entry)), _entry);
                            {
                                _requests.Add(request);
                                request.Request.BeginGetResponse(RespCallback, request);
                            }
                        }
                        break;
                    }
            }
            Thread.Sleep(1000);
        }

        private void RespCallback(IAsyncResult iar)
        {
            Requests request = (Requests)iar.AsyncState;
            try
            {
                request.Response = (HttpWebResponse)request.Request.EndGetResponse(iar);
                string text = request.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    Block block = new Block(text, request.Entry);
                    lock (_threadLock)
                        _pages.Enqueue(block);
                }
            }
            catch
            {
            }
            finally
            {
                request.Dispose();
            }

            _semaphore.Release();
            _background.ReportProgress(PercentProgress);
        }

        public void Stop()
        {
            if (_semaphore != null)
                _semaphore.Dispose();

            if (_background.IsBusy)
                _background.Dispose();

            foreach (Requests request in _requests)
            {
                request.Dispose();
            }
        }

        ~Worker()
        {
            Stop();
        }
    }
}
