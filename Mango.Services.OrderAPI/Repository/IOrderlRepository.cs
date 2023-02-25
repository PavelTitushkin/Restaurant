using Mango.Services.OrderAPI.Models;

namespace Mango.Services.OrderAPI.Repository
{
    public interface IOrderlRepository
    {
        Task<bool> AddOrderAsync(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
    }
}
