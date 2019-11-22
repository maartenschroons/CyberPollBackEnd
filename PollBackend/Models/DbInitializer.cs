using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class DbInitializer
    {
        public static void Initialize(PollContext context)
        {
            context.Database.EnsureCreated();
            // Look for any verkiezingen.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            context.Users.AddRange(
                new User
                {
                    Username = "admin",
                    Password = "admin",
                    Email = "admin.admin@thomasmore.be"
                },
                new User
                {
                    Username = "test",
                    Password = "test",
                    Email = "test.test@thomasmore.be"
                });



            context.SaveChanges();
        }
    }
}
