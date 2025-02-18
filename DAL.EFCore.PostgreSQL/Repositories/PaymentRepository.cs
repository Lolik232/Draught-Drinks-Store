using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EFCore.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ShopContext _context;

        public PaymentRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Payment payment)
        {
            var entry = await _context.Payments.AddAsync(payment);

            await _context.SaveChangesAsync();

            return (int)entry.Entity.Id;
        }

        public Payment Get(int id)
        {
            return _context.Payments.FirstOrDefault(or => or.Id == id);
        }

        public Payment Get(int id, List<string> include)
        {
            var queryable = _context.Payments.AsQueryable();

            foreach (var item in include)
            {
                queryable = queryable.Include(item);
            }

            return queryable.FirstOrDefault(or => or.Id == id);
        }

        public Payment Get(Expression<Func<Payment, bool>> expression)
        {
            return _context.Payments.FirstOrDefault(expression);
        }

        public Payment Get(Expression<Func<Payment, bool>> expression, List<string> include)
        {
            var queryable = _context.Payments.AsQueryable();

            foreach (var item in include)
            {
                queryable = queryable.Include(item);
            }

            return queryable.FirstOrDefault(expression);
        }

        public async Task<ICollection<Payment>> GetAllAsync(Expression<Func<Payment, bool>> expression)
        {
            return await _context.Payments.Where(expression).ToListAsync();
        }

        public Task<Payment> GetAsync(int id, List<string> include)
        {
            return Task.FromResult(Get(id, include));
        }

        public Task<Payment> GetAsync(Expression<Func<Payment, bool>> expression)
        {
            return Task.FromResult(Get(expression));
        }

        public Task<Payment> GetAsync(Expression<Func<Payment, bool>> expression, List<string> include)
        {
            return Task.FromResult(Get(expression, include));
        }

        public Task<Payment> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }
    }
}
