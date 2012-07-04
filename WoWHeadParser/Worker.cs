using System;
using System.Collections.Concurrent;
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
        private ConcurrentQueue<uint> _badIds;

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
            _semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
            _badIds = new ConcurrentQueue<uint>();
        }

        public Worker(EventHandler OnPageDownloadingComplete)
            : this()
        {
            PageDownloadingComplete += OnPageDownloadingComplete;
        }

        public void Parser(DataParser parser)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");

            _parser = parser;

            _address = new Uri(string.Format("http://{0}wowhead.com/", _locales[parser.Locale]));
            ServicePointManager.DefaultConnectionLimit = SemaphoreCount * 10;
            _service = ServicePointManager.FindServicePoint(_address);
            {
                _service.SetTcpKeepAlive(true, 100000, 100000);
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

        public void Start(ParsingType type, bool compress)
        {
            if (_isWorking)
                throw new InvalidOperationException("_isWorking");

            _isWorking = true;
            _timeStart = DateTime.Now;

            switch (type)
            {
                case ParsingType.TypeBySingleValue:
                    {
                        _semaphore.Wait();

                        Requests request = new Requests(_address, _start, compress);
                        request.Request.BeginGetResponse(RespCallback, request);
                        break;
                    }
                case ParsingType.TypeByMultipleValue:
                    {
                        for (uint entry = _start; entry <= _end; ++entry)
                        {
                            if (!_isWorking)
                                break;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, entry, compress);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
                case ParsingType.TypeByList:
                    {
                        for (int i = 0; i < _entries.Count; ++i)
                        {
                            if (!_isWorking)
                                break;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, _entries[i], compress);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
                case ParsingType.TypeByWoWHeadFilter:
                    {
                        for (uint entry = 0; entry <= _start; ++entry)
                        {
                            if (!_isWorking)
                                break;

                            _semaphore.Wait();

                            Requests request = new Requests(_address, (entry * 200), ((entry + 1) * 200), compress);
                            request.Request.BeginGetResponse(RespCallback, request);
                        }
                        break;
                    }
            }

            if (type == ParsingType.TypeByList)
            {
                while (!_badIds.IsEmpty)
                {
                    if (!_isWorking)
                        break;

                    uint id;
                    if (!_badIds.TryDequeue(out id))
                        continue;

                    _semaphore.Wait();

                    Requests request = new Requests(_address, id, compress);
                    request.Request.BeginGetResponse(RespCallback, request);
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

            bool ok = request.EndGetResponse(iar);
            if (ok)
            {
                string page = request.ToString();
                if (!string.IsNullOrEmpty(page))
                    _parser.TryParse(page, request.Id);
                else
                {
                    ok = false;
                    _badIds.Enqueue(request.Id);
                }
            }
            else
                _badIds.Enqueue(request.Id);

            request.Dispose();
            _semaphore.Release();

            if (ok && PageDownloadingComplete != null)
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
            StringBuilder content = new StringBuilder(_parser.Items.Count * 1024);

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