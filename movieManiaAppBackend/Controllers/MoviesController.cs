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
        [HttpGet]
        public IHttpActionResult GetMoviesAll()
        {
            List<Movies> movies = db.Movies.OrderByDescending(m => m.movie_id).ToList();
            return Ok(movies);
        }

        //GET api/movies/{page}/{limit}
        [HttpGet]
        public IHttpActionResult GetMoviesPagination(int page, int limit, string search = null)
        {
            // Calculate the number of records to skip based on the page and pageSize
            int skip = (page - 1) * limit;

            // Create a queryable object for the movies
            IQueryable<Movies> query = db.Movies.OrderByDescending(m => m.movie_id);

            // Apply the search filter if a value is provided
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.title.Contains(search) ||
                    m.director.Contains(search) ||
                    m.genre.Contains(search) ||
                    m.release_date.ToString().Contains(search) ||
                    m.actors.Contains(search) ||
                    m.stock.ToString().Contains(search) ||
                    m.price.ToString().Contains(search));
            }

            // Retrieve movies based on the calculated skip and pageSize
            List<Movies> movies = query
                .Skip(skip)
                .Take(limit)
                .ToList();

            // Retrieve the total number of movies
            int totalRecords = query.Count();

            // Create a response object containing the movies and total number of records
            var response = new
            {
                data = movies,
                total = totalRecords
            };

            return Ok(response);
        }

        // GET api/movies/{id}
        [HttpGet]
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
             || !ModelState.IsValidField("stock") || !ModelState.IsValidField("release_date")
             || !ModelState.IsValidField("genre") || !ModelState.IsValidField("director")
             || !ModelState.IsValidField("actors"))
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

            if (!string.IsNullOrEmpty(movie.genre))
            {
                existingMovie.genre = movie.genre;
            }

            if (!string.IsNullOrEmpty(movie.director))
            {
                existingMovie.director = movie.director;
            }

            if (!string.IsNullOrEmpty(movie.actors))
            {
                existingMovie.actors = movie.actors;
            }

            if (movie.release_date.HasValue)
            {
                existingMovie.release_date = movie.release_date;
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
