using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class Friendship
    {
        public long FriendshipId { get; set; }
        public long UserIdA { get; set; }
        public long UserIdB { get; set; }
    }
}
