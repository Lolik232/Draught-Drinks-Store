using DAL.Abstractions.Entities;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface ICategoryRepository : IBaseGetRepository<Category, int>
    {

        ICollection<Category> AllSubcategories(string name);
        Task<ICollection<Category>> AllSubcategoriesAsync(string name);

        ICollection<Category> AllSubcategories(int id);
        Task<ICollection<Category>> AllSubcategoriesAsync(int id);

        ICollection<Category> GetAll();
        Task<ICollection<Category>> GetAllAsync();
    }

}
