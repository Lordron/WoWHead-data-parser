using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WoWHeadParser.Parser;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {
        private uint _start;
        private uint _end;
        private bool _isWorking;
        private Uri _address;
        private DataParser _parser;
        private DateTime _timeStart;
        private DateTime _timeEnd;
        private List<uint> _entries;
        private ServicePoint _service;
        private SemaphoreSlim _semaphore;

        private object _threadLock = new object();

        private const int SemaphoreCount = 100;

        #region Locales
        private Dictionary<Locale, string> _locales = new Dictionary<Locale, string>
        {
            {Locale.Old, "old."},
            {Locale.English, "www."},
            {Locale.Russia, "ru."},
            {Locale.Germany, "de."},
            {Locale.France, "fr."},
            {Locale.Spain, "es."},
            {Locale.Portugal, "pt."},
        };
        #endregion

        public Worker()
        {
            _entries = new List<uint>();
            _semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
        }

        public Worker(EventHandler OnPageDownloadingComplete)
            : this()
        {
            PageDownloadingComplete += OnPageDownloadingComplete;
        }

        public void Parser(DataParser parser)
        {
            _parser = parser;

            _address = new Uri(string.Format("http://{0}wowhead.com/", _locales[parser.Locale]));
            _service = ServicePointManager.FindServicePoint(_address);
            {
                _service.MaxIdleTime = 500;
                _service.ConnectionLeaseTimeout = 500;
                _service.ConnectionLimit = SemaphoreCount;
                _service.SetTcpKeepAlive(true, 120000000, 1000);
            }
            _address = new Uri(_address, parser.Address);
        }

        public void SetValue(uint value)
        {
            _start = value;
        }

        public void SetValue(uint start, uint end)
        {
            _end = end;
            _start = start;
        }

        public void SetValue(List<uint> entries)
        {
            _entries = entries;
        }

        public void Start(ParsingType type)
        {
            if (_isWorking)
                throw new InvalidOperationException();

            _isWorking = true;
            _timeStart = DateTime.Now;

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
                            if (!_isWorking)
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
                            if (!_isWorking)
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
                            if (!_isWorking)
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
                Application.DoEvents();
            }

            _timeEnd = DateTime.Now;
        }

        private void RespCallback(IAsyncResult iar)
        {
            Requests request = (Requests)iar.AsyncState;
            if (request.EndGetResponse(iar))
            {
                lock (_threadLock)
                    _parser.TryParse(request.ToString(), request.Id);
            }

            request.Dispose();
            _semaphore.Release();

            if (PageDownloadingComplete != null)
                PageDownloadingComplete(null, EventArgs.Empty);
        }

        public void Stop()
        {
            _isWorking = false;
            _service.ConnectionLeaseTimeout = 0;
        }

        public void Reset()
        {
            _isWorking = false;
            _parser.Items.Clear();
            _service.ConnectionLeaseTimeout = 0;

            GC.Collect();
        }

        public void Dispose()
        {
            _semaphore.Dispose();
            _service.ConnectionLeaseTimeout = 0;
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder(_parser.Items.Count * 4096);

            content.AppendFormat(@"-- Dump of {0} ({1}), Total object count: {2}", _timeEnd, _timeEnd - _timeStart, _parser.Items.Count).AppendLine();
            content.Append(_parser);

            return content.ToString();
        }

        /// <summary>
        /// Occurs when a page is downloaded.
        /// </summary>
        public event EventHandler PageDownloadingComplete;
    }
}