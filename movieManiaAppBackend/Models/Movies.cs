using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using movieManiaAppBackend.Models;

namespace movieManiaAppBackend.Models
{
    public class Movies
    {
        public int movie_id { get; set; }
        public string title { get; set; }
        public decimal? price { get; set; }
        public int? stock { get; set; }

        // Navigation property
        public virtual ICollection<Rentals> Rentals { get; set; }
    }
}