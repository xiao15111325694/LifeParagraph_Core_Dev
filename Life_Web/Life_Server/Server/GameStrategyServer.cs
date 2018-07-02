using AutoMapper;
using HtmlAgilityPack;
using life_Comme;
using Life_Core_Repositories;
using Life_Untiy;
using Life_Web.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    /// <summary>
    /// 
    /// </summary>
    public class GameStrategyServer: Repository<GameStrategy>,IGameStrategyServer
    {
    
        List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>()
            {
                 new KeyValuePair<string, string>(key: "kw", value: "%E5%8D%95%E6%9C%BA"),
                 new KeyValuePair<string, string>(key: "sort", value: "hits"),
                 new KeyValuePair<string, string>(key: "page", value: "2"),
            };
        private readonly string indexone = "//div[@class='search-main-list']/div[@class='taptap-app-card']/a";
        private readonly string indexTwo = "//div[@class='taptap-app-list']/div[@class='taptap-app-item']/a";

        private readonly IGameHtmlInfoServer _gameHtmlInfoServer;
        private readonly IMapper _mapper;
        private readonly IGameNodeServer _gameNodeServer;
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="gameHtmlInfoServer"></param>
        /// <param name="gameNodeServer"></param>
        /// <param name="mapper"></param>
        public GameStrategyServer(ApplicationDbContext dbContext,
            IGameHtmlInfoServer gameHtmlInfoServer,
            IGameNodeServer gameNodeServer,
            IMapper mapper) : base(dbContext)
        {
            _gameHtmlInfoServer = gameHtmlInfoServer;
            _gameNodeServer = gameNodeServer;
            _mapper = mapper;
        }

        /// <summary>
        /// 解析URL返回对应标签数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, HtmlNodeCollection> GetGameHtmlInfo()
        {
            PubilcHtml pubilcHtml = new PubilcHtml();
            var gameHtmlInfos = _gameHtmlInfoServer.GetAll();
            Dictionary<int, HtmlNodeCollection> htmlNodes = new Dictionary<int, HtmlNodeCollection>();
            HtmlNodeCollection result = new HtmlNodeCollection(null);
            if (gameHtmlInfos != null)
            {

                for (int i = 1; i <= 23; i++)
                {

                    var html = gameHtmlInfos[0].HtmlUrl.Replace("e378", string.Format("e378?page={0}", i));
                    var nodeLists = pubilcHtml.ReturnHtmlNode(html, indexTwo);
                    foreach (var node in nodeLists)
                    {
                        result.Append(node);
                    }
                }

                var gameList = gameHtmlInfos.Skip(1);
                foreach (var item in gameList)
                {
                    var gameHtmlType = item.HtmlUrl.Substring(27);
                    for (int i = 1; i < 400; i++)
                    {
                        GameStrategy gameStrategy = new GameStrategy();
                        var JsonApi = String.Format("https://www.taptap.com/ajax/search/tags?&kw={0}&sort=hits&page={1}", gameHtmlType, i);
                        var temp = HttpGet(JsonApi, keyValues);
                        JsonHelp jsonHelp = new JsonHelp();
                        var tempValue = jsonHelp.JsonToObj<GameJson>(temp);
                        var htmlContent = "";
                        if (tempValue.Data.Html.Contains("img")) 
                        {
                            var htmlContentone = tempValue.Data.Html.Replace("<img", "<img style='width:150px;height:150px;margin-top:10px'");
                            var htmlContentTwo = htmlContentone.Replace("app-card-right app-tag-right", "pull-right col-md-7");
                            htmlContent = htmlContentTwo.Replace("btn btn-xs btn-default", "btn btn-xs btn-info");
                        }
                        gameStrategy.content = htmlContent;
                        gameStrategy.GameNodeID = item.GameNodeId.GetValueOrDefault();
                        gameStrategy.CreateTime = DateTime.Now;
                        Save(gameStrategy);
                    }
                }
                htmlNodes.Add(gameHtmlInfos[0].GameNodeId.GetValueOrDefault(), result);
            }
            return htmlNodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameNodeId"></param>
        /// <returns></returns>
        public async Task<List<GameStrategyViewModel>> GetGameStrategyByNodeID(int gameNodeId)
        {
            var ee = GetAll();
            var ss = _gameNodeServer.GetAll();
            var viewModels = (from en in await GetAllAsync()
                              join no in _gameNodeServer.GetAll()
                              on en.GameNodeID equals no.Id
                              where en.GameNodeID == gameNodeId
                              select new GameStrategyViewModel()
                              {
                                  Content = en.content,
                                  CommentCount = en.CommentCount,
                                  CreateTime = en.CreateTime,
                                  FromUrl = en.FromUrl,
                                  GameName = en.GameName,
                                  GameNodeID = en.GameNodeID,
                                  GameTitle = en.GameTitle,
                                  ReadCount = en.ReadCount,
                                  GameNodeName = no.GameTypeName
                              }).OrderByDescending(x => x.CreateTime).ToList();
            //var entitys = await LoadListAllAsync(x => x.GameNodeID == gameNodeId);
            //var viewModels = _mapper.Map<List<GameStrategyViewModel>>(entitys);
            return viewModels;
        }


        /// <summary>
        /// 游戏模块数据扒拉 
        /// </summary>
        public void PostHtmlGameStrategy()
        {
            var results = GetGameHtmlInfo();
            foreach (var items in results)
            {
                var games = items.Value;
                foreach (var item in games)
                {
                    GameStrategy gameStrategy = new GameStrategy();
                    var htmlinfo = item.InnerHtml.Trim();
                    if (item.ChildNodes.FindFirst("img").Attributes.Contains("data-src"))
                    {
                        var imgHref = item.ChildNodes.FindFirst("img").Attributes["data-src"].Value.ToLower().Trim();
                        var tite = item.ChildNodes.FindFirst("img").Attributes["title"].Value.ToLower().Trim();
                        var Fromhref = item.Attributes["href"].Value;
                        gameStrategy.content = imgHref;
                        gameStrategy.GameName = tite;
                        gameStrategy.GameNodeID = items.Key;
                        gameStrategy.FromUrl = Fromhref;
                        Save(gameStrategy);
                    }

                }
            }

        }

        /// 同步get请求
        /// </summary>
        /// <param name="url">链接地址</param>       
        /// <param name="formData">写在header中的键值对</param>
        /// <returns></returns>
        public string HttpGet(string url, List<KeyValuePair<string, string>> formData = null)
        {
            HttpClient httpClient = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(formData);
            if (formData != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                for (int i = 0; i < formData.Count; i++)
                {
                    content.Headers.Add(formData[i].Key, formData[i].Value);
                }
            }
            HttpMethod get = HttpMethod.Get;
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = get,
            };
            for (int i = 0; i < formData.Count; i++)
            {
                request.Headers.Add(formData[i].Key, formData[i].Value);
            }
            var res = httpClient.SendAsync(request);
            res.Wait();
            var resp = res.Result;
            Task<string> temp = resp.Content.ReadAsStringAsync();
            temp.Wait();
            return temp.Result;
        }
    }
}
