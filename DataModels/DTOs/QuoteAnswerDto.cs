using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.DTOs
{
    public class QuoteAnswerDto
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public string Answer { get; set; }

        public bool IsCorrect { get; set; }
    }
}
