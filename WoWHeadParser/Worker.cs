using System;
using System.Collections.Concurrent;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WoWHeadParser.Parser;

namespace WoWHeadParser
{
    public class Worker : IDisposable
    {

        [StructLayout(LayoutKind.Explicit)]
        public struct ParserValue
        {
            [FieldOffset(0)]
            public uint Start;

            [FieldOffset(4)]
            public uint End;

            [FieldOffset(0)]
            public uint Id;

            [FieldOffset(0)]
            public uint Maximum;

            [FieldOffset(8)]
            public uint[] Array;
        }

        private ParserValue m_value;

        private bool m_isWorking;
        private Uri m_address;
        private ParsingType m_type;
        private PageParser m_parser;
        private DateTime m_timeStart;
        private DateTime m_timeEnd;
        private ServicePoint m_service;
        private SemaphoreSlim m_semaphore;
        private ConcurrentQueue<uint> m_badIds;

        private const int SemaphoreCount = 100;
        private const int KeepAliveTime = 100000;

        private object m_locker = new object();

        public Worker(ParsingType type, PageParser parser, EventHandler onPageDownloadingComplete = null)
        {
            m_type = type;
            m_parser = parser;

            m_address = LocaleMgr.GetAddress(parser.Locale);
            ServicePointManager.DefaultConnectionLimit = SemaphoreCount * 10;
            {
                m_service = ServicePointManager.FindServicePoint(m_address);
                m_service.SetTcpKeepAlive(true, KeepAliveTime, KeepAliveTime);
            }
            m_address = new Uri(m_address, parser.Address);

            m_semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
            m_badIds = new ConcurrentQueue<uint>();

            PageDownloadingComplete += onPageDownloadingComplete;
        }

        public void SetValue(ParserValue value)
        {
            m_value = value;
        }

        public unsafe void Start()
        {
            if (m_isWorking)
                throw new InvalidOperationException("_isWorking");

            m_isWorking = true;
            m_timeStart = DateTime.Now;

            switch (m_type)
            {
                case ParsingType.TypeBySingleValue:
                    {
                        Process(m_value.Id);
                        break;
                    }
                case ParsingType.TypeByMultipleValue:
                    {
   
                        for (uint entry = m_value.Start; entry <= m_value.End; ++entry)
                        {
                            if (!Process(entry))
                                break;
                        }
                        break;
                    }
                case ParsingType.TypeByList:
                    {
                        for (int i = 0; i < m_value.Array.Length; ++i)
                        {
                            if (!Process(m_value.Array[i]))
                                break;
                        }
                        break;
                    }
                case ParsingType.TypeByWoWHeadFilter:
                    {
                        for (uint entry = 0; entry <= m_value.Maximum; ++entry)
                        {
                            if (!Process(entry))
                                break;
                        }
                        break;
                    }
            }

            while (m_semaphore.CurrentCount != SemaphoreCount)
            {
                Application.DoEvents();
            }

            if (m_type == ParsingType.TypeByList || m_type == ParsingType.TypeByWoWHeadFilter)
            {
                while (!m_badIds.IsEmpty)
                {
                    if (!m_isWorking)
                        break;

                    uint id;
                    if (!m_badIds.TryDequeue(out id))
                        continue;

                    if (!Process(id))
                        break;
                }
            }

            while (m_semaphore.CurrentCount != SemaphoreCount)
            {
                Application.DoEvents();
            }

            m_timeEnd = DateTime.Now;
        }

        private bool Process(uint id)
        {
            if (!m_isWorking)
                return false;

            m_semaphore.Wait();

            Requests request = new Requests(m_address, id, m_type == ParsingType.TypeByWoWHeadFilter);
            request.BeginGetResponse(RespCallback, request);
            return true;
        }

        private void RespCallback(IAsyncResult iar)
        {
            Requests request = (Requests)iar.AsyncState;

            string page;
            bool endGetResponse = request.EndGetResponse(iar, out page);
            lock (m_locker)
            {
                if (endGetResponse)
                    m_parser.TryParse(page, request.Id);
                else
                    m_badIds.Enqueue(request.Id);
            }
            request.Dispose();
            m_semaphore.Release();

            if (endGetResponse && PageDownloadingComplete != null)
                PageDownloadingComplete(null, EventArgs.Empty);
        }

        public void Stop()
        {
            m_isWorking = false;
            m_service.ConnectionLeaseTimeout = 0;
        }

        public void Reset()
        {
            m_parser = null;
            m_isWorking = false;
            m_service.ConnectionLeaseTimeout = 0;

            GC.Collect();
        }

        public void Dispose()
        {
            m_parser = null;
            m_semaphore.Dispose();
            m_service.ConnectionLeaseTimeout = 0;
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder(m_parser.Builder.Count * 256);

            content.AppendFormat(@"-- Dump of {0} ({1}), Total object count: {2}", m_timeEnd, m_timeEnd - m_timeStart, m_parser.Builder.Count).AppendLine();
            content.AppendLine();
            content.Append(m_parser.ToString());

            return content.ToString();
        }

        public event EventHandler PageDownloadingComplete;
    }
}