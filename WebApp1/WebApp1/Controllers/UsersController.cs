using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using System.Web.Http.Description;
using System.ComponentModel.DataAnnotations;
using WebApp1.Models;

namespace AA.PMTOGO.Controllers
{
    public class UsersController : ApiController
    {
        private readonly WebApp1Context _context;
        //private readonly _logger;

        public UsersController(WebApp1Context context)
        {
            _context = context;
        }

        //Get:all Users
        public IQueryable<Users> GetUser()
        {
            return _context.Users;
        }

        //Get Users by Email
        public IQueryable<Users> GetUserByEmail(string email)
        {
            return _context.Users.Where(m => m.Email.Equals(email));
        }

        /*Update User for pass recovery
        public async Task<IHttpActionResult> UpdateUser(string email, Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(email != user.Email)
            {
                return BadRequest();
            }
            _context.Update(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
 
        }*/
        //Post add new user
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> CreateUser(Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //Delete User
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> DeleteUser(string email)
        {
            Users user = await _context.Users.FindAsync(email);
            if (User == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UsersExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        // GET: Users
        /* public async Task<IActionResult> Index()
         {
             try
             {
                 return View(await _context.Users.ToListAsync());
             }
             catch
             {
                 return NotFound();
             }
         }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }
        // GET: Users/Details/5
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            if (email == null || _context.Users == null)
            {
                return NotFound();
            }
            //return the user whose matches the email
            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == email);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }




        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,email,firstName,lastName,role,passDigest,salt,username")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,email,firstName,lastName,role,passDigest,salt,username")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'WebApp1Context.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/


    }
}
