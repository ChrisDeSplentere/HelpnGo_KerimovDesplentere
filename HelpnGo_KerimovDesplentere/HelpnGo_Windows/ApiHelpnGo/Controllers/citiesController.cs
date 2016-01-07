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
    public class citiesController : ApiController
    {
        private bd_helpngoEntities db = new bd_helpngoEntities();

        // GET: api/cities/all
        [ActionName("all")]
        public IEnumerable<SimpleCity> Getcities()
        {
            var cities = db.cities.Select(city => new SimpleCity
            {
                City_id = city.City_id,
                Country = city.Country,
                Name = city.Name,
                Postal_code = city.Postal_code
            });
            return cities.ToList();
        }

        // GET: api/cities/byId/5
        [ActionName("byId")]
        public SimpleCity GetcityById(decimal id)
        {
            var cities = db.cities.Where(c => c.City_id == id).Select(city => new SimpleCity
            {
                City_id = city.City_id,
                Country = city.Country,
                Name = city.Name,
                Postal_code = city.Postal_code
            });
            return cities.First();
        }

        // GET: api/cities/5
        /*[ResponseType(typeof(city))]
        public IHttpActionResult Getcity(decimal id)
        {
            city city = db.cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }*/

        // PUT: api/cities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcity(decimal id, city city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.City_id)
            {
                return BadRequest();
            }

            db.Entry(city).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cityExists(id))
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

        // POST: api/cities
        [ResponseType(typeof(city))]
        public IHttpActionResult Postcity(city city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cities.Add(city);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (cityExists(city.City_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = city.City_id }, city);
        }

        // DELETE: api/cities/5
        [ResponseType(typeof(city))]
        public IHttpActionResult Deletecity(decimal id)
        {
            city city = db.cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            db.cities.Remove(city);
            db.SaveChanges();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cityExists(decimal id)
        {
            return db.cities.Count(e => e.City_id == id) > 0;
        }
    }
}