using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    public class Core
    {
        protected Worker _worker;
        protected WebClient _client;
        protected object _threadLock;
        protected int _rangeStart;
        protected int _rangeEnd;
        protected int _entry;

        public Core(int rangeStart, int rangeEnd, Worker worker)
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
            string baseAddress = string.Format("http://{0}{1}", (string.IsNullOrEmpty(_worker.Locale) ? "www." : _worker.Locale), _worker.Address);
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
                    Block block = new Block(e.Result, (uint)_entry);
                    _worker.Pages.Enqueue(block);
                }
            }
            _worker.Background.ReportProgress(1);
        }
    }
}
