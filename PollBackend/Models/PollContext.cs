using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBackend.Models
{
    public class PollContext: DbContext
    {
        public PollContext(DbContextOptions<PollContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Poll> Polls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<User>().ToTable("User");
            modelbuilder.Entity<Vote>().ToTable("Vote");
            modelbuilder.Entity<Participation>().ToTable("Participation");
            modelbuilder.Entity<Friendship>().ToTable("Friendship");
            modelbuilder.Entity<Answer>().ToTable("Answer");
            modelbuilder.Entity<Notification>().ToTable("Notification");
            modelbuilder.Entity<Poll>().ToTable("Poll");
        }
    }
}
