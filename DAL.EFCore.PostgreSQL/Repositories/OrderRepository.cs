using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EFCore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopContext _context;
        public OrderRepository(ShopContext context)
        {
            _context = context;
        }

        public void CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Order Get(int id)
        {
            return _context.Orders.First(or => or.Number == id);
        }

        public Order Get(int id, List<string> include)
        {
            var queryable = _context.Orders.AsQueryable();

            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(or => or.Number == id);
        }

        public Order Get(Expression<Func<Order, bool>> expression)
        {
            return _context.Orders.First(expression);
        }

        public Order Get(Expression<Func<Order, bool>> expression, List<string> include)
        {
            var queryable = _context.Orders.AsQueryable();

            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(expression);
        }

        public Task<Order> GetAsync(int id, List<string> include)
        {
            return Task.FromResult(Get(id, include));
        }

        public Task<Order> GetAsync(Expression<Func<Order, bool>> expression)
        {
            return Task.FromResult(Get(expression));
        }

        public Task<Order> GetAsync(Expression<Func<Order, bool>> expression, List<string> include)
        {
            return Task.FromResult(Get(expression, include));
        }

        public Task<Order> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }
    }
}
