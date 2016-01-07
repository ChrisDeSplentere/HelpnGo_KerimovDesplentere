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
    public class provincesController : ApiController
    {
        private bd_helpngoEntities db = new bd_helpngoEntities();

        // GET: api/provinces/all
        [ActionName("all")]
        public IEnumerable<SimpleProvince> Getprovinces()
        {
            var provinces = db.provinces.Select(pro => new SimpleProvince
            {
                Label = pro.Label
            });
            return provinces.ToList();
        }

        // GET: api/provinces/5
        [ResponseType(typeof(province))]
        public IHttpActionResult Getprovince(string id)
        {
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return NotFound();
            }

            return Ok(province);
        }

        // PUT: api/provinces/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putprovince(string id, province province)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != province.Label)
            {
                return BadRequest();
            }

            db.Entry(province).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!provinceExists(id))
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

        // POST: api/provinces
        [ResponseType(typeof(province))]
        public IHttpActionResult Postprovince(province province)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.provinces.Add(province);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (provinceExists(province.Label))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = province.Label }, province);
        }

        // DELETE: api/provinces/5
        [ResponseType(typeof(province))]
        public IHttpActionResult Deleteprovince(string id)
        {
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return NotFound();
            }

            db.provinces.Remove(province);
            db.SaveChanges();

            return Ok(province);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool provinceExists(string id)
        {
            return db.provinces.Count(e => e.Label == id) > 0;
        }
    }
}