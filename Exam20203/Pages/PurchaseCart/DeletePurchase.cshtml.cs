using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Exam20203.Enums;
using Exam20203.Models;
using Exam20203.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam20203.Pages.PurchaseCart
{
    [Authorize(Roles = UserRoles.Admin + ", " + UserRoles.User)]
    public class DeletePurchaseModel : PageModel
    {
        private readonly CartService _cartMan;
        


        public DeletePurchaseModel(CartService cartService)
        {
            this._cartMan = cartService;
          
        }

        [BindProperty(SupportsGet = true)]
        public PurchaseViewModel Form { set; get; }

        public async Task<ActionResult> OnGetAsync()
        {
            var userNameLogin = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userIdLogin = (await this._cartMan.GetLogin(userNameLogin)).UserId;
            

            Form = await _cartMan.GetPurchaseByProductId(Form.MakananId, userIdLogin);

            if (Form == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var userNameLogin = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userIdLogin = (await this._cartMan.GetLogin(userNameLogin)).UserId;

            Form = await _cartMan.GetPurchaseByProductId(Form.MakananId, userIdLogin);

            await _cartMan.DeleteCart(Form.MakananId, Form.UserId);
            return RedirectToPage("/PurchaseCart/Index");
        }
    }
}
