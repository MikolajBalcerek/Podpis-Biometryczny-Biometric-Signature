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
    public class SignaturesController : ApiController
    {
        private RestServiceContext db = new RestServiceContext();

        // GET: api/Signatures
        public IQueryable<Signature> GetSignatures()
        {
            return db.Signatures;
        }

        // GET: api/Signatures/5
        [ResponseType(typeof(Signature))]
        public IHttpActionResult GetSignature(int id)
        {
            Signature signature = db.Signatures.Find(id);
            if (signature == null)
            {
                return NotFound();
            }

            return Ok(signature);
        }

        // PUT: api/Signatures/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSignature(int id, Signature signature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != signature.Id)
            {
                return BadRequest();
            }

            db.Entry(signature).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignatureExists(id))
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

        // POST: api/Signatures
        [ResponseType(typeof(Signature))]
        public IHttpActionResult PostSignature(Signature signature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Signatures.Add(signature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = signature.Id }, signature);
        }

        // DELETE: api/Signatures/5
        [ResponseType(typeof(Signature))]
        public IHttpActionResult DeleteSignature(int id)
        {
            Signature signature = db.Signatures.Find(id);
            if (signature == null)
            {
                return NotFound();
            }

            db.Signatures.Remove(signature);
            db.SaveChanges();

            return Ok(signature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SignatureExists(int id)
        {
            return db.Signatures.Count(e => e.Id == id) > 0;
        }
    }
}