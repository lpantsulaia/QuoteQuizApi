using System.Threading.Tasks;

namespace QuoteQuizApi.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        IUserRepository UserRepository { get; }
        IQuoteRepository QuoteRepository { get; }

    }
}
