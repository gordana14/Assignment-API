using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Assignment_API_ASP.NETCore.Models
{
    public class StreamContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conn = GetConnectionString();
                //"Server=DESKTOP-B6SE9GK\\SQLEXPRESS;Database=StreamDB;Trusted_Connection=True;";
                //GetConnectionString();
                optionsBuilder.UseSqlServer(conn);
            }
         
        }


        public static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("StreamDatabase");
        }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set;  }
        public DbSet<UserChannel> UserChannels { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<Channel>().ToTable("ChannelInfo", "dbo");
            modelBuilder.Entity<Message>().ToTable("MessageInfo", "dbo");
            modelBuilder.Entity<User>().ToTable("UserInfo", "dbo");
            modelBuilder.Entity<UserChannel>().HasKey(x => new { x.UserID, x.ChannelID });
            


        }
    }
}
