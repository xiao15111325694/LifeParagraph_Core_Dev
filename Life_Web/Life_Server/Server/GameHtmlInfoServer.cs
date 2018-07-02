using Life_Core_Repositories;
using Life_Web.Data;
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
    public class GameHtmlInfoServer : Repository<GameHtmlInfo>, IGameHtmlInfoServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public GameHtmlInfoServer(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
