using DAL.Abstractions.Interfaces.Repositories;
using DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DAL.EFCore.PostgreSQL;

namespace DAL.EFCore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopContext _context;
        public OrderRepository(ShopContext context)
        {
            _context = context;
        }

        public long CreateOrder(Order order)
        {
            var entry = _context.Orders.Add(order);

            _context.SaveChanges();
            order = entry.Entity;

            return (long)entry.Entity.Number;
        }

        public Task<long> CreateOrderAsync(Order order)
        {
            return Task.FromResult(CreateOrder(order));
        }

        public Order Get(int id)
        {
            return _context.Orders.FirstOrDefault(or => or.Number == id);
        }

        public Order Get(int id, List<string> include)
        {
            var queryable = _context.Orders.AsQueryable();

            foreach (var item in include)
            {
                queryable = queryable.Include(item);
            }

            return queryable.FirstOrDefault(or => or.Number == id);
        }

        public Order Get(Expression<Func<Order, bool>> expression)
        {
            return _context.Orders.FirstOrDefault(expression);
        }

        public Order Get(Expression<Func<Order, bool>> expression, List<string> include)
        {
            var queryable = _context.Orders.AsQueryable();

            foreach (var item in include)
            {
                queryable = queryable.Include(item);
            }

            return queryable.FirstOrDefault(expression);
        }

        public async Task<ICollection<Order>> GetAllAsync(Expression<Func<Order, bool>> expression)
        {
            return await _context.Orders.Where(expression).ToListAsync();
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
