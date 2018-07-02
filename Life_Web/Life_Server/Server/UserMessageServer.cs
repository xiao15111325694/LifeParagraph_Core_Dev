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
    public class UserMessageServer : Repository<UserMessage>, IUserMessageServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public UserMessageServer(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMessage"></param>
        public void PostUserMessage(UserMessage userMessage)
        {
            Save(userMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMessage"></param>
        public async void PostUserMessagrAsync(UserMessage userMessage)
        {
            await SaveAsync(userMessage);
        }
    }
}
