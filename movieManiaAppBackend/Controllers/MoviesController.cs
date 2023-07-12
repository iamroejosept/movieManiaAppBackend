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
    public class MoviesController : ApiController
    {
        private myDBContext db;

        public MoviesController()
        {
            db = new myDBContext();
        }

        // GET api/movies
        public IHttpActionResult GetMovies()
        {
            List<Movies> movies = db.Movies.OrderByDescending(m => m.movie_id).ToList();
            return Ok(movies);
        }

        // GET api/movies/{id}
        public IHttpActionResult GetMovie(int id)
        {
            Movies movie = db.Movies.FirstOrDefault(m => m.movie_id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        // POST api/movies
        [HttpPost]
        public IHttpActionResult PostMovie(Movies movie)
        {

            if (!ModelState.IsValidField("title") || !ModelState.IsValidField("price")
             || !ModelState.IsValidField("stock"))
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.movie_id }, movie);
        }

        // PUT api/movies/{id}
        [HttpPut]
        public IHttpActionResult PutMovie(int id, Movies movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.movie_id)
            {
                return BadRequest();
            }

            // Fetch the existing movie record from the database
            var existingMovie = db.Movies.Find(id);

            if (existingMovie == null)
            {
                return NotFound();
            }

            // Update only the modified fields
            if (!string.IsNullOrEmpty(movie.title))
            {
                existingMovie.title = movie.title;
            }

            if (movie.price != null)
            {
                existingMovie.price = movie.price;
            }

            if (movie.stock != null)
            {
                existingMovie.stock = movie.stock;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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


        // DELETE api/movies/{id}
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movies movie = db.Movies.FirstOrDefault(m => m.movie_id == id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }


        private bool MovieExists(int id)
        {
            return db.Movies.Any(m => m.movie_id == id);
        }
    }
}
