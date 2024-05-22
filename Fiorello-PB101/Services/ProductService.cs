using Fiorello_PB101.Data;
using Fiorello_PB101.Models;
using Fiorello_PB101.Services.Interfaces;
using Fiorello_PB101.ViewModels.Baskets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fiorello_PB101.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;


        public ProductService(
            AppDbContext context,
            IHttpContextAccessor accessor
            )
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(m => m.ProductImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAddedBasketAsync()
        {
            List<BasketVM> basketDatas = new();
            List<Product> result = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            foreach (var item in basketDatas)
            {
                result
                    .Add(await _context.Products
                    .Where(m => m.Id == item.Id)
                    .Include(m => m.Category)
                    .Include(m => m.ProductImages)
                    .FirstOrDefaultAsync());
            }

            return result;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetByIdWithAllDatasAsync(int id)
        {
            return await _context.Products
                .Where(m => m.Id == id)
                .Include(m => m.Category)
                .Include(m => m.ProductImages)
                .FirstOrDefaultAsync();
        }
    }
}
