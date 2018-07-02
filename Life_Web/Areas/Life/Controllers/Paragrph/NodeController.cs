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
    [Produces("application/json")]

    public class NodeController : Controller
    {
        private readonly INodeServer _nodeServer;
        private readonly IParagrphNodeServer _paragrphNodeServer;
        public NodeController(INodeServer nodeServer,
            IParagrphNodeServer paragrphNodeServer)
        {
            _nodeServer = nodeServer;
            _paragrphNodeServer = paragrphNodeServer;
        }

        [Route("api/Node/GetAllNodeName")]
        public async Task<List<NodeViewModel>> GetAllNodeName()
        {
            var entity = (from node in await _nodeServer.GetAllNode()
                         select new NodeViewModel
                         {
                             ID = node.ID,
                             CreateTime = node.CreateTime,
                             NodeName = node.NodeName,
                             NodeDescribe = node.NodeDescribe,
                             ParagraphCount = _paragrphNodeServer.FinAll().Where(x => x.NodeId == node.ID).Select(x => x.ParagraphId).Count()
                         }).ToList();
            return entity;
        }
    }
}