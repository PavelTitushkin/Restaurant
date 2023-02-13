using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        protected ResponceDto _responce;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            this._responce = new ResponceDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                _responce.Result = cartDto;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost("AddCart")]
        [Authorize]
        public async Task<object> AddCart([FromBody] CartDto cartDto)
        {
            try
            {
                var dto = await _cartRepository.CreateUpdateCart(cartDto);
                _responce.Result = dto;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UodateCart([FromBody]CartDto cartDto)
        {
            try
            {
                var dto = await _cartRepository.CreateUpdateCart(cartDto);
                _responce.Result = dto;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody]int cartId)
        {
            try
            {
                var isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _responce.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }
    }
}
