using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public class Requests : IDisposable
    {
        public uint Id { get; private set; }
        public static bool Compress { get; set; }

        private Uri _uri;
        private HttpWebRequest _request;
        private HttpWebResponse _response;

        #region User Agents

        private Random _random = new Random();

        private string[] _userAgents = new string[]
        {
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.45 Safari/535.19",
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.20 (KHTML, like Gecko) Chrome/19.0.1036.7 Safari/535.20",
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/534.55.3 (KHTML, like Gecko) Version/5.1.3 Safari/534.53.10",
            @"Mozilla/5.0 (iPad; CPU OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko ) Version/5.1 Mobile/9B176 Safari/7534.48.3",
            @"Mozilla/5.0 (Windows; U; Windows NT 6.1; tr-TR) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/5.0.4 Safari/533.20.27",
            @"Mozilla/5.0 (Windows; U; Windows NT 6.1; sv-SE) AppleWebKit/533.19.4 (KHTML, like Gecko) Version/5.0.3 Safari/533.19.4",
            @"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-HK) AppleWebKit/533.18.1 (KHTML, like Gecko) Version/5.0.2 Safari/533.18.5",
            @"Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:2.2) Gecko/20110201",
            @"Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.9.2a1pre) Gecko",
            @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6",
            @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_0) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
            @"Opera/9.00 (Windows NT 5.1; U; en)",
            @"Opera/8.5 (Macintosh; PPC Mac OS X; U; en)",
            @"Opera/7.60 (Windows NT 5.2; U) [en] (IBM EVV/3.0/EAK01AG9/LE)",
            @"Opera/9.80 (Windows NT 6.1; WOW64; U; ru) Presto/2.10.229 Version/11.64",
            @"Opera/9.80 (Windows NT 6.1; U; es-ES) Presto/2.9.181 Version/12.00",
            @"Opera/9.80 (X11; Linux i686; U; ru) Presto/2.8.131 Version/11.11",
            @"Mozilla/5.0 (Windows NT 6.0; U; ja; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 11.00",
            @"Opera/9.70 (Linux ppc64 ; U; en) Presto/2.2.1",
        };

        #endregion

        public Requests(Uri address, uint id, bool isFilter = false)
        {
            Id = id;
            if (isFilter)
                _uri = new Uri(string.Format(address.OriginalString, (id * 200), ((id + 1) * 200)));
            else
                _uri = new Uri(string.Format(address.OriginalString, id));

            _request = (HttpWebRequest)WebRequest.Create(_uri);
            _request.UserAgent = GetRandomUserAgent();
            _request.KeepAlive = true;
            if (Compress)
                _request.Headers.Add("Accept-Encoding", "gzip,deflate");
        }

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return _request.BeginGetResponse(callback, state);
        }

        public bool EndGetResponse(IAsyncResult asyncResult, out string page)
        {
            try
            {
                _response = (HttpWebResponse)_request.EndGetResponse(asyncResult);
                page = ToString();
                return true;
            }
            catch
            {
                page = string.Empty;
                Console.WriteLine(Resources.Error_Cannot_get_response, _uri);
                return false;
            }
        }

        private string GetRandomUserAgent()
        {
            return _userAgents[_random.Next(0, _userAgents.Length - 1)];
        }

        public void Dispose()
        {
            if (_request != null)
                _request.Abort();
            if (_response != null)
                _response.Close();

            _userAgents = null;
        }

        public override string ToString()
        {
            if (_response == null)
                return string.Empty;

            Stream stream = _response.GetResponseStream();
            switch (_response.ContentEncoding)
            {
                case "gzip":
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                    break;
                case "deflate":
                    stream = new DeflateStream(stream, CompressionMode.Decompress);
                    break;
            }

            using (BufferedStream buffer = new BufferedStream(stream))
            using (StreamReader reader = new StreamReader(buffer))
            {
                return reader.ReadToEnd();
            }
        }
    }
}