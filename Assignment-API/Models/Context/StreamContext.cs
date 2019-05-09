using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Assignment_API.Models
{
    public class StreamContext : DbContext
    {
        public StreamContext() : base("name=DefaultConnection") 
        {
              //Database.SetInitializer<StreamContext>(new  DropCreateDatabaseAlways <StreamContext>());

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<Message>()
                .HasRequired(a => a.ReferenceChannel)
                .WithMany()
                .HasForeignKey(a => a.IdChannel);

        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}