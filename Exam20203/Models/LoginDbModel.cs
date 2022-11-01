using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam20203.Models
{
    public class LoginDbModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordUser { get; set; }
        public string RoleUser { get; set; }
    }
}
