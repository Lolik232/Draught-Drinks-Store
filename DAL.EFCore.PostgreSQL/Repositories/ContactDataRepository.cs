using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EFCore.Repositories
{
    public class ContactDataRepository : IContactDataRepository
    {
        private readonly ShopContext _context;

        public ContactDataRepository(ShopContext context)
        {
            _context = context;
        }

        public ContactData Get(int id, List<string> include)
        {
            throw new NotImplementedException();
        }

        public ContactData Get(Expression<Func<ContactData, bool>> expression)
        {
            return _context.ContactData.First(expression);
        }

        public ContactData Get(Expression<Func<ContactData, bool>> expression, List<string> include)
        {
            var queryable = _context.ContactData.AsQueryable();
            foreach (var item in include)
            {
                queryable.Include(item);
            }

            return queryable.First(expression);
        }

        public ContactData Get(int id)
        {
            return _context.ContactData.First(cd => cd.Id == id);
        }

        public Task<ContactData> GetAsync(int id, List<string> include)
        {
           return Task.FromResult(Get(id, include));
        }

        public Task<ContactData> GetAsync(Expression<Func<ContactData, bool>> expression)
        {
            return Task.FromResult(Get(expression));
        }

        public Task<ContactData> GetAsync(Expression<Func<ContactData, bool>> expression, List<string> include)
        {
            return Task.FromResult(Get(expression, include));
        }

        public Task<ContactData> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }
    }
}
