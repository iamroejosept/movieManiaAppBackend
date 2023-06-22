using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using movieManiaAppBackend.Models;

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
        public IHttpActionResult GetCustomers()
        {
            List<Customers> customers = db.Customers.ToList();
            return Ok(customers);
        }

        // GET api/customers/{id}
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
            if (!ModelState.IsValid)
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

            db.Entry(customer).State = EntityState.Modified;

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
