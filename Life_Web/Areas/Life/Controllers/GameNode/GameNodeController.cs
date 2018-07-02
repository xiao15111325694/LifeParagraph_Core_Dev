using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using life_Comme;
using Life_Paragraph_Core;
using Life_Web.Life_Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Life_Web.Areas.Life.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/GameNode")]
    public class GameNodeController : Controller
    {
        private readonly IGameNodeServer _gameNodeServer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameNodeServer"></param>
        public GameNodeController(IGameNodeServer gameNodeServer)
        {
            _gameNodeServer = gameNodeServer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("Get")]
        public List<GameNodeViewModel> GetAll()
        {
            var result = _gameNodeServer.GetAllGameNode();
            return result;
        }
    }
}