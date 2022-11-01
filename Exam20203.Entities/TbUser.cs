using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam20203.Entities
{
    public partial class TbUser
    {
        public TbUser()
        {
            TbTransaction = new HashSet<TbTransaction>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [StringLength(255)]
        public string UserRole { get; set; }
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

        [InverseProperty("User")]
        public virtual ICollection<TbTransaction> TbTransaction { get; set; }
    }
}
