using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_API_ASP.NETCore.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc.Routing;
using Assignment_API_ASP.NETCore.Models.ModelsOutput;

namespace Assignment_API_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly StreamContext _context;


        public MessagesController(StreamContext context)
        {
            _context = context;
         //   _urlHelper = uriHelper;
        }

        // GET: api/Messages
        [HttpGet("{idUser}/{idChannel}")]
        [SwaggerOperation(summary: "Gets the last message", description: "Return a last message from the requested user and channel sort by TimeCreation ")]
        [SwaggerResponse(400, "Not found - There is no messages.")]
        public IActionResult GetLastMessage([FromRoute] int idUser, [FromRoute] int idChannel)
        {
      
             var message = _context.Messages.Where(m => m.ReferenceChannelID.Equals(idChannel) & m.ReferenceUserID.Equals(idUser)).OrderByDescending(t => t.Inserted).ThenByDescending(t=>t.MessageID).FirstOrDefault();
                          
            if(message==null)
            {
                return NotFound("There is no messages.");
          
            }
            return Ok(message);
        }

        // GET: api/Messages/5
        [HttpGet("queries") ]
        [SwaggerOperation(summary: "Get the messages", description: "" +
            "Returns a list of all messages by some of criteria from queries ascending sort by TimeCreation and with message pagging")]
        [SwaggerResponse(400, "Not found - There is no messages.")]
        public ActionResult<PagedCollectionResponse<Message>> GetMessage([FromQuery] FilterModelMessages queryFilters)
        {
            /* if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState);
              }
              */
            if(queryFilters.Page==0 || queryFilters.Limit==0)
            {
                return BadRequest("Fields 'Page' and 'Limit' are required. ");
            }
            var message = from s in _context.Messages
                          select s;
            if (queryFilters.idUser > 0)
            {
                message = from s in message
                          where s.ReferenceUserID.Equals(queryFilters.idUser)
                          select s;
            }
            if(queryFilters.idChannel>0)
            {
                message = from s in message
                          where s.ReferenceChannelID.Equals(queryFilters.idChannel)
                          select s;
            }


            if (queryFilters.queryText != null && queryFilters.queryText.Trim().Length > 0)
            {
                message = from s in message
                              where s.Text.Contains(queryFilters.queryText)
                             select s;
            }

                
            message = from s in message
                      where (s.Validated.HasValue) & (s.Validated>= DateTime.Now)
                      orderby s.Inserted ascending
                      select s;
            var auxresult = message;

            if (message == null)
            {
                return NotFound();
            }
            

            message = message.Skip((queryFilters.Page - 1) * queryFilters.Limit).Take(queryFilters.Limit);

            //Get the data for the current page  
            var result = new PagedCollectionResponse<Message>();
            result.Items = message.ToList();

            //Get next page URL string  
            FilterModelMessages newfilter = new FilterModelMessages { queryText = queryFilters.queryText, Limit = queryFilters.Limit, idChannel = queryFilters.idChannel, idUser = queryFilters.idUser, Page = queryFilters.Page + 1 };
           // newfilter.Page = queryFilters.Page + 1;
            var newresult = new PagedCollectionResponse<Message>();
            newresult.Items = auxresult.Skip((newfilter.Page -1)* newfilter.Limit).Take(newfilter.Limit).ToList();
            String nextUrl = newresult.Items.Count() <= 0 ? null : Url.Action("GetMessage", null, newfilter, Request.Scheme);

            //Get previous page URL string  
            FilterModelMessages previousfilter = new FilterModelMessages { queryText = queryFilters.queryText, Limit = queryFilters.Limit, idChannel = queryFilters.idChannel, idUser = queryFilters.idUser, Page = queryFilters.Page - 1 };

            String previousUrl = previousfilter.Page <= 0 ? null : Url.Action("GetMessage", null, previousfilter, Request.Scheme);

            result.NextPage = !String.IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !String.IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return result;


        }

        // PUT: api/Messages/5
        [HttpPut]
        [SwaggerOperation(summary:"Update a message", description:"Method is not allowed.")]
        [SwaggerResponse(405, "Method not allowed. ")]
        public IActionResult PutMessage()
        {
            return StatusCode(405, "Method is not allowed"); 
        }

        // POST: api/Messages
        [HttpPost]
        [SwaggerOperation(summary:"Create a new message", description:"")]
        [SwaggerResponse(400, "Bad request - Model Message is not valid.")]
        [SwaggerResponse(400, "Bad request - Model User is not valid.")]
        [SwaggerResponse(400, "Bad request - Modal Channel is not valid.")]
        [SwaggerResponse(400, "Bad request - There is no right to creating a message.")]
        public IActionResult PostMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var messageUserId = message.ReferenceUserID;
            var messageChannelId = message.ReferenceChannelID;
            if (UserExists(messageUserId))
            {
                if (ChannelExists(messageChannelId))
                {
                    if (_context.UserChannels.Any(s => s.ChannelID.Equals(messageChannelId) & s.UserID.Equals(messageUserId)))
                    {
                        try
                        {
                            message.Inserted = DateTime.Now;

                           // _context.Database.ExecuteSqlCommand("INSERT INTO[dbo].[Messages]([Text] ,[Time] ,[ChannelID] ,[UserID])  VALUES( {0} , {1} , {2}, {3})", message.Text, message.Time, messageChannelId, messageUserId);
                           _context.Messages.Add(message);
                           _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException )
                        { throw; }
                    }
                    else return BadRequest("There is no right to creating a message.");
                }
                else return BadRequest("Modal Channel is not valid.");
            }
            else return BadRequest("Model User is not valid.");
            return CreatedAtAction("GetMessage", new { id = message.MessageID }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete]
        [SwaggerOperation(summary:"Delete message", description:"This method is not alllowed")]
        public IActionResult DeleteMessage()
        {
            return StatusCode(405, "Method not allowed"); 
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageID == id);
        }

        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.ChannelID == id);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id && e.Activated == true);
        }

        
        
    }
}
