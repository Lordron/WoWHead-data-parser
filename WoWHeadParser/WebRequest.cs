using System;
using System.IO;
using System.Net;

namespace WoWHeadParser
{
    public class Requests : IDisposable
    {
        private HttpWebRequest _request;
        private HttpWebResponse _response;
        private int _entry;

        public HttpWebRequest Request { get { return _request; } }
        public HttpWebResponse Response { get { return _response; } set { _response = value; } }
        public int Entry { get { return _entry; } }

        public Requests(Uri url, int entry)
        {
            _request = (HttpWebRequest)WebRequest.Create(url);
            _request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            _request.Method = "POST";
            _entry = entry;
        }

        public string GetContent()
        {
            if (_response == null)
                throw new ArgumentNullException("Response");

            return new StreamReader(_response.GetResponseStream()).ReadToEnd();
        }

        ~Requests()
        {
            Dispose();
        }

        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_request != null)
                    _request.Abort();

                if (_response != null)
                {
                    _response.Close();
                    _response.Dispose();
                }
            }
        }
    }
}
