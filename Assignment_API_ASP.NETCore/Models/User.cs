using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_API_ASP.NETCore.Models
{
    public class User
    {
 
        [Key]
        [Column("UserID", TypeName = "int")]
        public int UserID { get; set; }
        [Column("Name", TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Column("Activated", TypeName="bit")]
        public Boolean Activated  { get; set; }
        [NotMapped]
        public IList<Channel> UsersChannels { get; set; }

    }
}
