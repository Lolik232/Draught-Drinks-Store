using Core.BussinesLogic.Abstractions.Interfaces;
using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Repositories;
using System.Reflection;
using System.Xml.Linq;

namespace Core.BussinesLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<ICollection<int>>> AllSubcategoriesIdsAsync(string name)
        {
            try
            {
                var result = await _categoryRepository.AllSubcategoriesIdsAsync(name);

                if (result is null)
                {
                    return Result.Failture<ICollection<int>>(Error.NullValue);
                }

                return Result.Sucsess(result);
            }
            catch (Exception)
            {
                return Result.Failture<ICollection<int>>(Error.NullValue);
            }
        }

        public async Task<Result<ICollection<int>>> AllSubcategoriesIdsAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.AllSubcategoriesIdsAsync(id);

                if (result is null)
                {
                    return Error.NullValue;
                }

                return Result.Sucsess(result);
            }
            catch (Exception)
            {
                return Result.Failture<ICollection<int>>(Error.NullValue);
            }
        }

        public async Task<Result<ICollection<Category>>> GetAllAsync()
        {
            try
            {
                var result = await _categoryRepository.GetAllAsync();

                if (result is null)
                {
                    return Result.Failture<ICollection<Category>>(Error.NullValue);
                }

                return Result.Sucsess(result);
            }
            catch (Exception)
            {
                return Result.Failture<ICollection<Category>>(Error.NullValue);
            }
        }
    }
}
