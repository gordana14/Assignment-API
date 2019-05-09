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
    public class UsersController : ControllerBase
    {
        private readonly StreamContext _context;

        public UsersController(StreamContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [SwaggerOperation(summary: "Gets the users", description: "List of all users with channels details")]
        [SwaggerResponse(404, "There are no users.")]
        public IActionResult GetUsers()
        {
            IList<User> users = _context.Users.Where(s => s.Activated.Equals(true)).ToList();
            
            foreach (var ls in users)
            {
                    var chForUser = from ch in _context.Channels
                                    join uc in _context.UserChannels
                                    on ch.ChannelID equals uc.ChannelID
                                    where uc.Equals(ls.UserID)
                                    select ch;
                        if(chForUser!=null && chForUser.Count() > 0)
                       users[ls.UserID -1 ].UsersChannels  = chForUser.ToList();

             }
            if (users == null)
            {
                return NotFound("There are no channels.");
            }
            return Ok(users);



        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get User by id" ,description: "User Details with channel details for the requested id")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(401, "Not found - There is no user with the requested id.")]
        public IActionResult GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.Where(s => s.Activated.Equals(true) & s.UserID.Equals(id)).SingleOrDefault();

            if (user == null)
            {
                return NotFound("There is no user with the requested id.");
            }
            else
            {
                var chForUser = from ch in _context.Channels
                                join uc in _context.UserChannels
                                on ch.ChannelID equals uc.ChannelID
                                where uc.Equals(id)
                                select ch;
                user.UsersChannels = chForUser.ToList();



            }


            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update user", description: "Upadates user using PutUser method. Updates relationship between user and channel. Reference Channnel model must have correct id")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(400, "Bad request - Channel model is not valid.")]
        [SwaggerResponse(401, "Not found - There is no user with the requested id.")]
        public IActionResult PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest("There is no match between the user id and id. ");
            }

            if (!UserExists(id))
            {
                return NotFound("There is no user with the requested id.");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                var channelsByUser = _context.UserChannels.Where(s => s.UserID.Equals(id));
                _context.UserChannels.RemoveRange(channelsByUser);
                _context.SaveChanges();
                if (user.UsersChannels != null)
                {
                    foreach (var el in user.UsersChannels.Where(s => s.ChannelID > 0))
                    {
                        if (ChannelExists(el.ChannelID) && ChannelExists(el.Name))
                        {
                            var userchannel = new UserChannel { UserID = user.UserID, ChannelID = el.ChannelID };
                            _context.UserChannels.Add(userchannel);
                            _context.SaveChanges();

                        }
                        else { return BadRequest("Channel model is not valid."); }

                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        // POST: api/Users
        [HttpPost]
        [SwaggerOperation(summary: "Create New User", description: "Creates new user using PostUser method. Creates a relationship between user and channels.")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(400, "Bad request - Channel model is not valid.")]
        public IActionResult PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            try
            {
                _context.SaveChanges();
                if (user.UsersChannels != null)
                {
                    foreach (var ls in user.UsersChannels.Where(s => s.ChannelID > 0))
                    {
                        if (ChannelExists(ls.ChannelID) && ChannelExists(ls.Name))
                        {
                            var userchannel = new UserChannel { UserID = user.UserID, ChannelID = ls.ChannelID };
                            _context.UserChannels.Add(userchannel);
                            _context.SaveChanges();

                        }
                        else { return BadRequest("Channel model is not valid."); }
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;// "Error during insert entry in many-many table"
            }

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Deactive user", description: "This method will deactivate user because it's not good practice to delete. ")]
        [SwaggerResponse(400, "Bad request - Model is not valid. ")]
        [SwaggerResponse(401, "Not found - There is no user with the requested id.")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("There is no user with the requested id.");
            }
            user.Activated = false;
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            { throw; }

            return Ok();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
        
        private bool ChannelExists(int id)
        { return _context.Channels.Any(e => e.ChannelID == id); }

        private bool ChannelExists(string name)
        { return _context.Channels.Any(e => e.Name==name); }
    }
}