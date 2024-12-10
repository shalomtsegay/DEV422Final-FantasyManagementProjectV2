using Microsoft.EntityFrameworkCore;
using TeamManagementServiceV2.Model;

namespace TeamManagementServiceV2.Data
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions<TeamContext> options) : base(options)
        {
            //empty
        }

        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: seed a sample team for testing
            modelBuilder.Entity<Team>().HasData(
               new Team
               {
                   TeamId = 1,
                   TeamName = "Jaguars",
                   PlayerIds = new List<int>()  
               },
                new Team
                {
                    TeamId = 2,
                    TeamName = "Tigers",
                    PlayerIds = new List<int>()  
                },
                new Team
                {
                    TeamId = 3,
                    TeamName = "Panthers",
                    PlayerIds = new List<int>() 
                }
            );
        }

    }
}
