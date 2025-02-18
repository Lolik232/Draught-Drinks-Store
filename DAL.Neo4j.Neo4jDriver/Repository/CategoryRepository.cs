using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Repositories;

namespace DAL.Neo4j.Neo4jDriver.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public ICollection<Category> AllSubcategories(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Category> AllSubcategories(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Category>> AllSubcategoriesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Category>> AllSubcategoriesAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Category Get(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
