using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_API_ASP.NETCore.Models
{
    public class Message
    {
        [Key]
        [Column("MessageID", TypeName = "int")]
        public int MessageID { get; set; }
        [Column("Text", TypeName = "varchar(50)")]
        public string Text { get; set; }
        [Column("Inserted", TypeName = "datetime")]
        public DateTime? Inserted { get; set; } = null;
        [Column("Validated", TypeName = "datetime")]
        public DateTime? Validated { get; set; } = null;
        //Adding Foreign Key Constraints for State  
        [Column("ChannelID" , TypeName ="int")]
        public int ReferenceChannelID { get; set; }
        [Column("UserID" , TypeName ="int")]
        public int ReferenceUserID { get; set;  }

    }
}
