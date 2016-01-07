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
using WebApiHelpnGo.Models;

namespace WebApiHelpnGo.Controllers
{
    public class publicationsController : ApiController
    {
        private bd_helpngoEntities db = new bd_helpngoEntities();

        // GET: api/publications
        public IEnumerable<publication> Getpublications()
        {
            return db.publications.ToList();
        }

        // GET: api/publications/5
        [ResponseType(typeof(publication))]
        public IHttpActionResult Getpublication(decimal id)
        {
            publication publication = db.publications.Find(id);
            if (publication == null)
            {
                return NotFound();
            }

            return Ok(publication);
        }

        // PUT: api/publications/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putpublication(decimal id, publication publication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != publication.Publication_id)
            {
                return BadRequest();
            }

            db.Entry(publication).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!publicationExists(id))
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

        // POST: api/publications
        [ResponseType(typeof(publication))]
        public IHttpActionResult Postpublication(publication publication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.publications.Add(publication);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (publicationExists(publication.Publication_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = publication.Publication_id }, publication);
        }

        // DELETE: api/publications/5
        [ResponseType(typeof(publication))]
        public IHttpActionResult Deletepublication(decimal id)
        {
            publication publication = db.publications.Find(id);
            if (publication == null)
            {
                return NotFound();
            }

            db.publications.Remove(publication);
            db.SaveChanges();

            return Ok(publication);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool publicationExists(decimal id)
        {
            return db.publications.Count(e => e.Publication_id == id) > 0;
        }
    }
}