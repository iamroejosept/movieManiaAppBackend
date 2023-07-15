using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using movieManiaAppBackend.Models;

namespace movieManiaAppBackend.Models
{
    public class Customers
    {
        public int customer_id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string address { get; set; }
        public string contact_number { get; set; }

        // Navigation property
        public virtual ICollection<Rentals> Rentals { get; set; }
    }
}