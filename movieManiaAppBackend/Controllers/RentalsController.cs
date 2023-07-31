using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using movieManiaAppBackend.Models;
using System.Globalization;

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
        [HttpGet]
        public IHttpActionResult GetRentalsAll()
        {
            List<Rentals> rentals = db.Rentals.OrderByDescending(r => r.rental_id).ToList();
            return Ok(rentals);
        }

        //GET api/rentals/{page}/{limit}
        [HttpGet]
        public IHttpActionResult GetRentalsPagination(int page, int limit, string search = null)
        {
            // Calculate the number of records to skip based on the page and pageSize
            int skip = (page - 1) * limit;

            // Create a queryable object for the rentals
            IQueryable<Rentals> query = db.Rentals.OrderByDescending(r => r.rental_id);

            // Apply the search filter if a value is provided
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r =>
                    (r.Customer.first_name + " " + r.Customer.last_name).Contains(search) ||
                    r.rental_date.ToString().Contains(search) ||
                    r.status.ToString().Contains(search) ||
                    r.total_price.ToString().Contains(search));
            }

            // Retrieve rentals based on the calculated skip and pageSize
            List<Rentals> rentals = query
                .Skip(skip)
                .Take(limit)
                .ToList();

            // Retrieve the total number of rentals
            int totalRecords = query.Count();

            // Create a response object containing the rentals and total number of records
            var response = new
            {
                data = rentals,
                total = totalRecords
            };

            return Ok(response);
        }

        // GET api/rentals/{id}
        [HttpGet]
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

            if (!ModelState.IsValidField("customer_id") || !ModelState.IsValidField("rental_date")
             || !ModelState.IsValidField("status")
             || !ModelState.IsValidField("total_price"))
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

            // Fetch the existing rental record from the database
            var existingRental = db.Rentals.Find(id);

            if (existingRental == null)
            {
                return NotFound();
            }

            // Update only the modified fields
            if (rental.customer_id != null)
            {
                existingRental.customer_id = rental.customer_id;
            }

            if (rental.rental_date.HasValue)
            {
                existingRental.rental_date = rental.rental_date;
            }

            if (!string.IsNullOrEmpty(rental.status))
            {
                existingRental.status = rental.status;
            }

            if (rental.total_price != null)
            {
                existingRental.total_price = rental.total_price;
            }

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
        [HttpDelete]
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
