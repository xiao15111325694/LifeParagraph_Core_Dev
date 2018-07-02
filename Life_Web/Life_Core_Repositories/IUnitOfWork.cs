using System;
using System.Collections.Generic;
using System.Text;

namespace Life_Core_Repositories
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}
