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
using RestService.Models;

namespace RestService.Controllers
{
    public class StrokesController : ApiController
    {
        private RestServiceContext db = new RestServiceContext();

        // GET: api/Strokes
        public IQueryable<Stroke> GetStrokes()
        {
            return db.Strokes;
        }

        // GET: api/Strokes/5
        [ResponseType(typeof(Stroke))]
        public IHttpActionResult GetStroke(int id)
        {
            Stroke stroke = db.Strokes.Find(id);
            if (stroke == null)
            {
                return NotFound();
            }

            return Ok(stroke);
        }

        // PUT: api/Strokes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStroke(int id, Stroke stroke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stroke.Id)
            {
                return BadRequest();
            }

            db.Entry(stroke).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrokeExists(id))
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

        // POST: api/Strokes
        [ResponseType(typeof(Stroke))]
        public IHttpActionResult PostStroke(Stroke stroke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Strokes.Add(stroke);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stroke.Id }, stroke);
        }

        // DELETE: api/Strokes/5
        [ResponseType(typeof(Stroke))]
        public IHttpActionResult DeleteStroke(int id)
        {
            Stroke stroke = db.Strokes.Find(id);
            if (stroke == null)
            {
                return NotFound();
            }

            db.Strokes.Remove(stroke);
            db.SaveChanges();

            return Ok(stroke);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StrokeExists(int id)
        {
            return db.Strokes.Count(e => e.Id == id) > 0;
        }
    }
}