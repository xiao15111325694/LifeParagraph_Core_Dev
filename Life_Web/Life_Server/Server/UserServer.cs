

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SignInResult> CheckEmailAndPasswordAsync(string email, string password)
        {
            DESHelp dESHelp = new DESHelp();
            var passwordHash = dESHelp.Encrypt(password);
            var Checkresult = await LoadListAllAsync(x => x.Email == email && x.PasswordHash == passwordHash);
            if (Checkresult.Count > 0)
            {
                return SignInResult.Success;
            }
            else
            {
                return SignInResult.Failed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> ReturnCheckEmailAndPasswordAsync(string email, string password)
        {
            DESHelp dESHelp = new DESHelp();
            var passwordHash = dESHelp.Encrypt(password);
            var Checkresult = await GetAsync(x => x.Email == email && x.PasswordHash == passwordHash);
            return Checkresult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<RegisterResult> CreateUserAsync(User user, string password)
        {
            DESHelp dESHelp = new DESHelp();
            user.PasswordHash = dESHelp.Encrypt(password);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var entity = await GetAsync(x => x.Email == email);
            if (entity != null)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }
    }
}
