

using Life_Core_Repositories;
using Life_Untiy;
using Life_Web.Data;
using Life_Web.Models;
using Models;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public class UserServer : Repository<User>, IUserServer
    {
        private readonly ApplicationDbContext _dbContext;

        public UserServer( ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegisterResult> CreateUserAsync(User user, string password)
        {
            DESHelp des = new DESHelp();
            user.PasswordHash = des.Encrypt(password);
            var result = await SaveAsync(user);
            if (result)
            {
                return RegisterResult.Success;
            }
            return RegisterResult.Failed;
        }

        public User Get(int id)
        {
            return GetById(id);
        }

    }
}
