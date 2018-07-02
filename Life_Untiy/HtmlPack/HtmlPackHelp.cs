using HtmlAgilityPack;
using System.Collections.Generic;

namespace Life_Untiy
{
    public class HtmlPackHelp
    {
        public HtmlPackHelp()
        {

        }

        public List<life_Comme.ParagraphAddModel> PostPackHtmlData(string url)
        {
            EnCodHelp enCodHelp = new EnCodHelp();
            var EncodeUrl = enCodHelp.HttpGet(url);
            List<life_Comme.ParagraphAddModel> paragraphAddModel = new List<life_Comme.ParagraphAddModel>();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(EncodeUrl);
            HtmlNodeCollection htmlNodes;
            if (url == "http://ent.163.com/")
            {
                htmlNodes = document.DocumentNode.SelectNodes("//div[@class='top_news']/ul/li/h3/a[@href]");
            }
            else if(url== "http://www.9game.cn/wzry/gonglue-0-1/")
            {
                htmlNodes = document.DocumentNode.SelectNodes("//div[@class='box-text']/ul[@class='icon-word-list statistics-show']/li[@class='no-pic statistics-click']/div[@class='right-text']/p[@class='tit']/a[@href]");
            }
            else
            {
                htmlNodes = document.DocumentNode.SelectNodes("//div[@class='news-box news-box-thr news-box-pic clear']/h4/a[@href]");
            }

            foreach (var item in htmlNodes)
            {
                life_Comme.ParagraphAddModel paragraphAdd = new life_Comme.ParagraphAddModel
                {
                    Title = item.InnerHtml.Trim()
                };
                var hrefString = item.Attributes["href"].Value.ToLower().Trim(); ;
                var lasthref = "";
                if (hrefString.Contains("http:"))
                {
                    lasthref = hrefString.Replace("http:", "");
                }
                if (lasthref != "")
                {
                    paragraphAdd.VidUrl = lasthref;
                }
                else
                {
                    paragraphAdd.VidUrl = hrefString;
                }
                paragraphAddModel.Add(paragraphAdd);
            }
            return paragraphAddModel;
        }

        public Dictionary<int, string> PostPackHtmlContnt(string url, int parId)
        {   
            var url1 = url.Contains("wzry");
            string toUrl = "";
            HtmlNodeCollection htmlNodes;
            if (url1)
            {
                toUrl = url.Replace("/wzry", "http://www.9game.cn/wzry");
            }
            else
            {
                toUrl = url.Replace("//", "http://");
            }
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = htmlWeb.Load(toUrl);
            Dictionary<int,string> htmldic = new Dictionary<int, string>();
            var html = "";
            if (url1)
            {
                htmlNodes = document.DocumentNode.SelectNodes("//div[@class='text-con']/p");
            }
            else
            {
                htmlNodes = document.DocumentNode.SelectNodes("//article[@class='article']/p");
            }
            if (htmlNodes == null)
            {
                html += "<p>" + "小编正努力加班编写中^_^" + "<p/>";
            }
            else
            {
                foreach (var item in htmlNodes)
                {
                    var htmlValue = item.InnerHtml;
                    if (htmlValue.ToLower().Contains("img"))
                    {
                        html += htmlValue.Replace("img", "img width='650'");
                    }
                    else
                    {
                        html += htmlValue;
                    }
                 
                }
            }

            htmldic.Add(parId, html);
            return htmldic;
        }
    }
}
