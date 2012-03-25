using System;
using System.IO;
using System.Net;

namespace WoWHeadParser
{
    public class Requests : IDisposable
    {
        public Uri Uri { get; private set; }
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public uint Id { get; set; }

        public Requests(string address, uint id)
        {
            Uri = new Uri(string.Format("{0}{1}", address, id));
            Id = id;

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            Request.Method = "POST";
        }

        public Requests(string address, uint ids, uint ide)
        {
            Uri = new Uri(string.Format("{0}crv={1}:{2}", address, ids, ide));
            Id = ids;

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            Request.Method = "POST";
        }

        public override string ToString()
        {
            if (Response != null)
                return new StreamReader(Response.GetResponseStream()).ReadToEnd();

            return string.Empty;
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
                if (Request != null)
                    Request.Abort();

                if (Response != null)
                    Response.Close();
            }
        }
    }
}