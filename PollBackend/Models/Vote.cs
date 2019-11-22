using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class Vote
    {
        public long VoteId { get; set; }
        public long AnswerId { get; set; }
        public long UserId { get; set; }
    }
}
