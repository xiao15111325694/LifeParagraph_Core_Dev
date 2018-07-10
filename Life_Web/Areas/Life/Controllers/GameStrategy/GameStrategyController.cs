using System.Threading.Tasks;
using life_Comme;
using Life_Paragraph_Core;
using Life_Untiy;
using Life_Web.Life_Server;
using Life_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Life_Web.Areas.Life.Controllers.GameStrategy
{

    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/GameStrategy")]
    [Authorize]
    public class GameStrategyController : Controller
    {
        private readonly IGameStrategyServer _gameStrategyServer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameStrategyServer"></param>
        public GameStrategyController(IGameStrategyServer  gameStrategyServer)
        {
            _gameStrategyServer = gameStrategyServer;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameNodeID"></param>
        /// <param name="selectEntity"></param>
        /// <returns></returns>
       
        [Route("ByGameNodeID")]
        public async Task<PageViewModel<GameStrategyViewModel>> GetAllByGameNodeID([FromHeader]SelectGameStrategyEntity selectEntity)
        {
            var result = await _gameStrategyServer.GetGameStrategyByNodeID(selectEntity.GameNodeID);
            PageListServer<GameStrategyViewModel> pageListServer = new PageListServer<GameStrategyViewModel>();
            var pageViewModel= pageListServer.GetPageList(result, selectEntity.PageIndex, selectEntity.PageSize);
            return pageViewModel;
        }
    }
}