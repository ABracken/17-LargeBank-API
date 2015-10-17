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
    public class TransactionsController : ApiController
    {
        private LargeBankEntities db = new LargeBankEntities();

        // GET: api/Transactions
        public IQueryable<TransactionsModel> GetTransactions()
        {
            return db.Transactions.Select(t => new TransactionsModel
            {
                TransactionID = t.TransactionID,
                AccountID = t.AccountID,
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
            });
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(TransactionsModel))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction dbTransaction = db.Transactions.Find(id);

            if (dbTransaction == null)
            {
                return NotFound();
            }
            TransactionsModel modelTransaction = new TransactionsModel
            {
                TransactionID = dbTransaction.TransactionID,
                AccountID = dbTransaction.AccountID,
                TransactionDate = dbTransaction.TransactionDate,
                Amount = dbTransaction.Amount,
            };
            return Ok(dbTransaction);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, TransactionsModel transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }
            var dbPutTransaction = db.Transactions.Find(transaction.TransactionID);

            dbPutTransaction.Update(transaction);

            db.Entry(dbPutTransaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        [ResponseType(typeof(TransactionsModel))]
        public IHttpActionResult PostTransaction(TransactionsModel transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbPostTransaction = new Transaction();

            dbPostTransaction.Update(transaction);

            dbPostTransaction.Add(transaction);

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dbPostTransaction.TransactionID }, transaction);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.TransactionID == id) > 0;
        }
    }
}