using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DAL.Abstractions.Entities;

namespace DAL.EFCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopContext _context;

        public UserRepository(ShopContext context)
        {
            _context = context;
        }

        public long CreateUser(User user)
        {
            if (_context.Products.Count() == 0)
            {
                _context.Products.AddRange(
                    new List<DAL.Abstractions.Entities.Product> {
                            new(){
                            ImageUrl = "https://alivaria.by/media/41457/baltika_3_05l_2016.png?height=1140&mode=max",
                            CategoryId = 6,
                            Name = "Балтика 3",
                            Price = 67.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 100}
                            },
                             new(){
                            ImageUrl = "https://forumsamogon.ru/wp-content/uploads/3/8/4/384623521015aef85ed483c61c3d9032.jpg",
                            CategoryId = 7,
                            Name = "Балтика 9",
                            Price = 90.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 50}
                            },
                                new(){
                            ImageUrl = "https://lexa.bonsai-sushi.ru/wp-content/uploads/2021/12/Coca-Cola_0-5.jpg",
                            CategoryId = 4,
                            Name = "Кола",
                            Price = 100.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 150}
                            },
                             new(){
                            ImageUrl = "https://vol.selhozproduct.ru/upload/usl/f_625f096a16946.jpg",
                            CategoryId = 3,
                            Name = "Сосиски",
                            Price = 150.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 150}
                            },  new(){
                            ImageUrl = "https://www.positive-market.ru/f/store/offers/m9fz5tf8xvcv8ffs.jpg",
                            CategoryId = 5,
                            Name = "Байкал",
                            Price = 50.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 40}
                            }, new(){
                            ImageUrl = "http://home.nubo.ru/pavel_egorov/russian/rdm0501.jpg",
                            CategoryId = 6,
                            Name = "Фирменное пиво \"Кузьмич\"",
                            Price = 130.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 50}
                            },
                    }
                    );
                _context.SaveChanges();
            }

            var entry = _context.Users.Add(user);
            _context.SaveChanges();
            return entry.Entity.Id;
        }

        public Task<long> CreateUserAsync(User user)
        {
            return Task.FromResult(CreateUser(user));
        }

        public User? Get(Expression<Func<User, bool>> expression)
        {
            return _context.Users.FirstOrDefault(expression);
        }

        public User? Get(Expression<Func<User, bool>> expression, List<string> include)
        {
            var queryable = _context.Users.AsQueryable();
            foreach (var item in include)
            {
                queryable = queryable.Include(item);
            }

            return queryable.FirstOrDefault(expression);
        }

        public User? Get(int id)
        {
            return _context.Users.FirstOrDefault(us => us.Id == id);
        }

        public User? Get(int id, List<string> include)
        {
            var queryable = _context.Users.AsQueryable();
            foreach (var item in include)
            {
                queryable = queryable.Include(item);
            }

            return queryable.FirstOrDefault(us => us.Id == id);
        }

        public Task<User?> GetAsync(Expression<Func<User, bool>> expression)
        {
            return Task.FromResult(Get(expression));
        }

        public Task<User?> GetAsync(Expression<Func<User, bool>> expression, List<string> include)
        {
            return Task.FromResult(Get(expression, include));
        }

        public Task<User?> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }

        public Task<User?> GetAsync(int id, List<string> include)
        {
            return Task.FromResult<User>(Get(id, include));
        }
    }
}
