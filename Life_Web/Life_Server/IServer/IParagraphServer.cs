using life_Comme;
using Life_Core_Repositories;
using Life_Paragraph_Core;
using Life_Web.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParagraphServer : IRepository<Paragraph>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<ParagraphViewModel>> GetAllParagraphAsync(SelectParagrphEntity select);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addmodel"></param>
        void PostParagraph(ParagraphAddModel addmodel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addModels"></param>
        void PostBachParagraph(List<ParagraphAddModel> addModels);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ParagraphViewModel> GetByIDAsync(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<List<ParagraphViewModel>> GetByNodeId(int nodeId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<ParagraphViewModel>> GetParagraphMostThumbsupCount();
        /// <summary>
        /// 
        /// </summary>
        void HtmlPackPostParagraph();
        /// <summary>
        /// 
        /// </summary>
        void HtmlPackByUrlToContnt();
    }
}
