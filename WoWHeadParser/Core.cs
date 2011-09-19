using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public class Core
    {
        protected Worker _worker;
        protected object _threadLock;
        protected uint _rangeStart;
        protected uint _rangeEnd;
        protected WebClient _client;

        public Core(uint rangeStart, uint rangeEnd, Worker worker)
        {
            _threadLock = new object();
            _worker = worker;
            _rangeStart = rangeStart;
            _rangeEnd = rangeEnd;
            _client = new WebClient { Encoding = Encoding.UTF8 };
            _client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringDataCompleted);
        }

        public void Start()
        {
            string baseAddress = string.Format("http://{0}{1}", _worker.Locale, _worker.Address);
            for (uint i = _rangeStart; i <= _rangeEnd; ++i)
            {
                string address = string.Format("{0}{1}", baseAddress, i);
                try
                {
                    Task task = Download(address);
                    task.Wait();
                }
                catch
                {
                }
            }
        }

        public async Task Download(string address)
        {
            await _client.DownloadStringTaskAsync(address);
        }

        void DownloadStringDataCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                lock (_threadLock)
                {
                    _worker.Pages.Enqueue(e.Result);
                }
            }
            _worker.Progress();
        }
    }
}
