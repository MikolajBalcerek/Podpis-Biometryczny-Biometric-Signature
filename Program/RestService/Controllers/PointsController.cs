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
    public class PointsController : ApiController
    {
        private RestServiceContext db = new RestServiceContext();

        // GET: api/Points
        public IQueryable<Point> GetPoints()
        {
            return db.Points;
        }

        // GET: api/Points/5
        [ResponseType(typeof(Point))]
        public IHttpActionResult GetPoint(int id)
        {
            Point point = db.Points.Find(id);
            if (point == null)
            {
                return NotFound();
            }

            return Ok(point);
        }

        // PUT: api/Points/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPoint(int id, Point point)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != point.Id)
            {
                return BadRequest();
            }

            db.Entry(point).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointExists(id))
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

        // POST: api/Points
        [ResponseType(typeof(Point))]
        public IHttpActionResult PostPoint(Point point)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Points.Add(point);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = point.Id }, point);
        }

        // DELETE: api/Points/5
        [ResponseType(typeof(Point))]
        public IHttpActionResult DeletePoint(int id)
        {
            Point point = db.Points.Find(id);
            if (point == null)
            {
                return NotFound();
            }

            db.Points.Remove(point);
            db.SaveChanges();

            return Ok(point);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PointExists(int id)
        {
            return db.Points.Count(e => e.Id == id) > 0;
        }
    }
}