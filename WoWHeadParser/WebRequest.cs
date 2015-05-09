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
        public uint Id;
        public static bool Compress;

        private Uri m_uri;
        private HttpWebRequest m_request;
        private HttpWebResponse m_response;

        #region User Agents

        private static string[] s_userAgents = new string[]
        {
            @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36",
            @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36",
            @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0",
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/600.4.10 (KHTML, like Gecko) Version/8.0.4 Safari/600.4.10",
            @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36",
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.104 Safari/537.36",
            @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36",
            @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.89 Safari/537.36",
            @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36",
        };

        private static int s_CurrentUserAgent = 0;

        #endregion

        public Requests(Uri address, string relative, uint id, bool isFilter = false)
        {
            Id = id;

            if (isFilter)
                m_uri = new Uri(address, string.Format(relative, (id * 200), ((id + 1) * 200)));
            else
                m_uri = new Uri(address, string.Format(relative, id));

            m_request = (HttpWebRequest)WebRequest.Create(m_uri);
            m_request.UserAgent = GetRandomUserAgent();
            m_request.KeepAlive = true;
            if (Compress)
                m_request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        }

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return m_request.BeginGetResponse(callback, state);
        }

        public bool EndGetResponse(IAsyncResult asyncResult, out string page)
        {
            try
            {
                m_response = (HttpWebResponse)m_request.EndGetResponse(asyncResult);
                page = ToString();
                return true;
            }
            catch
            {
                page = string.Empty;
                Console.WriteLine(Resources.Error_Cannot_get_response, m_uri);
                return false;
            }
        }

        private string GetRandomUserAgent()
        {
            if (s_CurrentUserAgent >= s_userAgents.Length)
                s_CurrentUserAgent = 0;

            return s_userAgents[s_CurrentUserAgent++];
        }

        public void Dispose()
        {
            if (m_request != null)
                m_request.Abort();
            if (m_response != null)
                m_response.Close();
        }

        public override string ToString()
        {
            if (m_response == null)
                return string.Empty;

            using (BufferedStream buffer = new BufferedStream(m_response.GetResponseStream()))
            using (StreamReader reader = new StreamReader(buffer))
            {
                return reader.ReadToEnd();
            }
        }
    }
}