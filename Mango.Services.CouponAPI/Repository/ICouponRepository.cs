using Mango.Services.CouponAPI.Models.DTO;

namespace Mango.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCodeAsync(string couponCode);
    }
}
