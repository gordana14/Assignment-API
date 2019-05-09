using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_API_ASP.NETCore.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Assignment_API_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly StreamContext _context;

        public ChannelsController(StreamContext context)
        {
            _context = context;
        }

        // GET: api/channels
        [HttpGet]
        [SwaggerOperation(summary: "Gets the channels", description: "List of all channels with users details")]
        [SwaggerResponse(404, "Not found - There are no channels.")]
        public IActionResult GetChannels()
        {
            IList<Channel> channels = _context.Channels.ToList();

            foreach (var ls in channels)
            {
                var usForchannel = from u in _context.Users
                                   join uc in _context.UserChannels
                                   on u.UserID equals uc.UserID
                                   where uc.ChannelID.Equals(ls.ChannelID) & u.Activated.Equals(true)
                                   select u;
                if (usForchannel != null && usForchannel.Count() > 0)
                    channels[ls.ChannelID- 1].ChannelUsers= usForchannel.ToList();
            
            }
            if (channels == null)
            {
                return NotFound("There are no channels.");
            }
            return Ok(channels);
        }


        // GET: api/channels/5
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get channel by id", description: "Channel Details with user details for the requested id")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(401, "Not found - There is no channel with the requested id.")]
        public IActionResult GetChannel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = _context.Channels.Where(s => s.ChannelID.Equals(id)).SingleOrDefault();

            if (channel == null)
            {
                return NotFound("There is no channel with the requested id.");
            }
            else
            {
                var usForchannel = from u in _context.Users
                                   join uc in _context.UserChannels
                                   on u.UserID equals uc.UserID
                                   where uc.ChannelID.Equals(id) & u.Activated.Equals(true)
                                   select u;
                channel.ChannelUsers = usForchannel.ToList();
                
            }
            
            return Ok(channel);
        }

        // PUT: api/channels/5
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update channel", description: "Upadates channel using PutChannel method. Updates relationship between channel and users. User model must be valid. ")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(400, "Bad request - User model is not valid.")]
        [SwaggerResponse(401, "Not found - There is no channel with the requested id.")]
        public IActionResult PutChannel([FromRoute] int id, [FromBody] Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != channel.ChannelID)
            {
                return BadRequest("There is no match between the channel id and id. ");
            }

            if (!ChannelExists(id))
            {
                return NotFound("There is no channel with the requested id.");
            }

            _context.Entry(channel).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                var usersBychannel = _context.UserChannels.Where(s => s.ChannelID.Equals(id));
                _context.UserChannels.RemoveRange(usersBychannel);
                _context.SaveChanges();
                if (channel.ChannelUsers != null)
                {
                    foreach (var el in channel.ChannelUsers.Where(s => s.UserID > 0))
                    {
                        if (UserExists(el.UserID) && UserExists(el.Name))
                        {
                            var userchannel = new UserChannel { UserID = el.UserID, ChannelID = id };
                            _context.UserChannels.Add(userchannel);
                            _context.SaveChanges();

                        }
                        else { return BadRequest("User model is not valid."); }

                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return CreatedAtAction("Getchannel", new { id = channel.ChannelID }, channel);
        }

        // POST: api/channels
        [HttpPost]
        [SwaggerOperation(summary: "Create New channel", description: "Creates new channel using Postchannel method. Creates a relationship between channel and channels.")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(400, "Bad request - User model is not valid.")]
        public IActionResult PostChannel([FromBody] Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Channels.Add(channel);
            try
            {
                _context.SaveChanges();
                if (channel.ChannelUsers != null)
                {
                    foreach (var ls in channel.ChannelUsers.Where(s => s.UserID > 0))
                    {
                        if (UserExists(ls.UserID) && UserExists(ls.Name))
                        {
                            var userchannel = new UserChannel { UserID = ls.UserID, ChannelID = channel.ChannelID };
                            _context.UserChannels.Add(userchannel);
                            _context.SaveChanges();

                        }
                        else { return BadRequest("User model is not valid."); }
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;// "Error during insert entry in many-many table"
            }

            return CreatedAtAction("Getchannel", new { id = channel.ChannelID }, channel);
        }

        // DELETE: api/channels/5
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete channel", description: "This method will delete channel and realtionships with user. ")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(401, "Not found - There is no channel with the requested id.")]
        public IActionResult DeleteChannel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = _context.Channels.Find(id);
            if (channel == null)
            {
                return NotFound("There is no channel with the requested id.");
            }
            var userchannel = _context.UserChannels.Where(s => s.ChannelID.Equals(id));
            _context.UserChannels.RemoveRange(userchannel);
          

            try
            {
                _context.SaveChanges();
                _context.Channels.Remove(channel);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            { throw; }

            return Ok();
        }

        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.ChannelID == id);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id && e.Activated==true);
        }

        private bool UserExists(string name)
        {
            return _context.Users.Any(e => e.Name==name && e.Activated == true);
        }

    }
}