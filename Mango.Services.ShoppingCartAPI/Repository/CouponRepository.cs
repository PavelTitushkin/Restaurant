using Mango.Services.ShoppingCartAPI.Models.DTO;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDto> GetCouponAsync(string couponName)
        {
            var responce = await _httpClient.GetAsync($"/api/coupon/{couponName}");
            var apiContent  = await responce.Content.ReadAsStringAsync();
            var responceDto = JsonConvert.DeserializeObject<ResponceDto>(apiContent);
            if (responceDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responceDto.Result));
            }

            return new CouponDto();
        }
    }
}
