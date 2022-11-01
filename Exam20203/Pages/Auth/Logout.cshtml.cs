using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam20203.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam20203.Pages.Auth
{
    [Authorize(Roles = UserRoles.Admin + ", " + UserRoles.User)]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CartAuthenticationSchemes.Cookie);
            }
            return Redirect("~/Auth/Login");
        }
    }
}
