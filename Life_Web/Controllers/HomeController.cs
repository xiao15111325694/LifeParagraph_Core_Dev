using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Life_Web.Models;
using Life_Web.Life_Server;
using System.Threading.Tasks;
using System;
using Models;
using Life_Core_Repositories;

namespace Life_Web.Controllers
{
    public class HomeController : Controller
    { 
        private readonly IUserServer _userServer;
        private readonly INodeServer _nodeServer;
        private readonly IParagraphServer _paragraphServer;
        private readonly IRepository<GameStrategy> _repository;
        private readonly IGameNodeServer _gameNodeServer;
        private readonly IGameStrategyServer _gameStrategyServer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userServer"></param>
        /// <param name="nodeServer"></param>
        /// <param name="paragraphServer"></param>
        /// <param name="repository"></param>
        /// <param name="gameNodeServer"></param>
        /// <param name="gameStrategyServer"></param>
        public HomeController(
            IUserServer userServer,
            INodeServer nodeServer,
            IParagraphServer paragraphServer,
            IRepository<GameStrategy> repository,
            IGameNodeServer gameNodeServer,
            IGameStrategyServer gameStrategyServer
            )
        {
            _userServer = userServer;
            _nodeServer = nodeServer;
            _paragraphServer = paragraphServer;
            _repository = repository;
            _gameNodeServer = gameNodeServer;
            _gameStrategyServer = gameStrategyServer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //_gameStrategyServer.PostHtmlGameStrategy();
            ViewBag.Node = await _nodeServer.GetAllNode();
            ViewBag.Time = DateTime.Now.Date;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
