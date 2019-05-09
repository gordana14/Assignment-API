using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment_API.Models
{
    public class Channel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set;  }

    }
}