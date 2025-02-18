using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;

namespace Core.BussinesLogic.Abstractions.Interfaces;
public interface IProductService
{
    Task<Result<ICollection<Product>>> GetAllProductsAsync(int offset, int count);
    Task<Result<ICollection<Product>>> GetAllProductsByCategoryAsync(int offset, int count, int categoryId);
    Task<Result<ICollection<Product>>> GetAllProductsByCategoryAsync(int offset, int count, string categoryName);

    Task<Result<Product>> GetProductByIdAsync(int productId);


}
