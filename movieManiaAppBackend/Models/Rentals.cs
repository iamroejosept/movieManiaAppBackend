﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using movieManiaAppBackend.Models;

namespace movieManiaAppBackend.Models
{
    public class Rentals
    {
        public int rental_id { get; set; }
        public int? customer_id { get; set; }
        public int? movie_id { get; set; }
        public DateTime? rental_date { get; set; }
        public DateTime? return_date { get; set; }
        public string status { get; set; }

        // Navigation properties
        public virtual Customers Customer { get; set; }
        public virtual Movies Movie { get; set; }
    }
}