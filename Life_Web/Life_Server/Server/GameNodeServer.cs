using AutoMapper;
using life_Comme;
using Life_Core_Repositories;
using Life_Untiy;
using Life_Web.Data;
using Models;
using System;
using System.Collections.Generic;

namespace Life_Web.Life_Server
{
    /// <summary>
    /// 
    /// </summary>
    public class GameNodeServer : Repository<GameNode>, IGameNodeServer
    {

        private readonly string indexOne = "//ul[@class='list-unstyled category-tags']/li[@class='tag']/a[@href]";

        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public GameNodeServer(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public List<GameNode> PostHtmlGameNode(string url)
        {
            PubilcHtml pubilcHtml = new PubilcHtml();
            List<GameNode> gameNodes = new List<GameNode>();
            var results = pubilcHtml.ReturnHtmlNode(url, indexOne);
            foreach (var item in results)
            {
                GameNode gameNode = new GameNode();
                gameNode.GameTypeName = item.InnerHtml.Trim();
                gameNodes.Add(gameNode);
            }
            return gameNodes;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PostGameNode()
        {
            var eneitys = PostHtmlGameNode("https://www.taptap.com/categories");
            if (eneitys != null)
            {
                foreach (var item in eneitys)
                {
                    item.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                     Save(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GameNodeViewModel> GetAllGameNode()
        {
            var entitys = GetAll();
            var viewEntity = _mapper.Map<List<GameNodeViewModel>>(entitys);
            return viewEntity;
        }
    }
}
