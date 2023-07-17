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
    public class CustomersController : ApiController
    {
        private myDBContext db;

        public CustomersController()
        {
            db = new myDBContext();
        }

        // GET api/customers
        [HttpGet]
        public IHttpActionResult GetCustomersAll()
        {
            List<Customers> customers = db.Customers.OrderByDescending(c => c.customer_id).ToList();
            return Ok(customers);
        }

        //GET api/customers/{page}/{limit}
        [HttpGet]
        public IHttpActionResult GetCustomersPagination(int page, int limit)
        {
            // Calculate the number of records to skip based on the page and pageSize
            int skip = (page - 1) * limit;

            // Retrieve customers based on the calculated skip and pageSize
            List<Customers> customers = db.Customers
                .OrderByDescending(c => c.customer_id)
                .Skip(skip)
                .Take(limit)
                .ToList();

            // Retrieve the total number of customers
            int totalRecords = db.Customers.Count();

            // Create a response object containing the customers and total number of records
            var response = new
            {
                data = customers,
                total = totalRecords
            };

            return Ok(response);
        }

        // GET api/customers/{id}
        [HttpGet]
        public IHttpActionResult GetCustomer(int id)
        {
            Customers customer = db.Customers.FirstOrDefault(c => c.customer_id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // POST api/customers
        [HttpPost]
        public IHttpActionResult PostCustomer(Customers customer)
        {
            
            if (!ModelState.IsValidField("email") || !ModelState.IsValidField("first_name")
             || !ModelState.IsValidField("last_name") || !ModelState.IsValidField("date_of_birth")
             || !ModelState.IsValidField("address") || !ModelState.IsValidField("contact_number"))
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.customer_id }, customer);
        }

        // PUT api/customers/{id}
        [HttpPut]
        public IHttpActionResult PutCustomer(int id, Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.customer_id)
            {
                return BadRequest();
            }

            // Fetch the existing customer record from the database
            var existingCustomer = db.Customers.Find(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Update only the modified fields
            if (!string.IsNullOrEmpty(customer.email))
            {
                existingCustomer.email = customer.email;
            }

            if (!string.IsNullOrEmpty(customer.first_name))
            {
                existingCustomer.first_name = customer.first_name;
            }

            if (!string.IsNullOrEmpty(customer.last_name))
            {
                existingCustomer.last_name = customer.last_name;
            }

            if (customer.date_of_birth.HasValue)
            {
                existingCustomer.date_of_birth = customer.date_of_birth;
            }

            if (!string.IsNullOrEmpty(customer.address))
            {
                existingCustomer.address = customer.address;
            }

            if (!string.IsNullOrEmpty(customer.contact_number))
            {
                existingCustomer.contact_number = customer.contact_number;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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


        // DELETE api/customers/{id}
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customers customer = db.Customers.FirstOrDefault(c => c.customer_id == id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }


        private bool CustomerExists(int id)
        {
            return db.Customers.Any(c => c.customer_id == id);
        }
    }
}
