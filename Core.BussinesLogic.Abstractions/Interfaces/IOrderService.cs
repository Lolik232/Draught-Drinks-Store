using Core.BussinesLogic.Abstractions.Entities.Order;
using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;

namespace Core.BussinesLogic.Abstractions.Interfaces;

public interface IOrderService
{
    Task<Result<ICollection<Order>>> GetOrdersForUserAsync(int userId);
    Task<Result<Order>> GetOrderAsync(int number);
    Task<Result<OrderCreated>> MakeOrderAsync(int userId, CreateOrder order, string returnUrl);
}
