using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_API_ASP.NETCore.Models.ModelsOutput
{
    public class FilterModelMessages
    {
       
            public int idUser { get; set; }
            public int idChannel { get; set;  }
            public string queryText { get; set; }
            public int Page { get; set; }
            public int Limit { get; set; }
        

    }
}
