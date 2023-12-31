﻿using System;
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
        public DateTime? release_date { get; set; }
        public string genre { get; set; }
        public string director { get; set; }
        public string actors { get; set; }
        public decimal? price { get; set; }
        public int? stock { get; set; }

        // Navigation property
        public virtual ICollection<RentalMovies> RentalMovies { get; set; }
    }
}