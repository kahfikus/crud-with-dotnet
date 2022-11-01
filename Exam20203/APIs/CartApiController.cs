using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam20203.Models;
using Exam20203.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam20203.APIs
{
    [AllowAnonymous]
    [Route("api/v1/cart")]
    [ApiController]
    public class CartApiController : Controller
    {
        private readonly CartService _cartMan;

        public CartApiController(CartService cartService)
        {
            this._cartMan = cartService;
        }
        // GET: api/<controller>
        [HttpGet("get-all-transaction", Name = "getAllTransaction")]
        public async Task<ActionResult<List<TransactionViewModel>>> GetAllTransactionApi()
        {
            var data = await _cartMan.GetAllTran();

            return Ok(data);
        }

        [HttpPost("insert", Name = "insertUser")]
        public async Task<ActionResult<ResponseModel>> InsertNewUserAsync(InsertUserModel insertUserModel)
        {
            insertUserModel.Password = _cartMan.Hash(insertUserModel.Password);
            var isSuccess = await _cartMan.InsertUser(insertUserModel);

            return Ok(new ResponseModel
            {
                ResponseMessage = "Success to insert new user."
            });
        }

        [HttpGet("username-exist", Name = "usernameExist")]
        public async Task<ActionResult<InsertUserModel>> GetUsernameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(null);
            }

            var user = await _cartMan.CheckExistUsername(userName);

            if (user == null)
            {
                return Ok(null);
            }
            return Ok(user);
        }

        [HttpGet("filter-data", Name = "getFilterData")]
        public async Task<ActionResult<List<TransactionViewModel>>> GetFilterDataAsync([FromQuery] int pageIndex, int itemPerPage, string filterByName)
        {
            var data = await _cartMan.GetActivityAsync(pageIndex, itemPerPage, filterByName);

            return Ok(data);
        }

        [HttpGet("total-data", Name = "getTotalData")]
        public ActionResult<int> GetTotalData()
        {
            var totalData = _cartMan.GetTotalData();

            return Ok(totalData);
        }
    }
}
