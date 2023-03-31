using QuoteQuizApi.Interfaces;
using System.Threading.Tasks;

namespace QuoteQuizApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _dataContext;
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
            UserRepository = new UserRepository(_dataContext);
            QuoteRepository = new QuoteRepository(_dataContext);
        }
        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();

        }
        public IUserRepository UserRepository { get; }
        public IQuoteRepository QuoteRepository { get; }


    }
}
