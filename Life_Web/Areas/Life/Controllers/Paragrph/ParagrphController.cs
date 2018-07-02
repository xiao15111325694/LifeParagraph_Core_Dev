using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using life_Comme;
using Life_Paragraph_Core;
using Life_Untiy;
using Life_Web.Life_Server;
using Life_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Life_Web.Areas.Life.Controllers
{
    [Produces("application/json")]
    public class ParagrphController : Controller
    {
        private readonly IParagraphServer _paragraphServer;
        public ParagrphController(IParagraphServer paragraphServer)
        {
            _paragraphServer = paragraphServer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectEntity"></param>
        /// <returns></returns>
        [Route("api/Paragrph/GetAll")]
        public async Task<PageViewModel<ParagraphViewModel>> GetAll([FromHeader]SelectParagrphEntity selectEntity)
        {
            var result = await _paragraphServer.GetAllParagraphAsync(selectEntity);
            PageListServer<ParagraphViewModel> pageListServer = new PageListServer<ParagraphViewModel>();
            var resultViewModel= pageListServer.
                GetPageList(result, selectEntity.PageIndex, selectEntity.PageSize);
            return resultViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        [Route("api/Paragrph/NodeId")]
        public async Task<List<ParagraphViewModel>> GetAll(int nodeId)
        {
            var result = await _paragraphServer.GetByNodeId(nodeId);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Paragrph/Id")]
        public async Task<ParagraphViewModel> GetById(int id)
        {
            var result = await _paragraphServer.GetByIDAsync(id);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [Route("api/Paragrph/MostThumbsupCount")]
        public async Task<List<ParagraphViewModel>> GetParagraphMostThumbsupCount(DateTime time)
        {
            var result = await _paragraphServer.GetParagraphMostThumbsupCount();
            return result;
        }

    }
}