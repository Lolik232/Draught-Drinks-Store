using DAL.Abstractions.Entities;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface ICategoryRepository : IBaseGetRepository<Category, int>
    {
        Task<ICollection<int>?> AllSubcategoriesIdsAsync(string name);
        Task<ICollection<int>?> AllSubcategoriesIdsAsync(int id);


        Task<ICollection<Category>?> GetAllAsync();
    }

}
