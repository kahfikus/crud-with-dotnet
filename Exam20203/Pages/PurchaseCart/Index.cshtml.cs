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
    public class IndexModel : PageModel
    {
        private readonly CartService _cartAppMan;

     

        public List<PurchaseCartModel> Purchases { get; set; }

        public PurchaseViewModel Pesanan { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }

        public IndexModel(CartService cartService)
        {
            this._cartAppMan = cartService;
           
        }



        public async Task<IActionResult> OnGetAsync()
        {
            var userNameLogin = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var findLogin = await this._cartAppMan.GetLogin(userNameLogin);

            await this._cartAppMan.FetchAllById(findLogin.UserId);
            this.Purchases = this._cartAppMan.Purchases;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var userNameLogin = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var findLogin = await this._cartAppMan.GetLogin(userNameLogin);
            await this._cartAppMan.FetchAllById(findLogin.UserId);
            this.Purchases = this._cartAppMan.Purchases;

            var tanggalSekarang = DateTimeOffset.Now;

            foreach (var item in this.Purchases)
            {
                Pesanan = new PurchaseViewModel()
                {
                    UserId = item.UserId,
                    MakananId = item.MakananId,
                    MakananName = item.MakananName,
                    Quantity = item.Quantity,
                    TransactionDate = tanggalSekarang
                };

                var result = await this._cartAppMan.InsertOrder(Pesanan);
                if (result == false)
                {
                    SuccessMessage = "Failed";
                    return Page();
                }

            };

            await this._cartAppMan.SaveToDatabase();

            return Page();
        }
    }
}
