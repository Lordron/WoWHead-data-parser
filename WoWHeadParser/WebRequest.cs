using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace WoWHeadParser
{
    public class Requests : IDisposable
    {
        public Uri Uri { get; private set; }
        public HttpWebRequest Request { get; private set; }
        public HttpWebResponse Response { get; set; }
        public uint Id { get; private set; }

        #region User Agents

        private Random _random = new Random();

        private List<string> _userAgents = new List<string>
        {
            @"Science Traveller International 1X/1.0",
            @"Mozilla/3.0 (compatible)",
            @"amaya/9.52 libwww/5.4.0",
            @"amaya/9.51 libwww/5.4.0",
            @"amaya/9.1 libwww/5.4.0",
            @"AmigaVoyager/3.4.4 (MorphOS/PPC native)",
            @"xChaos_Arachne/5.1.89;GPL,386+",
            @"Chimera/2.0alpha",
            @"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24",
            @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4",
            @"Mozilla/4.0 (compatible; Dillo 2.2)",
            @"Opera/9.00 (Windows NT 5.1; U; en)",
            @"Opera/8.5 (Macintosh; PPC Mac OS X; U; en)",
            @"Opera/7.60 (Windows NT 5.2; U) [en] (IBM EVV/3.0/EAK01AG9/LE)",
            @"Links (2.1pre31; Linux 2.6.21-omap1 armv6l; x)"
        };
        #endregion

        public Requests(string address, uint id)
        {
            Id = id;
            Uri = new Uri(string.Format(address, id));

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = GetRandomUserAgent();
            Request.Headers.Add("Accept-Encoding", "gzip,deflate");
            Request.KeepAlive = true;
        }

        public Requests(string address, uint ids, uint ide)
        {
            Id = ids;
            Uri = new Uri(string.Format(address, ids, ide));

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = GetRandomUserAgent();
            Request.Headers.Add("Accept-Encoding", "gzip,deflate");
            Request.KeepAlive = true;
        }

        public bool EndGetResponse(IAsyncResult asyncResult)
        {
            try
            {
                Response = (HttpWebResponse)Request.EndGetResponse(asyncResult);
                return true;
            }
            catch
            {
                Console.WriteLine("Cannot get response from {0}", Uri);
                return false;
            }
        }

        private string GetRandomUserAgent()
        {
            return _userAgents[_random.Next(0, 14)];
        }

        public void Dispose()
        {
            if (Request != null)
                Request.Abort();
            if (Response != null)
                Response.Close();
        }

        public override string ToString()
        {
            if (Response == null)
                return string.Empty;

            Stream stream = Response.GetResponseStream();
            switch (Response.ContentEncoding.ToUpperInvariant())
            {
                case "GZIP":
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                    break;
                case "DEFLATE":
                    stream = new DeflateStream(stream, CompressionMode.Decompress);
                    break;
            }

            using (BufferedStream buffer = new BufferedStream(stream))
            {
                using (StreamReader reader = new StreamReader(buffer))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}