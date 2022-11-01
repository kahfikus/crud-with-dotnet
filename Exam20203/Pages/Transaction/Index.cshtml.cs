using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam20203.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam20203.Pages.Transaction
{
    [Authorize(Roles = UserRoles.Admin )]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
