using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Assignment_API.Models.Context
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<StreamContext>
    {
        protected override void Seed(StreamContext context)
        {
            base.Seed(context);
            Channel channel = new Channel { Id = 1,  Name = "Wheather" };
            context.Channels.Add(channel);
            context.SaveChanges();
        }
    }

 }
