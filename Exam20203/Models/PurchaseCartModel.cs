using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam20203.Models
{
    public class PurchaseCartModel
    {
        public int UserId { get; set; }
        public int MakananId { get; set; }
        public string MakananName { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
