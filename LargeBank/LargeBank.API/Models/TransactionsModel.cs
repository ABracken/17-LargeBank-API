using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LargeBank.API.Models
{
    public class TransactionsModel
    {
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
    }
}