﻿using Life_Core_Repositories;
using Life_Web.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public class UserCollectionServer : Repository<UserCollection>, IUserCollectionServer
    {
        public UserCollectionServer(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
