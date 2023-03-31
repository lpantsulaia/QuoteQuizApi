using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Contetnt { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public IEnumerable<QuoteAnswer> Answers { get; set; }
        public Quote()
        {
            Answers = new List<QuoteAnswer>();
        }


    }
}
