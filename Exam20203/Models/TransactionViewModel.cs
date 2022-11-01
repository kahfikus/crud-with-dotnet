using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam20203.Models
{
    public class TransactionViewModel
    {
        /// <summary>
        /// keterangan...
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Customer Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Nama Makanan
        /// </summary>
        public string MakananName { get; set; }

        /// <summary>
        /// Merupakan quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// waktu transaksi
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }
    }
}
