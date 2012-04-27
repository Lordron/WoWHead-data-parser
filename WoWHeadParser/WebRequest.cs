﻿using System;
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

        public Requests(Uri address, uint id)
        {
            Uri = new Uri(string.Format("{0}{1}", address, id));
            Id = id;

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            Request.Method = "POST";
            Request.Headers.Add("Accept-Encoding", "gzip,deflate");
            Request.KeepAlive = true;
        }

        public Requests(Uri address, uint ids, uint ide)
        {
            Uri = new Uri(string.Format("{0}crv={1}:{2}", address, ids, ide));
            Id = ids;

            Request = (HttpWebRequest)WebRequest.Create(Uri);
            Request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.503.0 Safari/534.6";
            Request.Method = "POST";
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