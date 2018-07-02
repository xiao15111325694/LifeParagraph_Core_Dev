using Life_Core_Repositories;
using Life_Web.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public class HtmlPack : Repository<HtmlPackUrlInfo>, IHtmlPack
    {
        public HtmlPack(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
