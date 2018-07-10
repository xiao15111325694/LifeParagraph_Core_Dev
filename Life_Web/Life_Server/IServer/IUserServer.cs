using Life_Core_Repositories;
using Life_Web.Models;
using Models;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public interface IUserServer: IRepository<User> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User Get(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<RegisterResult> CreateUserAsync(User user, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<SignInResult> CheckEmailAndPasswordAsync(string email, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> ReturnCheckEmailAndPasswordAsync(string email, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User> GetUserByEmailAsync(string email);
    }
}
