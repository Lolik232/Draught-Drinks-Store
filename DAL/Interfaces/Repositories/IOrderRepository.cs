using DAL.EFCore.PostgreSQL;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseGetFiltered<Order, int>
    {
        void CreateOrder(Order order);
    }
}
