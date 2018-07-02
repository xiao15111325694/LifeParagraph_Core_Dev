using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Life_Web.Life_Server;
using Microsoft.AspNetCore.Mvc;

namespace Life_Web.Controllers
{
    public class ParagraphController : Controller
    {
        private readonly INodeServer _nodeServer;
        public ParagraphController(INodeServer nodeServer)
        {
            _nodeServer = nodeServer;
        }
        public IActionResult ShowParagraphIndex(int id)
        {
            ViewBag.NodeId = id;
            ViewBag.NodeName = _nodeServer.GetNodeNameById(id);
            return View();
        }

        public IActionResult ParagrapDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}