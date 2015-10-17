using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LargeBank.API.Models
{
    public class AccountsModel
    {
        public int AccountID { get; set; }
        public int CustomerID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountNumber { get; set; }
        public decimal? Balance { get; set; }
    }
}