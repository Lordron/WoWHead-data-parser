using System;
using System.IO;
using System.Net;

namespace WoWHeadParser
{
    public class Requests : IDisposable
    {
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public uint Id { get; set; }

        public Requests(string address, uint id)
        {
            Uri url = new Uri(string.Format("{0}{1}", address, id));
            Id = id;

            Request = (HttpWebRequest)WebRequest.Create(url);
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