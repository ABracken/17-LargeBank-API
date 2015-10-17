using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LargeBank.API;
using LargeBank.API.Models;

namespace LargeBank.API.Controllers
{
    public class CustomersController : ApiController
    {
        private LargeBankEntities db = new LargeBankEntities();

        // GET: api/Customers
        public IQueryable<CustomerModel> GetCustomers()
        {
            return db.Customers.Select(c => new CustomerModel
            {
                CustomerID = c.CustomerID,
                CreatedDate = c.CreatedDate,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Address1 = c.Address1,
                Address2 = c.Address2,
                City = c.City,
                State = c.States,
                Zip = c.Zip,
            });
        }

        [Route("api/customers/{id}/accounts")]
        public IHttpActionResult GetAccountsForCustomer (int id)
        {
            var customersAccounts = db.Accounts.Where(a => a.CustomerID == id);

            return Ok(customersAccounts.Select(a => new AccountsModel
            {
                CustomerID = a.CustomerID,
                AccountID = a.AccountID,
                CreatedDate = a.CreatedDate,
                AccountNumber = a.AccountNumber,
                Balance = a.Balance,
            }));
        }

        [Route("api/customer/{id}/transactions")]
        public IHttpActionResult GetTransactionsForCustomer(int id)
        { 

            var accountsTransactions = db.Transactions.Where(t => t.Account.CustomerID == id);

            return Ok(accountsTransactions.Select(t => new TransactionsModel
            {
                TransactionID = t.TransactionID,
                AccountID = t.AccountID,
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
            }));
        }
        // GET: api/Customers/5
        [ResponseType(typeof(CustomerModel))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer dbCustomer = db.Customers.Find(id);

            if (dbCustomer == null)
            {
                return NotFound();
            }

            CustomerModel modelCustomer = new CustomerModel
            {
                CustomerID = dbCustomer.CustomerID,
                CreatedDate = dbCustomer.CreatedDate,
                FirstName = dbCustomer.FirstName,
                LastName = dbCustomer.LastName,
                Address1 = dbCustomer.Address1,
                Address2 = dbCustomer.Address2,
                City = dbCustomer.City,
                State = dbCustomer.States,
                Zip = dbCustomer.Zip,
            };
            
            return Ok(modelCustomer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerID)
            {
                return BadRequest();
            }
            var dbPutCustomer = db.Customers.Find(customer.CustomerID);

            dbPutCustomer.Update(customer);

            db.Entry(dbPutCustomer).State = EntityState.Modified;

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

        // POST: api/Customers
        [ResponseType(typeof(CustomerModel))]
        public IHttpActionResult PostCustomer(CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbPostCustomer = new Customer();

            dbPostCustomer.Update(customer);

            db.Customers.Add(dbPostCustomer);

            db.SaveChanges();

            customer.CustomerID = dbPostCustomer.CustomerID;

            return CreatedAtRoute("DefaultApi", new { id = dbPostCustomer.CustomerID }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}