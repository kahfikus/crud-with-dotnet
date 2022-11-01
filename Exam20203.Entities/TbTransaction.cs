using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam20203.Entities
{
    public partial class TbTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int MakananId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTimeOffset TransactionDate { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(MakananId))]
        [InverseProperty(nameof(TbMakanan.TbTransaction))]
        public virtual TbMakanan Makanan { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TbUser.TbTransaction))]
        public virtual TbUser User { get; set; }
    }
}
