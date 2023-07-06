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
        public IHttpActionResult GetRentalMovies(int id)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.RentalMovies.Add(rentalmovie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rentalmovie.rental_id }, rentalmovie);
        }

        // PUT api/rentalmovies/{id}
        [HttpPut]
        public IHttpActionResult PutRentalMovie(int id, RentalMovies rentalmovie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rentalmovie.rental_id)
            {
                return BadRequest();
            }

            db.Entry(rentalmovie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalMovieExists(id))
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

        // DELETE api/rentalmovies/{id}
        public IHttpActionResult DeleteRentalMovie(int id)
        {
            RentalMovies rentalmovie = db.RentalMovies.FirstOrDefault(rm => rm.rental_id == id);
            if (rentalmovie == null)
            {
                return NotFound();
            }

            db.RentalMovies.Remove(rentalmovie);
            db.SaveChanges();

            return Ok(rentalmovie);
        }


        private bool RentalMovieExists(int id)
        {
            return db.RentalMovies.Any(rm => rm.rental_id == id);
        }
    }
}
