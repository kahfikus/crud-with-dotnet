using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam20203.Models
{
    public class LoginFromModel
    {
        [Required(ErrorMessage = "harus diisi.")]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "harus diisi.")]
        [StringLength(255, MinimumLength = 8)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
