using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class Answer
    {
        public long AnswerId { get; set; }
        public long PollId { get; set; }
        public long UserId { get; set; }
        public string Text { get; set; }
        public long voteAmount { get; set; }
    }
}
