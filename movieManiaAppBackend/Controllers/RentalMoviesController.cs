using movieManiaAppBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace movieManiaAppBackend.Controllers
{
    public class RentalMoviesController : ApiController
    {
        private myDBContext db;

        public RentalMoviesController()
        {
            db = new myDBContext();
        }

        // GET api/rentalmovies
        public IHttpActionResult GetRentalMovies()
        {
            List<RentalMovies> rentalmovies = db.RentalMovies.OrderByDescending(rm => rm.rental_id).ToList();

            return Ok(rentalmovies);
        }

        // GET api/rentalmovies/{id}
        public IHttpActionResult GetRentalMovie(int id)
        {
            List<RentalMovies> rentalMovies = db.RentalMovies.Where(rm => rm.rental_id == id).ToList();
            if (rentalMovies.Count == 0)
            {
                return NotFound();
            }
            return Ok(rentalMovies);
        }

        // POST api/rentalmovies
        [HttpPost]
        public IHttpActionResult PostRentalMovie(RentalMovies rentalmovie)
        {
            if (!ModelState.IsValidField("rental_id") || !ModelState.IsValidField("movie_id")
              || !ModelState.IsValidField("individualstatus") || !ModelState.IsValidField("price"))
            {
                return BadRequest(ModelState);
            }

            // Retrieve the movie from the database based on the movie_id
            var movie = db.Movies.FirstOrDefault(m => m.movie_id == rentalmovie.movie_id);

            // Check if there is enough stock of the movie
            if (movie.stock <= 0)
            {
                return BadRequest("Movie out of stock.");
            }

            // Decrease the stock of the movie by 1
            movie.stock--;

            db.RentalMovies.Add(rentalmovie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rentalmovie.rental_id }, rentalmovie);
        }

        // PUT api/rentalmovies/{rentalId}-{movieId}
        [HttpPut]
        [Route("api/rentalmovies/{rentalId}-{movieId}")]
        public IHttpActionResult PutRentalMovie(int rentalId, int movieId, RentalMovies rentalmovie)
        {
            // Fetch the existing record from the database
            var existingRentalMovie = db.RentalMovies.Find(rentalId, movieId);

            if (existingRentalMovie == null)
            {
                return NotFound();
            }

            // Update only the modified field
            if (!string.IsNullOrEmpty(rentalmovie.individualstatus))
            {
                existingRentalMovie.individualstatus = rentalmovie.individualstatus;
            }

            if (rentalmovie.price != null)
            {
                existingRentalMovie.price = rentalmovie.price;
            }

            // Retrieve the movie from the database based on the movie_id
            var movie = db.Movies.FirstOrDefault(m => m.movie_id == movieId);

            // Decrease the stock of the movie by 1
            movie.stock++;

            try
            {
                db.SaveChanges();

                // Check if all RentalMovies for the rentalId have individualstatus as "Returned"
                var allReturned = db.RentalMovies
                    .Where(rm => rm.rental_id == rentalId)
                    .All(rm => rm.individualstatus == "Returned");

                if (allReturned)
                {
                    // Fetch the existing Rentals record from the database
                    var existingRental = db.Rentals.Find(rentalId);

                    if (existingRental == null)
                    {
                        return NotFound();
                    }

                    // Update the status of the rental to "Returned"
                    existingRental.status = "Returned";

                    db.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalMovieExists(rentalId, movieId))
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


        private bool RentalMovieExists(int rentalId, int movieId)
        {
            return db.RentalMovies.Any(rm => rm.rental_id == rentalId && rm.movie_id == movieId);
        }



    }
}
