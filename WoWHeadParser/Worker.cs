using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {
        private uint _start;
        private uint _end;
        private bool _isWorking;
        private string _address;
        private Parser _parser;
        private DateTime _timeStart;
        private DateTime _timeEnd;
        private List<uint> _entries;
        private ServicePoint _service;
        private List<PageItem> _pages;
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
            _pages = new List<PageItem>();
            _semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
        }

        public void Parser(Parser parser)
        {
            _parser = parser;
            _address = string.Format("http://{0}wowhead.com/{1}", _locales[parser.Locale], parser.Address);
            _service = ServicePointManager.FindServicePoint(new Uri(_address));
            {
                _service.ConnectionLeaseTimeout = Timeout.Infinite;
                _service.ConnectionLimit = SemaphoreCount;
            }
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

            SortOrder sortOrder = (SortOrder)Settings.Default.SortOrder;
            if (sortOrder > SortOrder.None)
                _pages.Sort(new PageItemComparer(sortOrder));
        }

        private void RespCallback(IAsyncResult iar)
        {
            Requests request = (Requests)iar.AsyncState;
            if (request.EndGetResponse(iar))
            {
                PageItem item = new PageItem(request);
                lock (_threadLock)
                {
                    item.Page = _parser.SafeParser(item);
                    if (!string.IsNullOrEmpty(item.Page))
                        _pages.Add(item);
                }
            }

            request.Dispose();
            _semaphore.Release();

            if (PageDownloadingComplete != null)
                PageDownloadingComplete();
        }

        public void Stop()
        {
            if (!_isWorking)
                throw new InvalidOperationException();

            _isWorking = false;
            _service.ConnectionLeaseTimeout = 0;
        }

        public void Reset()
        {
            _isWorking = false;
            _pages.Clear();
            _service.ConnectionLeaseTimeout = 0;
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder(_pages.Count * 4096);

            content.AppendFormat(@"-- Dump of {0} ({1}), Total object count: {2}", _timeEnd, _timeEnd - _timeStart, _pages.Count).AppendLine();

            string preParse = _parser.PreParse().TrimStart();
            if (!string.IsNullOrEmpty(preParse))
                content.Append(preParse);

            for (int i = 0; i < _pages.Count; ++i)
            {
                content.Append(_pages[i].ToString());
            }

            return content.ToString();
        }

        public delegate void OnPageDownloadingComplete();

        /// <summary>
        /// Occurs when a page is downloaded.
        /// </summary>
        public event OnPageDownloadingComplete PageDownloadingComplete;
    }
}