using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Life_Untiy
{
    public class PubilcHtml
    {
        /// <summary>
        /// 解析网站
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HtmlDocument AnalysisHtml(string url)
        {
            EnCodHelp enCodHelp = new EnCodHelp();
            var EncodeUrl = enCodHelp.HttpGet(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(EncodeUrl);
            return document;
        }

        /// <summary>
        /// 标签提取
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Indexes"></param>
        /// <returns></returns>
        public HtmlNodeCollection ReturnHtmlNode(string url, string Indexes)
        {
            var doc = AnalysisHtml(url);
            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes(Indexes);
            if (htmlNodes != null)
            {
                return htmlNodes;
            }
            return null;
        }
    }
}
