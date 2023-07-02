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
                        orderby r.rental_id descending 
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

            // Set rental details
            rental.rental_date = DateTime.Now;
            rental.return_date = DateTime.Now.AddMonths(1);
            rental.status = "Pending";

            // Retrieve the movie from the database based on the title
            var movie = db.Movies.FirstOrDefault(m => m.title == rental.Movie.title);
            if (movie == null)
            {
                return BadRequest("Movie not found.");
            }

            // Check if there is enough stock of the movie
            if (movie.stock <= 0)
            {
                return BadRequest("Movie out of stock.");
            }

            // Decrease the stock of the movie by 1
            movie.stock--;

            rental.movie_id = movie.movie_id;
            rental.Movie = null; // Remove the movie navigation property

            // Retrieve the customer from the database based on the first name and last name
            var customer = db.Customers.FirstOrDefault(c =>
                c.first_name == rental.Customer.first_name &&
                c.last_name == rental.Customer.last_name);

            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            rental.customer_id = customer.customer_id;
            rental.Customer = null; // Remove the customer navigation property

            // Add rental to the database
            db.Rentals.Add(rental);

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rental.rental_id }, rental);
        }


        // PUT api/rentals
        [HttpPut]
        public IHttpActionResult PutRental(Rentals rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the movie from the database based on the title
            var movie = db.Movies.FirstOrDefault(m => m.title == rental.Movie.title);
            if (movie == null)
            {
                return BadRequest("Movie not found.");
            }
            rental.movie_id = movie.movie_id;
            rental.Movie = null; // Remove the movie navigation property

            // Increase the stock of the movie by 1
            movie.stock++;

            // Retrieve the customer from the database based on the first name and last name
            var customer = db.Customers.FirstOrDefault(c =>
                c.first_name == rental.Customer.first_name &&
                c.last_name == rental.Customer.last_name);

            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            rental.customer_id = customer.customer_id;
            rental.Customer = null; // Remove the customer navigation property

            // Retrieve the rental from the database based on the customer_id and movie_id
            var tempRental = db.Rentals.FirstOrDefault(r => r.movie_id == rental.movie_id &&
                r.customer_id == rental.customer_id && r.status == "Pending");

            if (tempRental == null)
            {
                return BadRequest("Rental not found.");
            }

            //Change the status from Pending to Returned
            tempRental.status = "Returned";


            db.Entry(tempRental).State = EntityState.Modified;

            db.SaveChanges();

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
    }
}
