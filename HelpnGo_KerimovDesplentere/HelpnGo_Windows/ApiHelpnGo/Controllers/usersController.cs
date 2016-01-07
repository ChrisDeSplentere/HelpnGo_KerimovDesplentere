using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApiHelpnGo.Models;

namespace ApiHelpnGo.Controllers
{
    public class usersController : ApiController
    {
        private bd_helpngoEntities db = new bd_helpngoEntities();

        // GET: api/users/all
        /*[ActionName("all")]
        public IEnumerable<user> Getusers()
        {
            return db.users.ToList();
        }*/

        //GET: api/users/all
        [ActionName("all")]
        public IEnumerable<SimpleUser> GetAllUsers()
        {
            var users = db.users.Select(user => new SimpleUser
            {
                Date_of_birth = user.Date_of_birth,
                Diplomas = user.Diplomas,
                Email = user.Email,
                Firstname = user.Firstname,
                Jobs = user.Jobs,
                Lastname = user.Lastname,
                Living_city_id = user.Living_city_id,
                password = user.password,
                Phone_number = user.Phone_number,
                Street_name = user.Street_name,
                Street_number = user.Street_number,
                User_id = user.User_id
            });
            return users.ToList();
        }

        // GET: api/users/byId/5
        /*[ActionName("byId")]
        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser(decimal id)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }*/

        // GET: api/users/byEmail/?email=chrisDS@hotmail.com
        [ActionName("byEmail")]
        public SimpleUser GetUsersByEmail(string email)
        {
            var users = db.users.Where(u => u.Email == email).Select(user => new SimpleUser
            {
                Date_of_birth = user.Date_of_birth,
                Diplomas = user.Diplomas,
                Email = user.Email,
                Firstname = user.Firstname,
                Jobs = user.Jobs,
                Lastname = user.Lastname,
                Living_city_id = user.Living_city_id,
                password = user.password,
                Phone_number = user.Phone_number,
                Street_name = user.Street_name,
                Street_number = user.Street_number,
                User_id = user.User_id
            });
            return users.ToList().First();
        }

        [ActionName("newId")]
        public decimal GetNewId()
        {
            try
            {
                var ids = db.users.Select(user => user.User_id);
                return (ids.Max() + 1);
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(decimal id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.User_id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/users
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (userExists(user.User_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.User_id }, user);
        }

        // DELETE: api/users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(decimal id)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(decimal id)
        {
            return db.users.Count(e => e.User_id == id) > 0;
        }
    }
}