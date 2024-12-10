using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Model;

namespace PlayerManagementService.Data
{
    public class PlayerContext : DbContext
    {
        //constructor
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options) 
        {
        //empty
        
        }
      public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasData(


              new Player { PlayerId = 1, PlayerName = "John Doe", TeamId = "", IsDrafted = false, Details = "Forward player, excellent stamina." },
              new Player { PlayerId = 2, PlayerName = "Jane Smith", TeamId = "", IsDrafted = false, Details = "Defender, tactical expert." },
              new Player { PlayerId = 3, PlayerName = "Mike Johnson", TeamId = "", IsDrafted = false, Details = "Goalkeeper, strong reflexes." }
             
                );
        }

    }
}
