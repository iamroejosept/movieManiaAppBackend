using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using movieManiaAppBackend.Models;

namespace movieManiaAppBackend.Controllers
{
    public class RentalsController : ApiController
    {
        private myDBContext db;

        public RentalsController()
        {
            db = new myDBContext();
        }

        // GET api/rentals
        public IHttpActionResult GetRentals()
        {
            List<Rentals> rentals = db.Rentals.OrderByDescending(r => r.rental_id).ToList();

            return Ok(rentals);
        }

        // GET api/rentals/{id}
        public IHttpActionResult GetRental(int id)
        {
            Rentals rental = db.Rentals.FirstOrDefault(r => r.rental_id == id);
            if (rental == null)
            {
                return NotFound();
            }
            return Ok(rental);
        }

        // POST api/rentals
        [HttpPost]
        public IHttpActionResult PostRental(Rentals rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rentals.Add(rental);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rental.rental_id }, rental);
        }

        // PUT api/rentals/{id}
        [HttpPut]
        public IHttpActionResult PutRental(int id, Rentals rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rental.rental_id)
            {
                return BadRequest();
            }

            db.Entry(rental).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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

        // DELETE api/rentals/{id}
        public IHttpActionResult DeleteRental(int id)
        {
            Rentals rental = db.Rentals.FirstOrDefault(r => r.rental_id == id);
            if (rental == null)
            {
                return NotFound();
            }

            db.Rentals.Remove(rental);
            db.SaveChanges();

            return Ok(rental);
        }


        private bool RentalExists(int id)
        {
            return db.Rentals.Any(r => r.rental_id == id);
        }
    }
}
