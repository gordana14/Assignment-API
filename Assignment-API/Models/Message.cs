using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment_API.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        //Adding Foreign Key Constraints for State  
        public int IdChannel { get; set; }
        public Channel ReferenceChannel { get; set; }
    }
}