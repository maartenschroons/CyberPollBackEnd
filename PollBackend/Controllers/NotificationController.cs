using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollBackend.Models;

namespace PollBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly PollContext _context;

        public NotificationController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Notification

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        //Get a list of notifications by receiver
        [HttpGet("receiver/{id}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByReceiver(long id)
        {
            var notificationList = await _context.Notifications.ToListAsync();
            var notifications = new List<Notification>();

            //check notification if the receiver equals the given id and add it to the list
            foreach (var notification in notificationList)
            {
                if (notification.ReceiverId == id && notification.Answered == false)
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        //Get a list of friendnotifications by receiverId and type
        [HttpGet("friend/{id}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetFriendNotificationsByReceiver(long id)
        {
            var notificationList = await _context.Notifications.ToListAsync();
            var notifications = new List<Notification>();
            //check notification if the receiver equals the given id and the type is "friend" and add it to the list
            foreach (var notification in notificationList)
            {
                if (notification.ReceiverId == id && notification.Answered == false && notification.Type == "friend")
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        //Get a list of pollnotifications by receiverId and type
        [HttpGet("poll/{id}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetPollNotificationsByReceiver(long id)
        {
            var notificationList = await _context.Notifications.ToListAsync();
            var notifications = new List<Notification>();
            //check notification if the receiver equals the given id and the type is "poll" and add it to the list
            foreach (var notification in notificationList)
            {
                if (notification.ReceiverId == id && notification.Answered == false && notification.Type == "poll")
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        //Get a list of notifications by sender
        [HttpGet("sender/{id}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsBySender(long id)
        {
            var notificationList = await _context.Notifications.ToListAsync();
            var notifications = new List<Notification>();

            //check notification if the sender equals the given id and add it to the list
            foreach (var notification in notificationList)
            {
                if (notification.SenderId == id && notification.Answered == false)
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }


        //Get a list of denied Notifications
        [HttpGet("denied")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetDeniedNotifications()
        {
            var notificationList = await _context.Notifications.ToListAsync();
            var notifications = new List<Notification>();

            //Check each friendnotificatin if it is answered and it is not accepted
            foreach (var notification in notificationList)
            {
                if (notification.Accepted == false && notification.Answered == true && notification.Type == "friend")
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        // GET: api/Notification/5

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return notification;
        }

        // PUT: api/Notification/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(long id, Notification notification)
        {
            if (id != notification.NotificationId)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notification
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notification)
        {
            //Set the name of the sender
            var user = await _context.Users.FindAsync(notification.SenderId);
            notification.SenderName = user.Email;

            //Set the name of the receiver
            var userR = await _context.Users.FindAsync(notification.ReceiverId);
            notification.ReceiverName = userR.Email;

            //Set the name of the poll if it is a pollinvite
            if (notification.Type.Equals("poll"))
            {
                var poll = await _context.Polls.FindAsync(notification.PollId);
                notification.PollName = poll.Title;
            }

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotification", new { id = notification.NotificationId }, notification);
        }

        // DELETE: api/Notification/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Notification>> DeleteNotification(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        private bool NotificationExists(long id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }
    }
}
