using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Life_Untiy
{
    public class EnCodHelp
    {
        public string HttpGet(string url)
        {
            string responsestr = "";
            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.Accept = "*/*";
            req.Method = "GET";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
            using (HttpWebResponse response = req.GetResponse() as HttpWebResponse)
            {
                Stream stream;
                if (response.ContentType.ToLower().Contains("gzip"))
                {
                    stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (response.ContentType.ToLower().Contains("deflate"))
                {
                    stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream = response.GetResponseStream();
                }
                using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet)))
                {
                    responsestr = reader.ReadToEnd();
                    stream.Dispose();
                }
            }
            return responsestr;
        }
    }
}
