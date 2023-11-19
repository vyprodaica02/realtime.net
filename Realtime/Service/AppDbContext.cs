using Microsoft.EntityFrameworkCore;
using Realtime.Entity;

namespace Realtime.Service
{
    public class AppdbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Integrated Security=true;Initial Catalog=RealTimeChatApp;MultipleActiveResultSets=True;encrypt=true;trustservercertificate=true");
        }
      
        public DbSet<User> users { get; set; }
        public DbSet<Message> messages { get; set; }
    }
}
