using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam20203.Entities
{
    public partial class TbMakanan
    {
        public TbMakanan()
        {
            TbTransaction = new HashSet<TbTransaction>();
        }

        [Key]
        public int MakananId { get; set; }
        [Required]
        [StringLength(255)]
        public string MakananName { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal MakananHarga { get; set; }
        public int MakananStock { get; set; }
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

        [InverseProperty("Makanan")]
        public virtual ICollection<TbTransaction> TbTransaction { get; set; }
    }
}
