using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        public async Task<IActionResult> ProductIndex()
        {
            var list = new List<ProductDto>();
            var response = await _productServices.GetAllProductAsync<ResponceDto>();
            if (response !=null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid) 
            {
                var response = await _productServices.CreateProductAsync<ResponceDto>(model);
                if(response != null && response.IsSuccess) 
                {
                    return RedirectToAction("ProductIndex");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productServices.GetProductByIdAsync<ResponceDto>(productId);

            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productServices.UpdateProductAsync<ResponceDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("ProductIndex");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var response = await _productServices.GetProductByIdAsync<ResponceDto>(productId);

            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productServices.DeleteProductAsync<ResponceDto>(model.ProductId);
                if (response.IsSuccess)
                {
                    return RedirectToAction("ProductIndex");
                }
            }
            return View(model);
        }
    }
}
