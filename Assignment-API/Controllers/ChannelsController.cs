using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment_API.Models;
using Assignment_API.Models.Context;

namespace Assignment_API.Controllers
{
    public class ChannelsController : ApiController
    {
        //Creating Instance of DatabaseContext class  
        private StreamContext db = new StreamContext();
        //Creating a method to return Json data   
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try {
                var results = db.Channels;
                if (results != null) return Ok(results);
                else return InternalServerError();
            }
            catch (Exception)
            { 
                //If any exception occurs Internal Server Error i.e. Status Code 500 will be returned  
                return InternalServerError();
            }

        }

    }
}
