using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices _productServices;
        private readonly ICartService _cartService;
        public HomeController(ILogger<HomeController> logger, IProductServices productServices, ICartService cartService)
        {
            _logger = logger;
            _productServices = productServices;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var list = new List<ProductDto>();
            var responce = await _productServices.GetAllProductAsync<ResponceDto>("");
            if (responce != null && responce.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responce.Result));
            }

            return View(list);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            var model = new ProductDto();
            var responce = await _productServices.GetProductByIdAsync<ResponceDto>(productId, "");
            if (responce != null && responce.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responce.Result));
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> Details(ProductDto productDto)
        {
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value,
                    CouponCode = null
                }
            };

            CartDetailsDto cartDetailsDto = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };

            var responce = await _productServices.GetProductByIdAsync<ResponceDto>(productDto.ProductId, "");
            if (responce != null && responce.IsSuccess)
            {
                cartDetailsDto.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responce.Result));
            }
            List<CartDetailsDto> cartDetailsDtos = new();
            cartDetailsDtos.Add(cartDetailsDto);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResponce = await _cartService.AddToCartAsync<ResponceDto>(cartDto, accessToken);
            if (addToCartResponce != null && addToCartResponce.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}