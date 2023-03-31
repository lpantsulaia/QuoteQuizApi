using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using QuoteQuizApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoteQuizApi.Repositories
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(DataContext context) : base(context)
        {

        }
        public new async Task<Quote> GetAsync(int id)
        {
            return await QuoteContext.Quotes.Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public new async Task<IEnumerable<Quote>> GetAllAsync()
        {
            return await QuoteContext.Quotes.Include(x => x.Answers).ToListAsync();
        }
        private DataContext QuoteContext => Context as DataContext;


    }
}
