using Life_Core_Repositories;
using Life_Web.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public class ParagraphNodeServer : Repository<ParagraphNode>, IParagrphNodeServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ParagraphNodeServer(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<ParagraphNode> FinAll()
        {
            return GetAll().AsQueryable();
        }
    }
}
