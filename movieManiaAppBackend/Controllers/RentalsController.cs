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
        public IHttpActionResult GetCustomers()
        {
            var query = from r in db.Rentals
                        join c in db.Customers on r.customer_id equals c.customer_id
                        join m in db.Movies on r.movie_id equals m.movie_id
                        where r.customer_id != null && r.movie_id != null
                        select new
                        {
                            rental_id = r.rental_id,
                            title = m.title,
                            first_name = c.first_name,
                            last_name = c.last_name,
                            rental_date = r.rental_date,
                            return_date = r.return_date,
                            status = r.status
                        };

            return Ok(query.ToList());
        }

        // GET api/rentals/{id}
        public IHttpActionResult GetRental(int id)
        {
            Rentals rental = db.Rentals.FirstOrDefault(c => c.rental_id == id);
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
            Rentals rental = db.Rentals.FirstOrDefault(c => c.rental_id == id);
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
            return db.Rentals.Any(c => c.rental_id == id);
        }
    }
}
