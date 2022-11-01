using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Exam20203.Models;
using Exam20203.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam20203.Pages.PurchaseCart
{
    public class InsertPurchaseModel : PageModel
    {
        private readonly CartService _cartAppMan;
 

        [BindProperty(SupportsGet = true)]
        public List<ProductViewModel> ListProduct { get; set; }

        [BindProperty(SupportsGet = true)]
        public PurchaseViewModel Form { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public InsertPurchaseModel(CartService cartService)
        {
            this._cartAppMan = cartService;
            
        }

        public async Task<IActionResult> OnGetAsync()
        { 
            ListProduct = await _cartAppMan.GetAllProduct();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var userNameLogin = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var findLogin = await this._cartAppMan.GetLogin(userNameLogin);
            Form.UserId = findLogin.UserId;

            var checkProductExist = await _cartAppMan.CheckCartProductExist(Form);

            if (checkProductExist == false)
            {
                SuccessMessage = "Makanan telah ada di keranjang";
                return RedirectToPage();
            }

            var stock = await _cartAppMan.CheckStock(Form);

            if (stock == false)
            {
                SuccessMessage = "Permintaan melebihi Stock, mohon kurangi jumlah Quantity Anda";
                return RedirectToPage();
            }
            else
            {
                await _cartAppMan.InsertCart(Form);
            }


            return Redirect("/PurchaseCart/Index");
        }
    }
}
