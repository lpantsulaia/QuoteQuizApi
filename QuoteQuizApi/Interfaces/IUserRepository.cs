using DataModels.Models;
using System.Threading.Tasks;

namespace QuoteQuizApi.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Authenticate(string username, string password);
        //User GetById(int id);
        Task<User> Create(User user, string password);
        Task Update(User user, string password = null);
        //void Delete(int id);
    }
}
