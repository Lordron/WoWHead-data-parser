using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    public class Core
    {
        protected Worker _worker;
        protected object _threadLock;
        protected uint _rangeStart;
        protected uint _rangeEnd;
        protected WebClient _client;
        protected uint _entry;

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
            for (_entry = _rangeStart; _entry <= _rangeEnd; ++_entry)
            {
                string address = string.Format("{0}{1}", baseAddress, _entry);
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
                    Block block = new Block(e.Result, _entry);
                    _worker.Pages.Enqueue(block);
                }
            }
            _worker.Progress();
        }
    }
}
