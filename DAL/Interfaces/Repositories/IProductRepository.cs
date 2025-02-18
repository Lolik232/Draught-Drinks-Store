using DAL.Abstractions.Entities;
using System.Linq.Expressions;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IProductRepository : IBaseGetFiltered<Product, int>.WithGetAll
    {
    }
}
