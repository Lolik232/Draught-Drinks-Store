using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EFCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopContext _context;

        public UserRepository(ShopContext context)
        {
            _context = context;
        }

        public User Get(Expression<Func<User, bool>> expression)
        {
            return _context.Users.First(expression);
        }

        public User Get(Expression<Func<User, bool>> expression, List<string> include)
        {
            var queryable = _context.Users.AsQueryable();
            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(expression);
        }

        public User Get(int id)
        {
            return _context.Users.First(us => us.Id == id);
        }

        public User Get(int id, List<string> include)
        {
            var queryable = _context.Users.AsQueryable();
            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(us => us.Id == id);
        }

        public Task<User> GetAsync(Expression<Func<User, bool>> expression)
        {
            return Task.FromResult(Get(expression));
        }

        public Task<User> GetAsync(Expression<Func<User, bool>> expression, List<string> include)
        {
            return Task.FromResult(Get(expression, include));
        }

        public Task<User> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }

        public Task<User> GetAsync(int id, List<string> include)
        {
            return Task.FromResult<User>(Get(id, include));
        }
    }
}
