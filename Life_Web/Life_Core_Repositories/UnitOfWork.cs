using Life_Web.Data;
using Models;
using System;

namespace Life_Core_Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private ApplicationDbContext _Context;

        public UnitOfWork()
        {
        }

        public UnitOfWork(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        public bool Commit()
        {
           return _Context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            if (_Context != null)
            {
                _Context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
