using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EFCore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopContext _context;

        public ProductRepository(ShopContext context)
        {
            _context = context;
        }

        public Product Get(Expression<Func<Product, bool>> expression)
        {
            return _context.Products.First(expression);
        }

        public Product Get(Expression<Func<Product, bool>> expression, List<string> include)
        {
            var queryable = _context.Products.AsQueryable();
            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(expression);
        }

        public Product Get(int id)
        {
            return _context.Products.First(us => us.Id == id);
        }

        public Product Get(int id, List<string> include)
        {
            var queryable = _context.Products.AsQueryable();
            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(us => us.Id == id);
        }

        public IEnumerable<Product> GetAll(int offset, int count)
        {
            return _context.Products.Skip(offset).Take(count).ToList();
        }

        public IEnumerable<Product> GetAll(
            int offset,
            int count, Expression<Func<Product, bool>> expression,
            string sorting,
            List<string> include)
        {
            var queryable = _context.Products.AsQueryable();
            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.Skip(offset).Take(count).Where(expression).ToList();
        }

        public Task<IEnumerable<Product>> GetAllAsync(int offset, int count)
        {
            return Task.FromResult(GetAll(offset, count));
        }

        public Task<IEnumerable<Product>> GetAllAsync(int offset, int count, Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
            //return Task.FromResult(GetAll(offset, count, expression));
        }

        public Task<Product> GetAsync(Expression<Func<Product, bool>> expression)
        {
            return Task.FromResult(Get(expression));
        }

        public Task<Product> GetAsync(Expression<Func<Product, bool>> expression, List<string> include)
        {
            return Task.FromResult(Get(expression, include));
        }

        public Task<Product> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }

        public Task<Product> GetAsync(int id, List<string> include)
        {
            return Task.FromResult(Get(id, include));
        }
    }
}
