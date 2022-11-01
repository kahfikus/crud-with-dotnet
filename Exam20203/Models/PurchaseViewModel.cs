using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam20203.Models
{
    public class PurchaseViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "harus diisi.")]
        [Display(Name = "Product Id")]
        public int MakananId { get; set; }

        public string MakananName { get; set; }

        [Required(ErrorMessage = "harus diisi.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
