using Life_Core_Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public interface IParagrphNodeServer : IRepository<ParagraphNode>
    {
        IQueryable<ParagraphNode> FinAll();
    }
}
