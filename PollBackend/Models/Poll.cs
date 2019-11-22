using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class Poll
    {
        public long PollId { get; set; }
        public string Title { get; set; }
        public long Creator { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }

    }
}
