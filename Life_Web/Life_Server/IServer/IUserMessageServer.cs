﻿using Life_Core_Repositories;
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
    public interface IUserMessageServer : IRepository<UserMessage>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMessage"></param>
        void PostUserMessage(UserMessage userMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMessage"></param>
        void PostUserMessagrAsync(UserMessage userMessage);
    }
}
