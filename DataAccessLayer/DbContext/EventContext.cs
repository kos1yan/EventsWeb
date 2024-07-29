using Events.Domain.Entities;
using Events.Infrastructure.DbContext.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.DbContext
{
    public class EventContext : IdentityDbContext<User>
    {
        public EventContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventConfiguration).Assembly);
        }

        public DbSet<Event>? Events{ get; set; }
        public DbSet<Member>? Members{ get; set; }
        public DbSet<Image>? Images{ get; set; }
        public DbSet<Category>? Categories{ get; set; }
        
    }
}
