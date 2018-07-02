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
    /// <summary>
    /// 
    /// </summary>
    public interface IGameNodeServer : IRepository<GameNode>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<GameNodeViewModel> GetAllGameNode();
        /// <summary>
        /// 
        /// </summary>
        void PostGameNode();


    }
}
