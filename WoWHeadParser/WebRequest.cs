using System;
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

        public Requests(string address, uint id)
        {
            Uri = new Uri(string.Format("{0}{1}", address, id));
            Id = id;

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            Request.Method = "POST";
            Request.Headers.Add("Accept-Encoding", "gzip,deflate");
            Request.KeepAlive = true;
        }

        public Requests(string address, uint ids, uint ide)
        {
            Uri = new Uri(string.Format("{0}crv={1}:{2}", address, ids, ide));
            Id = ids;

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            Request.Method = "POST";
            Request.Headers.Add("Accept-Encoding", "gzip,deflate");
            Request.KeepAlive = true;
        }

        public void Dispose()
        {
            Request.Abort();
            Response.Close();
        }

        public override string ToString()
        {
            if (Response == null)
                return string.Empty;

            Stream stream = default(Stream);
            switch (Response.ContentEncoding.ToUpperInvariant())
            {
                case "GZIP":
                    stream = new GZipStream(Response.GetResponseStream(), CompressionMode.Decompress);
                    break;
                case "DEFLATE":
                    stream = new DeflateStream(Response.GetResponseStream(), CompressionMode.Decompress);
                    break;
                default:
                    stream = Response.GetResponseStream();
                    break;
            }

            return new StreamReader(stream).ReadToEnd();
        }
    }
}