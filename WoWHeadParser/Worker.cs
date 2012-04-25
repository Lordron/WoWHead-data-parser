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
        private bool _working;
        private Uri _address;
        private Parser _parser;
        private DateTime _timeStart;
        private DateTime _timeEnd;
        private ParsingType _type;
        private List<uint> _entries;
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
            _address = new Uri(string.Format("http://{0}{1}", _locales[parser.Locale], parser.Address));
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
            if (_working)
                throw new InvalidOperationException();

            _type = type;
            _working = true;
            _timeStart = DateTime.Now;

            ServicePoint servicePoint = ServicePointManager.FindServicePoint(_address);
            {
                servicePoint.ConnectionLimit = 1000;
                servicePoint.SetTcpKeepAlive(true, 100000, 100000);
            }

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

            _timeEnd = DateTime.Now;

            SortOrder order = (SortOrder)Settings.Default.SortOrder;
            if (order > SortOrder.None)
                _pages.Sort(new PageItemComparer(order));
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
                if (_type != ParsingType.TypeMultiple)
                    Console.WriteLine("Cannot get response from {0}", request.Uri);
            }

            string page = request.ToString();
            {
                if (!string.IsNullOrWhiteSpace(page))
                {
                    PageItem item = new PageItem(request.Id, page);
                    lock (_threadLock)
                    {
                        item.Page = _parser.Parse(item);
                        if (!string.IsNullOrEmpty(item.Page))
                            _pages.Add(item);
                    }
                }

                lock (_threadLock)
                    _semaphore.Release();
            }

            request.Dispose();

            if (PageDownloadingComplete != null)
                PageDownloadingComplete();
        }

        public void Stop()
        {
            if (!_working)
                throw new InvalidOperationException();

            _working = false;
        }

        public void Reset()
        {
            _working = false;
            _pages.Clear();
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder(_pages.Count * 4096);

            content.AppendFormat(@"-- Dump of {0} ({1}), Total object count: {2}", _timeEnd, _timeEnd - _timeStart, _pages.Count).AppendLine();

            string beforParsing = _parser.BeforParsing().TrimStart();
            if (!string.IsNullOrEmpty(beforParsing))
                content.Append(beforParsing);

            for (int i = 0; i < _pages.Count; ++i)
            {
                content.AppendLine(_pages[i].ToString());
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