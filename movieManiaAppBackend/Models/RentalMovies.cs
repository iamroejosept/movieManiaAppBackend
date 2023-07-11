using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using movieManiaAppBackend.Models;

namespace movieManiaAppBackend.Models
{
    public class RentalMovies
    {
        public int rental_id { get; set; }
        public int movie_id { get; set; }
        public string individualstatus { get; set; }

        // Navigation properties
        public virtual Rentals Rental { get; set; }
        public virtual Movies Movie { get; set; }
        
    }
}