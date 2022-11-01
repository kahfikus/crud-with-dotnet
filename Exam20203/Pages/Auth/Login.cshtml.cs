using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Exam20203.Enums;
using Exam20203.Models;
using Exam20203.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam20203.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly CartService _cartMan;

        [BindProperty]
        public LoginFromModel Form { get; set; }
        public string RoleDb { get; set; }
        public string Email { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        public LoginModel(CartService cartService)
        {
            this._cartMan = cartService;
        }

        private ClaimsPrincipal GenerateClaims()
        {
            var claims = new ClaimsIdentity(CartAuthenticationSchemes.Cookie);

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Form.Username));
            claims.AddClaim(new Claim(ClaimTypes.Role, RoleDb));
            return new ClaimsPrincipal(claims);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/PurchaseCart/Index");
            }
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var loginDb = await _cartMan.GetLogin(Form.Username);
            if (loginDb == null)
            {
                SuccessMessage = "Invalid username or password.";
                return Page();
            }

            var usernameDb = loginDb.Username;
            var passwordDb = loginDb.PasswordUser;
            RoleDb = loginDb.RoleUser;

            var isUserMatched = Form.Username == usernameDb;
            var isPassMatched = _cartMan.Verify(Form.Password, passwordDb);

            var isCredentialsOk = isUserMatched && isPassMatched;
            if (isCredentialsOk == false)
            {
                SuccessMessage = "Invalid username or password.";
                return Page();
            }

            var claims = GenerateClaims();
            var persistence = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CartAuthenticationSchemes.Cookie, claims, persistence);

            if (string.IsNullOrEmpty(returnUrl) == false)
            {
                return LocalRedirect(returnUrl);
            }

            return Redirect("~/PurchaseCart/Index");
        }
    }
}
