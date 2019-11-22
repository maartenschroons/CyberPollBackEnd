using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class Participation
    {
        public long ParticipationId { get; set; }
        public long PollId { get; set; }
        public long UserId { get; set; }
    }
}
