using Life_Core_Repositories;
using Life_Web.Models;
using Models;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public interface IUserServer: IRepository<User> 
    {
        User Get(int id);

        Task<RegisterResult> CreateUserAsync(User user, string password);
    }
}
