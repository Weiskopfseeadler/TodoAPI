using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Controllers;
using TodoApi.Services;


namespace TodoApi.Controllers
{
    /*
    Use Following Credentials for the Database:
    Username: user.db.service
    Password: [DEIN HEIMATORT]90
     */

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ToDoContext _context;

        public UserController(ToDoContext context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                // Create a new User if collection is empty,
                // which means you can't delete all Users.
                _context.Users.Add(new User
                {

                });
                _context.SaveChanges();



            }
        }


        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            List<User> UserList = new List<User>();
            UserList = _context.Users.ToList();
            foreach (User item in UserList)
            {
                item.Pwhash = null;
            }
            return  UserList;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {

           System.Console.WriteLine("*-**-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
           user.Pwhash = SecurityService.Hash(user);
           System.Console.WriteLine(user.Name);
           System.Console.WriteLine(user.Pwhash);
           System.Console.WriteLine(user);
           System.Console.WriteLine(user.Pwhash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

  
        [HttpPost("Check")]
        public async Task<ActionResult<User>> CheckUser(User user)
        {
           // user.Pwhash = SecurityService.Hash(user.Pwhash,SecurityService.creatSalt(user));
           System.Console.WriteLine("*-**-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
           if (user.Name == "Teapot")
           {
               return StatusCode(418);
           }
           System.Console.WriteLine(user.Pwhash);
           try
           {
              User dbtestUser = _context.Users.First(t=> t.Name == user.Name ); 
           }
           catch (System.Exception)
           {
               
               return StatusCode(409);
           }
           
           User dbUser = _context.Users.First(t=> t.Name == user.Name ); 
           user.Email = dbUser.Email;
            
            
            
            if( SecurityService.CanAuthenticate(SecurityService.Hash(user),dbUser.Pwhash))
            {                
                dbUser.Pwhash = null;
                return dbUser;
            }
            return StatusCode(409);


        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            List<User> UserList = new List<User>();
            foreach (User item in _context.Users)
            {
                item.Pwhash = null;
                UserList.Add(item);

            }

            return UserList;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            Console.WriteLine("delet API");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            List<User> UserList = new List<User>();
            foreach (User item in _context.Users)
            {
                item.Pwhash = null;
                UserList.Add(item);

            }

            return UserList;
        }
    }
}