using DAL.Abstractions.Entities;
using System.Linq.Expressions;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseGetFiltered<Order, int>
    {
        long CreateOrder(Order order);
        Task<long> CreateOrderAsync(Order order);

        Task<ICollection<Order>> GetAllAsync(Expression<Func<Order, bool>> expression);
    }
}
