using System;
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
        public DateTime? rental_date { get; set; }
        public string status { get; set; }
        public decimal? total_price { get; set; }

        // Navigation properties
        public virtual Customers Customer { get; set; }
        public virtual ICollection<RentalMovies> RentalMovies { get; set; }

    }
}