using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class Notification
    {
        public long NotificationId { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public long ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Type { get; set; }
        public long PollId { get; set; }   
        public string PollName { get; set; }
        public bool Accepted { get; set; }
        public bool Answered { get; set; }
    }
}
