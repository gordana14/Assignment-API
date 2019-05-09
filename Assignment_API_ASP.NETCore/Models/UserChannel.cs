using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_API_ASP.NETCore.Models
{
    public class UserChannel
    {
        public int UserID { get; set; }
        public int ChannelID { get; set; }
        public virtual User User { get; set; }
        public virtual Channel Channel { get; set; }

    }
}
