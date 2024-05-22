using Fiorello_PB101.Services.Interfaces;
using Fiorello_PB101.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fiorello_PB101.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _accessor;

        public CartController(
            IProductService productService,
            IHttpContextAccessor accessor
            )
        {
            _productService = productService;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas;

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            var addedProducts = await _productService.GetAllAddedBasketAsync();

            var response = addedProducts.Select(m => new BasketDetailVM
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                Category = m.Category.Name,
                ProductImages = m.ProductImages,
                Count = basketDatas.FirstOrDefault(b => b.Id == m.Id).Count,
            });

            ViewBag.TotalPrice = basketDatas.Sum(b => b.Count * b.Price);

            return View(response);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();

            List<BasketVM> basketDatas;

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            var existBasketData = basketDatas.FirstOrDefault(m => m.Id == id);

            if (existBasketData is null) return NotFound();

            basketDatas.Remove(existBasketData);

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            int count = basketDatas.Sum(m => m.Count);
            decimal totalPrice = basketDatas.Sum(m => m.Count * m.Price);

            return Ok(new { count, totalPrice });
        }
    }
}
