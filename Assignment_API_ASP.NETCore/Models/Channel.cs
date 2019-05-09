using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Assignment_API_ASP.NETCore.Models
{
    public class Channel
    {

        [Key]
        [Column("ChannelID", TypeName = "int")]
        public int ChannelID { get; set; }
        [Column("Name", TypeName = "varchar(50)")]
        public string Name { get; set; }
        [NotMapped]
        public IList<User> ChannelUsers { get; set; }

    }
}
