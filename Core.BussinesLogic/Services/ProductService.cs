using Core.BussinesLogic.Abstractions.Interfaces;
using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Repositories;

namespace Core.BussinesLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }

        public async Task<Result<ICollection<Product>>> GetAllProductsAsync(int offset, int count)
        {
            var res = await _productRepository.GetAllAsync(offset, count);

            return Result.Sucsess<ICollection<Product>>(res.ToList());
        }

        public async Task<Result<ICollection<Product>>> GetAllProductsByCategoryAsync(int offset, int count, int categoryId)
        {
            var categoriesResult = await _categoryService.AllSubcategoriesIdsAsync(categoryId);
            if (categoriesResult.IsFailture) return categoriesResult.Error;


            var res = await _productRepository.GetAllAsync(offset, count, pr => categoriesResult.Value.Contains((int)pr.CategoryId));

            return Result.Sucsess<ICollection<Product>>(res.ToList());
        }

        public async Task<Result<ICollection<Product>>> GetAllProductsByCategoryAsync(int offset, int count, string categoryName)
        {
            var categoriesResult = await _categoryService.AllSubcategoriesIdsAsync(categoryName);
            if (categoriesResult.IsFailture) return categoriesResult.Error;


            var res = await _productRepository.GetAllAsync(offset, count, pr => categoriesResult.Value.Contains((int)pr.CategoryId));

            return Result.Sucsess<ICollection<Product>>(res.ToList());
        }

        public async Task<Result<Product>> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetAsync(productId);

            return Result.Sucsess<Product>(product);
        }
    }
}
