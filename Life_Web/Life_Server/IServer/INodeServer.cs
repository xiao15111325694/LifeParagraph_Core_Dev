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
    public interface INodeServer : IRepository<Node>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<NodeViewModel>> GetAllNode();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetNodeNameById(int id);
    }
}
