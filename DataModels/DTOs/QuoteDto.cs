using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.DTOs
{
    public class QuoteDto
    {
        public int Id { get; set; }
        public string Contetnt { get; set; }
        public int CreatorId { get; set; }


        public IEnumerable<QuoteAnswerDto> Answers { get; set; }
        public QuoteDto()
        {
            Answers = new List<QuoteAnswerDto>();
        }
    }
}
