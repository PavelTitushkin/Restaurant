using Mango.Services.ShoppingCartAPI.Messages;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        protected ResponceDto _responce;

        public CartAPIController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            this._responce = new ResponceDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserIdAsync(userId);
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
                var dto = await _cartRepository.CreateUpdateCartAsync(cartDto);
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
                var dto = await _cartRepository.CreateUpdateCartAsync(cartDto);
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
                var isSuccess = await _cartRepository.RemoveFromCartAsync(cartId);
                _responce.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var isSuccess = await _cartRepository.ApplyCouponAsync(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                _responce.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                var isSuccess = await _cartRepository.RemoveCouponAsync(userId);
                _responce.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserIdAsync(checkoutHeader.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }
                checkoutHeader.CartDetails = cartDto.CartDetails;
                //logic to add message to process order
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
