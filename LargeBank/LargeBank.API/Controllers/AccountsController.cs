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
    public class AccountsController : ApiController
    {
        private LargeBankEntities db = new LargeBankEntities();

        // GET: api/Accounts
        public IQueryable<Account> GetAccounts()
        {
            return db.Accounts;
        }

        // GET: api/Accounts/5
        [ResponseType(typeof(AccountsModel))]
        public IHttpActionResult GetAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }
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

        // PUT: api/Accounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccount(int id, AccountsModel account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.AccountID)
            {
                return BadRequest();
            }
            var dbPutAccount = db.Accounts.Find(account.CustomerID);

            dbPutAccount.Update(account);

            db.Entry(dbPutAccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/Accounts
        [ResponseType(typeof(AccountsModel))]
        public IHttpActionResult PostAccount(AccountsModel account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbPostAccount = new Account();

            dbPostAccount.Update(account);

            db.Accounts.Add(dbPostAccount);

            db.SaveChanges();

            account.CustomerID = dbPostAccount.CustomerID;

            return CreatedAtRoute("DefaultApi", new { id = dbPostAccount.CustomerID }, account);
        }

        // DELETE: api/Accounts/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult DeleteAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            db.Accounts.Remove(account);
            db.SaveChanges();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.AccountID == id) > 0;
        }
    }
}