using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Life_Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class GameStrategyController : Controller
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
     
        public IActionResult Index(int id)
        {
            ViewBag.GameNodeID = id;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
     
        public IActionResult GameIndex(int id)
        {
            ViewBag.GameNodeID = id;
            return View();
        }
    }
}