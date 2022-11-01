using Exam20203.Entities;
using Exam20203.Enums;
using Exam20203.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam20203.Services
{
    public class CartService
    {
        private readonly CartDbContext _cartDbContext;
        private readonly IDistributedCache _cacheMan;
        private readonly string _cacheKey = "Purchases";
        public List<PurchaseCartModel> Purchases = new List<PurchaseCartModel>();
        public List<PurchaseViewModel> ListCart = new List<PurchaseViewModel>();
        public bool IsLoaded = false;

        public CartService(CartDbContext dbContext, IDistributedCache distributedCache)
        {
            this._cartDbContext = dbContext;
            this._cacheMan = distributedCache;
        }

        public async Task<List<TransactionViewModel>> GetAllTran()
        {
            var query = from t in _cartDbContext.TbTransaction
                        join b in _cartDbContext.TbMakanan
                        on t.MakananId equals b.MakananId
                        join c in _cartDbContext.TbUser
                        on t.UserId equals c.UserId
                        select new TransactionViewModel
                        {
                            TransactionId = t.TransactionId,
                            UserName = c.UserName,
                            MakananName = b.MakananName,
                            Quantity = t.Quantity,
                            TransactionDate = t.TransactionDate
                        };
            var data = await query
                .AsNoTracking()
                .ToListAsync();
            return data;
        }

        public async Task<LoginDbModel> GetLogin(string username)
        {
            var getData = await this._cartDbContext.TbUser
                .Where(Q => Q.UserName == username)
                .Select(Q => new LoginDbModel
                {
                    UserId = Q.UserId,
                    Username = Q.UserName,
                    PasswordUser = Q.Password,
                    RoleUser = Q.UserRole,
                }).FirstOrDefaultAsync();
            return getData;
        }

        public bool Verify(string notHash, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(notHash, hash);
        }

        public async Task FetchAllById(int userId)
        {
            if (this.IsLoaded)
            {
                return;
            }

            var all = await _cacheMan.GetStringAsync(this._cacheKey);
            this.IsLoaded = true;
            if (all == null)
            {
                return;
            }

            this.Purchases = JsonSerializer.Deserialize<List<PurchaseCartModel>>(all);

           

            this.Purchases = this.Purchases
                .Where(Q => Q.UserId == userId)
                .ToList();
        }

        public async Task<ProductViewModel> GetProductDataById(int makananId)
        {
            var getProduct = await _cartDbContext.TbMakanan
                .Where(Q => Q.MakananId == makananId)
                .Select(Q => new ProductViewModel
                {
                    MakananName = Q.MakananName,
                    MakananHarga = Q.MakananHarga,
                    MakananStock = Q.MakananStock
                })
                .FirstOrDefaultAsync();

            return getProduct;
        }

        private async Task SaveToCache(int userId)
        {
            await FetchAllById(userId);
            var serialized = JsonSerializer.Serialize(this.Purchases);
            await _cacheMan.SetStringAsync(this._cacheKey, serialized);
        }

        public async Task InsertCart(PurchaseViewModel model)
        {
            var getProduct = await GetProductDataById(model.MakananId);

            await FetchAllById(model.UserId);

            this.Purchases.Add(new PurchaseCartModel
            {
                UserId = model.UserId,
                MakananId = model.MakananId,
                MakananName = getProduct.MakananName,
                TransactionDate = DateTimeOffset.UtcNow,
                Quantity = model.Quantity
            });

            await SaveToCache(model.MakananId);
        }

        public async Task<List<ProductViewModel>> GetAllProduct()
        {
            var getProduct = await _cartDbContext.TbMakanan
                .Select(Q => new ProductViewModel
                {
                    MakananId = Q.MakananId,
                    MakananName = Q.MakananName,
                    MakananHarga = Q.MakananHarga,
                    MakananStock = Q.MakananStock
                }).ToListAsync();

            return getProduct;
        }

        public async Task<bool> CheckCartProductExist(PurchaseViewModel model)
        {
            await FetchAllById(model.UserId);

            var productExist = Purchases.Where(Q => Q.MakananId == model.MakananId)
                .FirstOrDefault();

            if (productExist != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckStock(PurchaseViewModel model)
        {
            var getProduct = await GetProductDataById(model.MakananId);

            if (model.Quantity > getProduct.MakananStock)
            {
                return false;
            }

            return true;
        }

        public async Task<PurchaseViewModel> GetPurchaseByProductId(int makananId, int userId)
        {
            await FetchAllById(userId);

            var findProduct = Purchases
                .Where(Q => Q.MakananId == makananId)
                .FirstOrDefault();

            if (findProduct == null)
            {
                return new PurchaseViewModel();
            }

            var result = new PurchaseViewModel()
            {
                UserId = findProduct.UserId,
                MakananId = findProduct.MakananId,
                MakananName = findProduct.MakananName,
                Quantity = findProduct.Quantity
            };

            return result;
        }

        public async Task UpdateCart(PurchaseViewModel model)
        {
            await FetchAllById(model.UserId);
            var findProduct = Purchases
                .Where(Q => Q.MakananId == model.MakananId)
                .FirstOrDefault();
            findProduct.Quantity = model.Quantity;
            await SaveToCache(model.UserId);
        }

        public async Task DeleteCart(int makananId, int userId)
        {
            await FetchAllById(userId);

            var deletePurchase = Purchases
                .Where(Q => Q.MakananId == makananId)
                .FirstOrDefault();

            this.Purchases.Remove(deletePurchase);

            await SaveToCache(userId);
        }

        public async Task<bool> InsertOrder(PurchaseViewModel model)
        {
            this._cartDbContext.TbTransaction.Add(new TbTransaction
            {
                UserId = model.UserId,
                MakananId = model.MakananId,
                Quantity = model.Quantity,
                TransactionDate = model.TransactionDate
            });
            return true;
        }

        public async Task<bool> SaveToDatabase()
        {
            await this._cartDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<InsertUserModel> CheckExistUsername(string userName)
        {
            var user = await this._cartDbContext
                .TbUser
                .Where(Q => Q.UserName == userName)
                .Select(Q => new InsertUserModel
                {
                    UserId = Q.UserId,
                    UserName = Q.UserName,
                    Password = Q.Password,
                    UserRole = Q.UserRole
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public async Task<bool> InsertUser(InsertUserModel insertUserModel)
        {
            this._cartDbContext.TbUser.Add(new TbUser
            {
                UserName = insertUserModel.UserName,
                Password = insertUserModel.Password,
                UserRole = insertUserModel.UserRole
            });

            await _cartDbContext.SaveChangesAsync();
            return true;
        }



        public async Task<List<TransactionViewModel>> GetActivityAsync(int pageIndex, int itemPerPage, string filterByName)
        {
            var query = this._cartDbContext
            .TbTransaction
            .AsQueryable();

            if (string.IsNullOrEmpty(filterByName) == false)
            {
                query = query
                    .Where(Q => Q.Makanan.MakananName.ToLower().Contains(filterByName.ToLower()));
            }

            var founded = await query
                .Select(Q => new TransactionViewModel
                {
                    TransactionId = Q.TransactionId,
                    UserName = Q.User.UserName,
                    MakananName = Q.Makanan.MakananName,
                    Quantity = Q.Quantity,
                    TransactionDate = Q.TransactionDate
                })
                .AsNoTracking()
                .ToListAsync();

            founded = founded
                .OrderBy(Q => Q.TransactionId)
                .Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToList();


            return founded;
        }

        public int GetTotalData()
        {
            var totalData = this._cartDbContext
                .TbTransaction
                .Count();

            return totalData;
        }

    }
}
