using life_Comme;
using Life_Core_Repositories;
using Life_Paragraph_Core;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public interface IGameStrategyServer : IRepository<GameStrategy>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameNodeId"></param>
        /// <returns></returns>
        Task<List<GameStrategyViewModel>> GetGameStrategyByNodeID(int gameNodeId);
        void PostHtmlGameStrategy();
    }
}
